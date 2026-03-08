using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// QuantityLength represents a length value with its unit.
    /// 
    /// Responsibilities:
    /// - Store value and unit
    /// - Perform arithmetic operations
    /// - Compare quantities
    /// 
    /// Conversion logic is handled by LengthUnit.
    /// </summary>
    public class QuantityLength : IComparable<QuantityLength>
    {
        /// <summary>
        /// Numeric value of the quantity
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Unit of the quantity
        /// </summary>
        public LengthUnit Unit { get; }

        /// <summary>
        /// Constructor to create a quantity
        /// </summary>
        public QuantityLength(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Convert this quantity to base unit (Feet)
        /// </summary>
        private double ToFeet()
        {
            return Unit.ConvertToBaseUnit(Value);
        }

        /// <summary>
        /// Static helper to convert between units
        /// </summary>
        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            return source.ConvertTo(value, target);
        }

        /// <summary>
        /// Convert this quantity to another unit
        /// </summary>
        public double ConvertTo(LengthUnit targetUnit)
        {
            return Unit.ConvertTo(Value, targetUnit);
        }

        /// <summary>
        /// Add two quantities (result in same unit as first quantity)
        /// </summary>
        public QuantityLength Add(QuantityLength other)
        {
            if (other == null)
                throw new ArgumentException("Cannot add null");

            double sumFeet = this.ToFeet() + other.ToFeet();
            double result = this.Unit.ConvertFromBaseUnit(sumFeet);

            return new QuantityLength(result, this.Unit);
        }

        /// <summary>
        /// Add two quantities with target unit
        /// </summary>
        public QuantityLength Add(QuantityLength other, LengthUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Cannot add null");

            double sumFeet = this.ToFeet() + other.ToFeet();
            double result = targetUnit.ConvertFromBaseUnit(sumFeet);

            return new QuantityLength(result, targetUnit);
        }

        /// <summary>
        /// Subtract two quantities
        /// </summary>
        public QuantityLength Subtract(QuantityLength other)
        {
            if (other == null)
                throw new ArgumentException("Cannot subtract null");

            double diffFeet = this.ToFeet() - other.ToFeet();
            double result = this.Unit.ConvertFromBaseUnit(diffFeet);

            return new QuantityLength(result, this.Unit);
        }

        /// <summary>
        /// Multiply quantity with number
        /// </summary>
        public QuantityLength Multiply(double factor)
        {
            return new QuantityLength(Value * factor, Unit);
        }

        /// <summary>
        /// Compare two quantities for sorting
        /// </summary>
        public int CompareTo(QuantityLength? other)
        {
            if (other == null) return 1;

            return this.ToFeet().CompareTo(other.ToFeet());
        }

        /// <summary>
        /// Equality check using base unit
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is not QuantityLength other)
                return false;

            return Math.Abs(this.ToFeet() - other.ToFeet()) < 0.0001;
        }

        /// <summary>
        /// HashCode based on base unit value
        /// </summary>
        public override int GetHashCode()
        {
            return ToFeet().GetHashCode();
        }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return $"{Value} {Unit}";
        }

        /// <summary>
        /// Operator +
        /// </summary>
        public static QuantityLength operator +(QuantityLength a, QuantityLength b)
            => a.Add(b);

        /// <summary>
        /// Operator -
        /// </summary>
        public static QuantityLength operator -(QuantityLength a, QuantityLength b)
            => a.Subtract(b);

        /// <summary>
        /// Operator ==
        /// </summary>
        public static bool operator ==(QuantityLength a, QuantityLength b)
            => a.Equals(b);

        /// <summary>
        /// Operator !=
        /// </summary>
        public static bool operator !=(QuantityLength a, QuantityLength b)
            => !a.Equals(b);
    }
}