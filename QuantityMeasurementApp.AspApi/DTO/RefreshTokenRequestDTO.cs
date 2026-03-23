using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementApp.AspApi.DTO
{
    // ── UC17: Refresh Token Request ───────────────────────────────────────
    // Sent to POST /api/v1/users/refresh to exchange a refresh token
    // for a new JWT without signing in again.
    //
    // Request body:
    //   { "username":"anamika", "refreshToken":"A3B9C1D2..." }
    public class RefreshTokenRequestDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username     { get; set; } = string.Empty;

        [Required(ErrorMessage = "RefreshToken is required.")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}