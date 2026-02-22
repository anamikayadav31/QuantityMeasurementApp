using System;

namespace QuantityMeasurementApp.Models
{
    // Represents a length value with a unit (Feet or Inch)


    public class QuantityLength
    {
        public double Value { get; }
        public LengthUnit Unit { get; }

        // Constructor
        public QuantityLength(double value, LengthUnit unit)
        {
            // Reject invalid number (NaN)
            if (double.IsNaN(value))
                throw new ArgumentException("Invalid numeric value");

            Value = value;
            Unit = unit;
        }

        // Convert value to Feet (base unit)
        private double ToFeet()
        {
            return Unit switch
            {
                LengthUnit.FEET => Value,
                LengthUnit.INCH => Value / 12.0,          // 12 inch = 1 foot
                LengthUnit.YARDS => Value * 3.0,          // 1 yard = 3 feet
                LengthUnit.CENTIMETERS => Value / 30.48,  // 1 foot = 30.48 cm
                _ => throw new ArgumentException("Unsupported unit")
            };
        }

        // Override Equals
        public override bool Equals(object obj)
        {
            // If same reference → true
            if (ReferenceEquals(this, obj))
                return true;

            // If null or different type → false
            if (obj is not QuantityLength other)
                return false;

            // Compare converted values
            return Math.Abs(this.ToFeet() - other.ToFeet()) < 0.0001;
        }

        // Required when overriding Equals
        public override int GetHashCode()
        {
            return ToFeet().GetHashCode();
        }
    }
}