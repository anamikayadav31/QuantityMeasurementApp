using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Represents a weight quantity with value and unit.
    /// Similar to QuantityLength but for weight.
    /// </summary>
    public class QuantityWeight
    {
        public double Value { get; }
        public WeightUnit Unit { get; }

        /// <summary>
        /// Constructor with validation
        /// </summary>
        public QuantityWeight(double value, WeightUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Convert this weight to base unit (kilogram)
        /// </summary>
        private double ToKilogram()
        {
            return Unit.ConvertToBaseUnit(Value);
        }

        /// <summary>
        /// Convert to another unit
        /// </summary>
        public QuantityWeight ConvertTo(WeightUnit targetUnit)
        {
            double baseValue = ToKilogram();
            double converted = targetUnit.ConvertFromBaseUnit(baseValue);

            return new QuantityWeight(converted, targetUnit);
        }

        /// <summary>
        /// Add two weights (result in first unit)
        /// </summary>
        public QuantityWeight Add(QuantityWeight other)
        {
            double sumKg = this.ToKilogram() + other.ToKilogram();
            double result = this.Unit.ConvertFromBaseUnit(sumKg);

            return new QuantityWeight(result, this.Unit);
        }

        /// <summary>
        /// Add two weights with target unit
        /// </summary>
        public QuantityWeight Add(QuantityWeight other, WeightUnit targetUnit)
        {
            double sumKg = this.ToKilogram() + other.ToKilogram();
            double result = targetUnit.ConvertFromBaseUnit(sumKg);

            return new QuantityWeight(result, targetUnit);
        }

        /// <summary>
        /// Equality check using kilogram
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(QuantityWeight))
                return false;

            QuantityWeight other = (QuantityWeight)obj;

            double val1 = this.ToKilogram();
            double val2 = other.ToKilogram();

            return Math.Abs(val1 - val2) < 0.0001;
        }

        /// <summary>
        /// Hashcode based on kilogram value
        /// </summary>
        public override int GetHashCode()
        {
            return ToKilogram().GetHashCode();
        }

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return $"{Value} {Unit}";
        }
    }
}