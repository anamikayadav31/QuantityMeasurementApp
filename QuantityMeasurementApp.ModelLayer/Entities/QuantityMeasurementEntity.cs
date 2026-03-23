using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurementApp.ModelLayer.Entities
{
    // ── EF Core Entity — maps to QuantityMeasurements table in SQL Server ─
    //
    // EF Core reads these annotations and creates the table automatically
    // when you run:  dotnet ef migrations add Init
    //                dotnet ef database update
    //
    // Spring equivalent:
    //   @Entity           →  [Table]
    //   @Id               →  [Key]
    //   @GeneratedValue   →  [DatabaseGenerated]
    //   @Column           →  [Column]

    [Table("QuantityMeasurements")]   // SQL Server table name
    public class QuantityMeasurementEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }                           // auto-increment int PK

        [Required]
        [MaxLength(50)]
        public string Operation { get; set; } = string.Empty; // e.g. COMPARE, ADD

        [Required]
        [MaxLength(200)]
        public string Operand1 { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Operand2 { get; set; }

        [MaxLength(500)]
        public string? Result { get; set; }

        public bool HasError { get; set; } = false;

        [MaxLength(1000)]
        public string? ErrorMessage { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // ── Constructors ──────────────────────────────────────────────────

        public QuantityMeasurementEntity() { }

        // Success constructor
        public QuantityMeasurementEntity(
            string operation, string operand1, string? operand2, string result)
        {
            Operation = operation;
            Operand1  = operand1;
            Operand2  = operand2;
            Result    = result;
            HasError  = false;
            Timestamp = DateTime.UtcNow;
        }

        // Error constructor
        public QuantityMeasurementEntity(
            string operation, string operand1, string? operand2,
            string errorMessage, bool hasError)
        {
            Operation    = operation;
            Operand1     = operand1;
            Operand2     = operand2;
            ErrorMessage = errorMessage;
            HasError     = hasError;
            Timestamp    = DateTime.UtcNow;
        }

        public override string ToString()
        {
            string ops = Operand2 != null ? $"{Operand1} & {Operand2}" : Operand1;
            return HasError
                ? $"[{Timestamp:HH:mm:ss}]  {Operation,-20}  {ops}  →  ERROR: {ErrorMessage}"
                : $"[{Timestamp:HH:mm:ss}]  {Operation,-20}  {ops}  →  {Result}";
        }
    }
}