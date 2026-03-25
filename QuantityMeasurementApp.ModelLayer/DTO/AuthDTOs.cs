using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementApp.AspApi.DTO
{
    // ════════════════════════════════════════════════════════════════════
    //  UC17: Authentication DTOs
    //  These classes define the JSON the client sends for signup/signin
    //  and what the API sends back.
    // ════════════════════════════════════════════════════════════════════

    // ── SIGN UP request ───────────────────────────────────────────────────
    // POST /api/v1/auth/signup
    // {
    //   "username": "anamika",
    //   "email": "anamika@email.com",
    //   "password": "Secret123!"
    // }
    public class SignUpRequestDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;
    }

    // ── SIGN IN request ───────────────────────────────────────────────────
    // POST /api/v1/auth/signin
    // {
    //   "username": "anamika",
    //   "password": "Secret123!"
    // }
    public class SignInRequestDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;
    }

    // ── AUTH response (returned for both signup and signin) ───────────────
    // {
    //   "token": "eyJhbGciOi...",
    //   "username": "anamika",
    //   "email": "anamika@email.com",
    //   "role": "User",
    //   "expiresAt": "2026-03-23T10:00:00Z",
    //   "message": "Sign in successful."
    // }
    public class AuthResponseDTO
    {
        public string   Token     { get; set; } = string.Empty;
        public string   Username  { get; set; } = string.Empty;
        public string   Email     { get; set; } = string.Empty;
        public string   Role      { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string   Message   { get; set; } = string.Empty;
    }
}