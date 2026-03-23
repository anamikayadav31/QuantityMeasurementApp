using QuantityMeasurementApp.ModelLayer.Enums;

namespace QuantityMeasurementApp.ModelLayer.DTO
{
    // ── UC15: N-Tier Architecture ────────────────────────────────────────
    // DTO (Data Transfer Object) that carries a quantity between layers.
    //
    // Key design decisions:
    //   - Immutable: all properties are set once in the constructor.
    //   - Unit stored as uppercase string so it can cross layer boundaries
    //     without forcing every layer to know about enums.
    //   - MeasurementType tag prevents cross-category operations.
    //
    // Example:
    //   var dto = new QuantityDTO(1.0, "FEET", MeasurementType.LENGTH);
    public class QuantityDTO
    {
        // ── Properties ───────────────────────────────────────────────────

        /// <summary>The numeric amount, e.g. 1.0, 12.5</summary>
        public double Value { get; }

        /// <summary>Unit name in UPPER_CASE, e.g. "FEET", "GRAM", "CELSIUS"</summary>
        public string Unit { get; }

        /// <summary>Which measurement category: LENGTH / WEIGHT / VOLUME / TEMPERATURE</summary>
        public MeasurementType MeasurementType { get; }

        // ── Constructor ──────────────────────────────────────────────────

        public QuantityDTO(double value, string unit, MeasurementType measurementType)
        {
            // Guard: value must be a normal finite number
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException(
                    $"Value '{value}' is not valid. Please enter a finite number.");

            // Guard: unit string must not be blank
            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentException("Unit cannot be empty.");

            Value           = value;
            Unit            = unit.ToUpperInvariant();   // always uppercase
            MeasurementType = measurementType;
        }

        // ── Display ──────────────────────────────────────────────────────

        // Example output:  "1 FEET [LENGTH]"
        public override string ToString()
            => $"{Math.Round(Value, 4)} {Unit} [{MeasurementType}]";
    }
}