using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.AspApi.Services
{
    // ── UC17: JWT Service ─────────────────────────────────────────────────
    //
    // JWT = JSON Web Token
    // A JWT is a compact, self-contained token that proves who the user is.
    //
    // Structure of a JWT:
    //   Header.Payload.Signature
    //   eyJhbGci...  eyJ1c2Vy...  abc123...
    //
    // Payload contains "claims" — facts about the user:
    //   - sub  (subject)  = user ID
    //   - name            = username
    //   - email           = email
    //   - role            = "User" or "Admin"
    //   - exp             = expiry time
    //
    // The Signature proves the token has not been tampered with.
    // It is created using a secret key that only the server knows.
    //
    // Flow:
    //   1. User signs in  → server creates a JWT and returns it
    //   2. User calls API → sends JWT in the Authorization header
    //   3. Server checks  → verifies the signature and reads the claims
    //   4. If valid       → allows the request; if not → 401 Unauthorized

    public class JwtService
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int    _expiryMinutes;

        // Settings come from appsettings.json via IConfiguration
        public JwtService(IConfiguration configuration)
        {
            _secretKey     = configuration["Jwt:SecretKey"]
                             ?? throw new InvalidOperationException("Jwt:SecretKey is missing from appsettings.json");
            _issuer        = configuration["Jwt:Issuer"]   ?? "QuantityMeasurementAPI";
            _audience      = configuration["Jwt:Audience"] ?? "QuantityMeasurementClient";
            _expiryMinutes = int.Parse(configuration["Jwt:ExpiryMinutes"] ?? "60");
        }

        // ── Generate a JWT token for a user ──────────────────────────────

        public (string token, DateTime expiresAt) GenerateToken(UserEntity user)
        {
            // Claims = facts we embed into the token about this user
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name,  user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role,               user.Role),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()) // unique token ID
            };

            // Create signing key from our secret
            var key         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.UtcNow.AddMinutes(_expiryMinutes);

            // Build the token
            var token = new JwtSecurityToken(
                issuer:             _issuer,
                audience:           _audience,
                claims:             claims,
                notBefore:          DateTime.UtcNow,
                expires:            expiresAt,
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expiresAt);
        }

        // ── Validate a token and extract claims ───────────────────────────
        // Returns the ClaimsPrincipal if valid, null if invalid/expired.

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,      // checks expiry
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = _issuer,
                    ValidAudience            = _audience,
                    IssuerSigningKey         = key,
                    ClockSkew                = TimeSpan.Zero  // no grace period on expiry
                };

                return new JwtSecurityTokenHandler()
                    .ValidateToken(token, parameters, out _);
            }
            catch
            {
                return null;  // invalid or expired token
            }
        }
    }
}