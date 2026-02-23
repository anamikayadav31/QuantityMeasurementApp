using System;

namespace QuantityMeasurementApp.Models
{
/// <summary>
/// Represents a length quantity (e.g., 5 FEET, 12 INCHES) and provides operations for it.
/// 
/// Features:
/// 1. Stores a numeric value and its <see cref="LengthUnit"/>.
/// 2. Converts between units (FEET, INCHES, YARDS, CENTIMETERS).
/// 3. Adds two <see cref="QuantityLength"/> objects:
///    - Returns result in this object's unit by default.
///    - Can return result in a specified target unit.
/// 4. Compares quantities for equality, considering unit conversions.
/// 5. Provides string representation for display purposes.
/// 6. Validates numeric input to prevent NaN or Infinity values.
/// </summary>
    public class QuantityLength
    {
        public double Value { get; }      // Numeric value
        public LengthUnit Unit { get; }   // Unit of the value

        public QuantityLength(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            Value = value;
            Unit = unit;
        }

        // Convert this quantity to FEET (base unit)
        private double ToFeet() => Value * Unit.ToFeetFactor();

        // Convert any numeric value from source unit to target unit
        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            double valueInFeet = value * source.ToFeetFactor();
            return valueInFeet / target.ToFeetFactor();
        }

        // Add another QuantityLength; result is in this object's unit
        public QuantityLength Add(QuantityLength other)
        {
            if (other == null)
                throw new ArgumentException("Cannot add null quantity");

            double sumInFeet = this.ToFeet() + other.ToFeet();
            double sumInCurrentUnit = sumInFeet / this.Unit.ToFeetFactor();

            return new QuantityLength(sumInCurrentUnit, this.Unit);
        }

        // NEW: Add another QuantityLength, specifying target unit
        public QuantityLength Add(QuantityLength other, LengthUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Cannot add null quantity");

            // Validate target unit
            if (!Enum.IsDefined(typeof(LengthUnit), targetUnit))
                throw new ArgumentException("Invalid target unit");

            // Step 1: Convert both quantities to base unit (FEET)
            double sumInFeet = this.ToFeet() + other.ToFeet();

            // Step 2: Convert sum to the specified target unit
            double sumInTargetUnit = sumInFeet / targetUnit.ToFeetFactor();

            // Return new object in target unit
            return new QuantityLength(sumInTargetUnit, targetUnit);
        }

        // Convert this object directly to another unit
        public double ConvertTo(LengthUnit targetUnit) => Convert(this.Value, this.Unit, targetUnit);

        // Equality comparison using FEET as base
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is not QuantityLength other) return false;
            return Math.Abs(this.ToFeet() - other.ToFeet()) < 0.0001;
        }

        public override int GetHashCode() => ToFeet().GetHashCode();

        public override string ToString() => $"{Value} {Unit}";
    }
}