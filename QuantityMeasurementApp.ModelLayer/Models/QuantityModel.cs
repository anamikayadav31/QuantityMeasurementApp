using QuantityMeasurementApp.ModelLayer.Enums;

namespace QuantityMeasurementApp.ModelLayer.Models
{
    /// <summary>
    /// Internal model representing a quantity with a strongly-typed unit.
    ///
    /// Purpose:
    ///   Used within the Business Layer for type-safe operations.
    ///   Generic type T must be one of:
    ///     LengthUnit | WeightUnit | VolumeUnit | TemperatureUnit
    ///
    /// Difference from QuantityDTO:
    ///   QuantityDTO  — uses string unit, crosses layer boundaries
    ///   QuantityModel — uses typed enum unit, stays inside service layer
    ///
    /// Design: Immutable — all fields set via constructor, no setters.
    /// </summary>
    public class QuantityModel<T> where T : Enum
    {
        /// <summary>Numeric value of the quantity.</summary>
        public double Value { get; }

        /// <summary>Strongly-typed unit. E.g. LengthUnit.FEET</summary>
        public T Unit { get; }

        /// <summary>Measurement category of this quantity.</summary>
        public MeasurementType MeasurementType { get; }

        /// <summary>
        /// Creates an immutable QuantityModel.
        /// </summary>
        public QuantityModel(double value, T unit, MeasurementType measurementType)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException(
                    $"Invalid numeric value '{value}'. Must be a finite number.");

            if (unit == null)
                throw new ArgumentNullException(nameof(unit), "Unit cannot be null.");

            Value           = value;
            Unit            = unit;
            MeasurementType = measurementType;
        }

        /// <summary>Human-readable representation. E.g. "1 FEET"</summary>
        public override string ToString()
            => $"{Math.Round(Value, 3)} {Unit}";
    }
}