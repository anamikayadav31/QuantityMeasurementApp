using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurementApp.ModelLayer.Entities
{
    // ── EF Core Entity — maps to Users table in SQL Server ───────────────
    //
    // Spring equivalent:
    //   @Entity + @Table("users")
    //   @Column(unique = true)  →  [Index(IsUnique = true)] on DbContext

    [Table("Users")]
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;    // unique — enforced in DbContext

        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;       // stored AES-encrypted

        [Required]
        [MaxLength(500)]
        public string PasswordHash { get; set; } = string.Empty; // BCrypt + salt

        [Required]
        [MaxLength(20)]
        public string Role { get; set; } = "User";              // "User" or "Admin"

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}