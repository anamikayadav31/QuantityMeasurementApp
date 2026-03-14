using QuantityMeasurementApp.ModelLayer.Enums;

namespace QuantityMeasurementApp.ModelLayer.DTO
{
    /// <summary>
    /// Data Transfer Object (DTO) for quantity measurements.
    ///
    /// Purpose:
    ///   Carries quantity data between layers:
    ///     Controller → Service → (back) → Controller
    ///
    /// Design:
    ///   - Immutable: all properties set via constructor, no setters
    ///   - Unit stored as uppercase string for flexibility across layers
    ///   - MeasurementType tags which category the unit belongs to,
    ///     preventing cross-category operations at service level
    ///
    /// Example usage:
    ///   var dto = new QuantityDTO(1.0, "FEET", MeasurementType.LENGTH);
    /// </summary>
    public class QuantityDTO
    {
        /// <summary>Numeric value of the quantity. E.g. 1.0, 12.5, 100</summary>
        public double Value { get; }

        /// <summary>Unit as uppercase string. E.g. "FEET", "GRAM", "CELSIUS"</summary>
        public string Unit { get; }

        /// <summary>Measurement category. E.g. LENGTH, WEIGHT, VOLUME, TEMPERATURE</summary>
        public MeasurementType MeasurementType { get; }

        /// <summary>
        /// Creates an immutable QuantityDTO.
        /// </summary>
        /// <param name="value">Numeric value — must not be NaN or Infinity</param>
        /// <param name="unit">Unit string — must not be null or empty</param>
        /// <param name="measurementType">Measurement category</param>
        public QuantityDTO(double value, string unit, MeasurementType measurementType)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException(
                    $"Invalid numeric value '{value}'. Must be a finite number.");

            if (string.IsNullOrWhiteSpace(unit))
                throw new ArgumentException("Unit cannot be null or empty.");

            Value           = value;
            Unit            = unit.ToUpperInvariant();
            MeasurementType = measurementType;
        }

        /// <summary>Human-readable representation. E.g. "1 FEET [LENGTH]"</summary>
        public override string ToString()
            => $"{Math.Round(Value, 3)} {Unit} [{MeasurementType}]";
    }
}