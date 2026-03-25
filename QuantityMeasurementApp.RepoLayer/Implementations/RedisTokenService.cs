using StackExchange.Redis;
using Microsoft.Extensions.Logging;

namespace QuantityMeasurementApp.AspApi.Security
{
    // Redis token service - all methods are SAFE when Redis is offline.
    // If Redis is not running, every method silently returns a safe default
    // instead of throwing an exception and crashing the request.
    public class RedisTokenService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<RedisTokenService> _logger;

        private const string ActivePrefix    = "token:active:";
        private const string BlacklistPrefix = "token:blacklist:";
        private const string RefreshPrefix   = "token:refresh:";
        private const string RateLimitPrefix = "ratelimit:login:";

        public RedisTokenService(
            IConnectionMultiplexer redis,
            ILogger<RedisTokenService> logger)
        {
            _redis  = redis;
            _logger = logger;
        }

        private IDatabase? GetDb()
        {
            try
            {
                if (!_redis.IsConnected) return null;
                return _redis.GetDatabase();
            }
            catch { return null; }
        }

        // Store active token - silently skips if Redis offline
        public async Task StoreActiveTokenAsync(string jti, string username, TimeSpan expiry)
        {
            try
            {
                var db = GetDb();
                if (db == null) return;
                await db.StringSetAsync(ActivePrefix + jti, username, expiry);
            }
            catch (Exception ex) { _logger.LogWarning(ex, "Redis StoreActiveToken failed"); }
        }

        // Returns TRUE (allow request) if Redis is offline - fail open
        public async Task<bool> IsTokenActiveAsync(string jti)
        {
            try
            {
                var db = GetDb();
                if (db == null) return true;   // Redis offline = allow request
                bool exists      = await db.KeyExistsAsync(ActivePrefix + jti);
                bool blacklisted = await IsTokenBlacklistedAsync(jti);
                return exists && !blacklisted;
            }
            catch { return true; }   // fail open - don't block user if Redis fails
        }

        public async Task BlacklistTokenAsync(string jti, TimeSpan remainingExpiry)
        {
            try
            {
                var db = GetDb();
                if (db == null) return;
                await db.KeyDeleteAsync(ActivePrefix + jti);
                await db.StringSetAsync(BlacklistPrefix + jti, "blacklisted", remainingExpiry);
            }
            catch (Exception ex) { _logger.LogWarning(ex, "Redis BlacklistToken failed"); }
        }

        public async Task<bool> IsTokenBlacklistedAsync(string jti)
        {
            try
            {
                var db = GetDb();
                if (db == null) return false;
                return await db.KeyExistsAsync(BlacklistPrefix + jti);
            }
            catch { return false; }
        }

        public async Task StoreRefreshTokenAsync(string username, string refreshToken, TimeSpan expiry)
        {
            try
            {
                var db = GetDb();
                if (db == null) return;
                string hashed = Sha256Hasher.HashWithSalt(refreshToken, username);
                await db.StringSetAsync(RefreshPrefix + username, hashed, expiry);
            }
            catch (Exception ex) { _logger.LogWarning(ex, "Redis StoreRefreshToken failed"); }
        }

        public async Task<bool> ValidateRefreshTokenAsync(string username, string refreshToken)
        {
            try
            {
                var db = GetDb();
                if (db == null) return false;
                RedisValue stored = await db.StringGetAsync(RefreshPrefix + username);
                if (!stored.HasValue) return false;
                string hashed = Sha256Hasher.HashWithSalt(refreshToken, username);
                return stored.ToString() == hashed;
            }
            catch { return false; }
        }

        public async Task DeleteRefreshTokenAsync(string username)
        {
            try
            {
                var db = GetDb();
                if (db == null) return;
                await db.KeyDeleteAsync(RefreshPrefix + username);
            }
            catch (Exception ex) { _logger.LogWarning(ex, "Redis DeleteRefreshToken failed"); }
        }

        // Returns 0 (no failures) if Redis is offline - so login is never blocked
        public async Task<long> GetFailedLoginCountAsync(string username)
        {
            try
            {
                var db = GetDb();
                if (db == null) return 0;
                RedisValue val = await db.StringGetAsync(RateLimitPrefix + username);
                return val.HasValue ? (long)val : 0;
            }
            catch { return 0; }
        }

        public async Task<long> IncrementFailedLoginAsync(string username)
        {
            try
            {
                var db = GetDb();
                if (db == null) return 0;
                long count = await db.StringIncrementAsync(RateLimitPrefix + username);
                if (count == 1) await db.KeyExpireAsync(RateLimitPrefix + username, TimeSpan.FromMinutes(15));
                return count;
            }
            catch { return 0; }
        }

        public async Task ResetFailedLoginAsync(string username)
        {
            try
            {
                var db = GetDb();
                if (db == null) return;
                await db.KeyDeleteAsync(RateLimitPrefix + username);
            }
            catch (Exception ex) { _logger.LogWarning(ex, "Redis ResetFailedLogin failed"); }
        }

        public async Task<bool> IsConnectedAsync()
        {
            try
            {
                if (!_redis.IsConnected) return false;
                var db = _redis.GetDatabase();
                await db.PingAsync();
                return true;
            }
            catch { return false; }
        }
    }
}