using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.AspApi.DTO;
using QuantityMeasurementApp.AspApi.Security;
using QuantityMeasurementApp.AspApi.Services;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.AspApi.Controllers
{
    // ── UC17: User Controller ─────────────────────────────────────────────
    //
    // Security applied:
    //   PasswordHasher  → BCrypt + explicit salt (password is NEVER plain text)
    //   Sha256Hasher    → refresh token stored hashed in Redis
    //   RedisTokenService → token whitelist, blacklist, rate limiting
    //
    // Email is stored as plain text in SQL Server (no encryption).

    [ApiController]
    [Route("api/v1/users")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository         _userRepository;
        private readonly JwtService              _jwtService;
        private readonly RedisTokenService       _redisTokenService;
        private readonly ILogger<UserController> _logger;

        private const int MaxFailedAttempts = 5;

        public UserController(
            IUserRepository          userRepository,
            JwtService               jwtService,
            RedisTokenService        redisTokenService,
            ILogger<UserController>  logger)
        {
            _userRepository    = userRepository;
            _jwtService        = jwtService;
            _redisTokenService = redisTokenService;
            _logger            = logger;
        }

        // ── POST /api/v1/users/signup ─────────────────────────────────────
        // Registers a new user.
        // Password is hashed with BCrypt + explicit salt before saving.
        // Email is stored as plain text.
        //
        // CURL:
        //   curl -X POST http://localhost:5271/api/v1/users/signup
        //        -H "Content-Type: application/json"
        //        -d '{"username":"anamika","email":"a@email.com","password":"Secret123!"}'
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDTO request)
        {
            try
            {
                // Check username and email are not already taken
                if (await _userRepository.UsernameExistsAsync(request.Username))
                    return BadRequest(new { message = $"Username '{request.Username}' is already taken." });

                if (await _userRepository.EmailExistsAsync(request.Email))
                    return BadRequest(new { message = $"Email '{request.Email}' is already registered." });

                // Hash password with BCrypt + explicit salt
                // Format stored: SALT||BCRPTHASH
                // e.g. A3B9C1D2...||$2a$12$...
                string passwordHash = PasswordHasher.HashPassword(request.Password);

                var user = new UserEntity
                {
                    Username     = request.Username,
                    Email        = request.Email,      // plain text - no encryption
                    PasswordHash = passwordHash,        // BCrypt + salt
                    Role         = "User",
                    CreatedAt    = DateTime.UtcNow
                };

                // Save to SQL Server via EF Core
                await _userRepository.SaveAsync(user);

                // Issue JWT and store in Redis whitelist
                var (token, expiresAt) = _jwtService.GenerateToken(user);
                string jti = GetJtiFromToken(token);
                await _redisTokenService.StoreActiveTokenAsync(
                    jti, user.Username, expiresAt - DateTime.UtcNow);

                // Issue refresh token (stored hashed in Redis)
                string refreshToken = GenerateRefreshToken();
                await _redisTokenService.StoreRefreshTokenAsync(
                    user.Username, refreshToken, TimeSpan.FromDays(7));

                _logger.LogInformation("New user registered: {Username}", user.Username);

                return StatusCode(201, new
                {
                    token        = token,
                    refreshToken = refreshToken,
                    username     = user.Username,
                    email        = user.Email,
                    role         = user.Role,
                    expiresAt    = expiresAt,
                    message      = "Account created successfully."
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SignUp failed for {Username}", request.Username);
                return BadRequest(new { message = ex.Message });
            }
        }

        // ── POST /api/v1/users/signin ─────────────────────────────────────
        // Sign in with username + password.
        // Rate limited via Redis (max 5 failed attempts per 15 minutes).
        // Password verified with BCrypt.VerifyPassword().
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDTO request)
        {
            try
            {
                // Redis rate limiting
                long failedAttempts = await _redisTokenService
                    .GetFailedLoginCountAsync(request.Username);

                if (failedAttempts >= MaxFailedAttempts)
                    return StatusCode(429, new
                    {
                        message = "Too many failed login attempts. Try again in 15 minutes."
                    });

                // Find user in SQL Server
                var user = await _userRepository.FindByUsernameAsync(request.Username);
                if (user == null)
                {
                    await _redisTokenService.IncrementFailedLoginAsync(request.Username);
                    return Unauthorized(new { message = "Invalid username or password." });
                }

                // Verify BCrypt + salt password
                bool passwordValid = PasswordHasher.VerifyPassword(
                    request.Password, user.PasswordHash);

                if (!passwordValid)
                {
                    long attempts  = await _redisTokenService
                        .IncrementFailedLoginAsync(request.Username);
                    int  remaining = MaxFailedAttempts - (int)attempts;
                    return Unauthorized(new
                    {
                        message           = "Invalid username or password.",
                        attemptsRemaining = remaining > 0 ? remaining : 0
                    });
                }

                // Success — reset Redis rate limit counter
                await _redisTokenService.ResetFailedLoginAsync(request.Username);

                // Issue JWT and store in Redis
                var (token, expiresAt) = _jwtService.GenerateToken(user);
                string jti = GetJtiFromToken(token);
                await _redisTokenService.StoreActiveTokenAsync(
                    jti, user.Username, expiresAt - DateTime.UtcNow);

                // Issue refresh token
                string refreshToken = GenerateRefreshToken();
                await _redisTokenService.StoreRefreshTokenAsync(
                    user.Username, refreshToken, TimeSpan.FromDays(7));

                _logger.LogInformation("User signed in: {Username}", user.Username);

                return Ok(new
                {
                    token        = token,
                    refreshToken = refreshToken,
                    username     = user.Username,
                    email        = user.Email,
                    role         = user.Role,
                    expiresAt    = expiresAt,
                    message      = "Sign in successful."
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SignIn failed for {Username}", request.Username);
                return BadRequest(new { message = ex.Message });
            }
        }

        // ── POST /api/v1/users/logout ─────────────────────────────────────
        // Blacklists the JWT in Redis so it cannot be reused even if still valid.
        // Without Redis this is skipped — token expires naturally after 60 min.
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            string? jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            if (jti == null)
                return BadRequest(new { message = "Token missing jti claim." });

            await _redisTokenService.BlacklistTokenAsync(jti, TimeSpan.FromHours(1));

            string? username = User.Identity?.Name;
            if (username != null)
                await _redisTokenService.DeleteRefreshTokenAsync(username);

            _logger.LogInformation("User logged out: {Username}", username);
            return Ok(new { message = "Logged out. Token invalidated." });
        }

        // ── POST /api/v1/users/refresh ────────────────────────────────────
        // Exchange a refresh token for a new JWT without signing in again.
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO request)
        {
            bool valid = await _redisTokenService.ValidateRefreshTokenAsync(
                request.Username, request.RefreshToken);

            if (!valid)
                return Unauthorized(new { message = "Refresh token invalid or expired. Please sign in again." });

            var user = await _userRepository.FindByUsernameAsync(request.Username);
            if (user == null)
                return Unauthorized(new { message = "User not found." });

            var (token, expiresAt) = _jwtService.GenerateToken(user);
            string jti = GetJtiFromToken(token);
            await _redisTokenService.StoreActiveTokenAsync(
                jti, user.Username, expiresAt - DateTime.UtcNow);

            // Rotate refresh token
            string newRefreshToken = GenerateRefreshToken();
            await _redisTokenService.StoreRefreshTokenAsync(
                user.Username, newRefreshToken, TimeSpan.FromDays(7));

            return Ok(new
            {
                token        = token,
                refreshToken = newRefreshToken,
                expiresAt    = expiresAt,
                message      = "Token refreshed."
            });
        }

        // ── GET /api/v1/users/profile ─────────────────────────────────────
        // Returns the signed-in user's profile.
        // Checks Redis to ensure the token has not been blacklisted (logged out).
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            // Extra Redis check: is this token still active?
            string? jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            if (jti != null && !await _redisTokenService.IsTokenActiveAsync(jti))
                return Unauthorized(new { message = "Token has been invalidated. Please sign in again." });

            string? username = User.Identity?.Name;
            if (username == null) return Unauthorized();

            var user = await _userRepository.FindByUsernameAsync(username);
            if (user == null) return NotFound(new { message = "User not found." });

            return Ok(new
            {
                id        = user.Id,
                username  = user.Username,
                email     = user.Email,       // plain text
                role      = user.Role,
                createdAt = user.CreatedAt
            });
        }

        // ── GET /api/v1/users/all ─────────────────────────────────────────
        // List all users. Admin role only.
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = (await _userRepository.GetAllAsync()).Select(u => new
            {
                id        = u.Id,
                username  = u.Username,
                email     = u.Email,
                role      = u.Role,
                createdAt = u.CreatedAt
            });
            return Ok(users);
        }

        // ── GET /api/v1/users/redis-health ────────────────────────────────
        [HttpGet("redis-health")]
        public async Task<IActionResult> RedisHealth()
        {
            bool connected = await _redisTokenService.IsConnectedAsync();
            return Ok(new
            {
                redis   = connected ? "Connected" : "Disconnected",
                message = connected ? "Redis is running." : "Redis is not reachable."
            });
        }

        // ── Private helpers ───────────────────────────────────────────────

        private static string GetJtiFromToken(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwt.Claims
                      .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)
                      ?.Value ?? Guid.NewGuid().ToString();
        }

        private static string GenerateRefreshToken()
        {
            byte[] bytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(32);
            return Convert.ToHexString(bytes);
        }
    }
}