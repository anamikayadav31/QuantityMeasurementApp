using System;

namespace QuantityMeasurementApp.Models
{
    
    /// Represents a length quantity (example: 5 FEET, 12 INCHES).
    /// This class supports:
    /// - Unit conversion
    /// - Addition of two lengths
    /// - Equality comparison
   
    public class QuantityLength
    {
        // Numeric value (example: 5)
        public double Value { get; }

        // Unit of the value (example: FEET, INCHES)
        public LengthUnit Unit { get; }

        
        /// Constructor – creates a new QuantityLength object.
        /// Validates that the numeric value is not NaN or Infinity.
       
        public QuantityLength(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            Value = value;
            Unit = unit;
        }

        
        /// Converts the current quantity to FEET.
        /// FEET is treated as the base unit for all calculations.
       
        private double ToFeet()
        {
            // Convert this unit to feet using extension method
            return Value * Unit.ToFeetFactor();
        }

        
        /// Static method:
        /// Converts a numeric value from one unit to another.
        /// Example: Convert(1, FEET, INCHES) → 12
       
        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            // Step 1: Convert source value to FEET
            double valueInFeet = value * source.ToFeetFactor();

            // Step 2: Convert FEET to target unit
            return valueInFeet / target.ToFeetFactor();
        }

        
        /// Adds another QuantityLength to this one.
        /// The result will be returned in THIS object's unit.
       
        public QuantityLength Add(QuantityLength other)
        {
            if (other == null)
                throw new ArgumentException("Cannot add null quantity");

            // Convert both values to FEET (base unit)
            double sumInFeet = this.ToFeet() + other.ToFeet();

            // Convert back to this object's unit
            double sumInCurrentUnit = sumInFeet / this.Unit.ToFeetFactor();

            // Return a new QuantityLength object
            return new QuantityLength(sumInCurrentUnit, this.Unit);
        }

        
        /// Converts this object directly to another unit.
        /// Example: quantity.ConvertTo(INCHES)
       
        public double ConvertTo(LengthUnit targetUnit)
        {
            return Convert(this.Value, this.Unit, targetUnit);
        }

        
        /// Checks if two QuantityLength objects are equal.
        /// Equality is checked after converting both to FEET.
        /// A small tolerance is used for floating-point safety.
       
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;

            if (obj is not QuantityLength other) return false;

            // Compare values in FEET
            return Math.Abs(this.ToFeet() - other.ToFeet()) < 0.0001;
        }

        
        /// Required when overriding Equals().
        /// Uses FEET value to generate hash code.
       
        public override int GetHashCode()
        {
            return ToFeet().GetHashCode();
        }

        
        /// Returns a readable string like:
        /// "5 FEET"
       
        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}