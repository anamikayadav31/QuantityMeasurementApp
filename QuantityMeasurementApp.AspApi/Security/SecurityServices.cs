using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace QuantityMeasurementApp.AspApi.Security
{
    // ════════════════════════════════════════════════════════════════════
    //  PASSWORD HASHING WITH EXPLICIT SALTING
    // ════════════════════════════════════════════════════════════════════
    //
    //  What is salting?
    //    A "salt" is a random value added to the password BEFORE hashing.
    //    This means even if two users have the same password, their hashes
    //    will be completely different.
    //
    //  Example without salt:
    //    "Secret123" → always → "abc123hash"   ← attacker can precompute
    //
    //  Example with salt:
    //    "Secret123" + "rand1" → "xyz999hash"
    //    "Secret123" + "rand2" → "qwe555hash"  ← each user gets a unique hash
    //
    //  BCrypt already embeds a salt internally, but here we ALSO prepend
    //  our own application-level salt (pepper) for an extra layer.
    //  The salt is stored alongside the hash so we can verify later.
    //
    //  Storage format:  SALT||BCRPYTHASH
    //    e.g. "a1b2c3d4||$2a$12$..."

    public class PasswordHasher
    {
        // Work factor: BCrypt will do 2^12 = 4096 rounds of hashing
        // Higher = slower = harder to brute force
        private const int    WorkFactor    = 12;
        private const string Separator     = "||";

        // ── Hash a plain text password ────────────────────────────────────

        /// <summary>
        /// Hash a password with a random salt.
        /// Returns a string containing SALT||HASH that you store in the database.
        /// </summary>
        public static string HashPassword(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                throw new ArgumentException("Password cannot be empty.");

            // Step 1: Generate a random application-level salt (32 bytes = 256 bits)
            string salt = GenerateSalt();

            // Step 2: Combine our salt with the plain password (pepper + password)
            string saltedPassword = salt + plainPassword;

            // Step 3: BCrypt hashes the already-salted password
            //         BCrypt also adds its own internal salt (embedded in the hash)
            //         WorkFactor=12 means ~250ms per hash — slow enough to deter
            //         brute force but fast enough for normal login
            string bcryptHash = BCrypt.Net.BCrypt.HashPassword(saltedPassword, WorkFactor);

            // Step 4: Store SALT||BCRPTHASH so we can recreate the salted password on verify
            return $"{salt}{Separator}{bcryptHash}";
        }

        // ── Verify a plain text password against a stored hash ────────────

        /// <summary>
        /// Verify a plain text password against a stored SALT||HASH string.
        /// Returns true if the password matches.
        /// </summary>
        public static bool VerifyPassword(string plainPassword, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(plainPassword)) return false;
            if (string.IsNullOrWhiteSpace(storedHash))   return false;

            // Split the stored value back into salt and bcrypt hash
            var parts = storedHash.Split(new[] { Separator }, StringSplitOptions.None);
            if (parts.Length != 2) return false;

            string salt      = parts[0];
            string bcryptHash = parts[1];

            // Re-combine our salt with the plain password — same as we did on hash
            string saltedPassword = salt + plainPassword;

            // BCrypt.Verify handles timing-safe comparison internally
            return BCrypt.Net.BCrypt.Verify(saltedPassword, bcryptHash);
        }

        // ── Generate a cryptographically random salt ──────────────────────

        /// <summary>
        /// Generate a random 32-character hex string to use as a salt.
        /// Uses RandomNumberGenerator — cryptographically secure.
        /// </summary>
        public static string GenerateSalt()
        {
            // 16 random bytes → 32 hex characters
            byte[] saltBytes = RandomNumberGenerator.GetBytes(16);
            return Convert.ToHexString(saltBytes);  // e.g. "A3F9C1D2..."
        }
    }


    // ════════════════════════════════════════════════════════════════════
    //  AES ENCRYPTION / DECRYPTION SERVICE
    // ════════════════════════════════════════════════════════════════════
    //
    //  What is encryption?
    //    Encryption converts readable data (plaintext) into unreadable
    //    data (ciphertext) using a key. Only someone with the key can
    //    decrypt it back to the original.
    //
    //  When to use encryption vs hashing?
    //    Hashing   = one-way (passwords). You cannot get the original back.
    //    Encryption = two-way (sensitive data like email, PII). You CAN decrypt.
    //
    //  AES-256-CBC:
    //    AES = Advanced Encryption Standard (industry standard)
    //    256 = 256-bit key (very strong)
    //    CBC = Cipher Block Chaining (each block depends on the previous)
    //
    //  IV (Initialization Vector):
    //    A random value used to start the encryption chain.
    //    A new IV is generated for EVERY encryption operation.
    //    The IV is prepended to the ciphertext so we can use it on decrypt.
    //    The IV is NOT secret — it just ensures identical inputs produce
    //    different ciphertext each time.
    //
    //  Storage format: BASE64(IV + CIPHERTEXT)

    public class AesEncryptionService
    {
        // AES-256 requires exactly 32 bytes (256 bits) for the key
        private readonly byte[] _key;

        public AesEncryptionService(IConfiguration configuration)
        {
            string? keyString = configuration["Encryption:AesKey"]
                ?? throw new InvalidOperationException(
                    "Encryption:AesKey is missing from appsettings.json");

            // Derive a 32-byte key from the config string using SHA256
            // This lets the config value be any length string
            _key = SHA256.HashData(Encoding.UTF8.GetBytes(keyString));
        }

        // ── Encrypt plaintext → Base64 ciphertext ─────────────────────────

        /// <summary>
        /// Encrypt a string. Returns Base64(IV + CipherText).
        /// A new random IV is generated each time, so identical inputs
        /// produce different outputs — this is correct and expected.
        /// </summary>
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentException("Text to encrypt cannot be empty.");

            using var aes = Aes.Create();
            aes.Key     = _key;
            aes.Mode    = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Generate a fresh random IV for this encryption
            aes.GenerateIV();
            byte[] iv = aes.IV;  // 16 bytes

            using var encryptor = aes.CreateEncryptor();
            byte[] plainBytes   = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherBytes  = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // Prepend IV to ciphertext: [IV (16 bytes)][CipherText]
            byte[] result = new byte[iv.Length + cipherBytes.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(cipherBytes, 0, result, iv.Length, cipherBytes.Length);

            return Convert.ToBase64String(result);
        }

        // ── Decrypt Base64 ciphertext → plaintext ─────────────────────────

        /// <summary>
        /// Decrypt a Base64(IV + CipherText) string back to plaintext.
        /// </summary>
        public string Decrypt(string base64CipherText)
        {
            if (string.IsNullOrEmpty(base64CipherText))
                throw new ArgumentException("Text to decrypt cannot be empty.");

            byte[] fullBytes = Convert.FromBase64String(base64CipherText);

            // Extract the IV (first 16 bytes) and ciphertext (the rest)
            byte[] iv          = new byte[16];
            byte[] cipherBytes = new byte[fullBytes.Length - 16];
            Buffer.BlockCopy(fullBytes, 0,  iv,          0, 16);
            Buffer.BlockCopy(fullBytes, 16, cipherBytes, 0, cipherBytes.Length);

            using var aes = Aes.Create();
            aes.Key     = _key;
            aes.IV      = iv;
            aes.Mode    = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            byte[] plainBytes   = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }
    }


    // ════════════════════════════════════════════════════════════════════
    //  SHA-256 HASHING UTILITY
    // ════════════════════════════════════════════════════════════════════
    //
    //  SHA-256 produces a fixed 64-character hex string from any input.
    //  It is deterministic — same input always → same hash.
    //  Used for: data integrity checks, token fingerprinting, cache keys.
    //  NOT used for passwords — use PasswordHasher (BCrypt) for passwords.

    public static class Sha256Hasher
    {
        /// <summary>
        /// Compute SHA-256 hash of a string.
        /// Returns a 64-character lowercase hex string.
        /// </summary>
        public static string Hash(string input)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Input cannot be empty.");

            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes  = SHA256.HashData(inputBytes);
            return Convert.ToHexString(hashBytes).ToLowerInvariant();
        }

        /// <summary>
        /// Compute SHA-256 hash of a string with an extra salt appended.
        /// Useful for hashing tokens, API keys, etc.
        /// </summary>
        public static string HashWithSalt(string input, string salt)
            => Hash(input + salt);
    }
}