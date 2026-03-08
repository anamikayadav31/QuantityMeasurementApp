using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Generic class to represent a quantity with value and unit
    /// </summary>
    public class Quantity<T>
    {
        // Stores the value of the quantity
        public double Value { get; }

        // Stores the unit type (LengthUnit, WeightUnit, etc.)
        public T Unit { get; }

        // Constructor to initialize value and unit
        public Quantity(double value, T unit)
        {
            // Check if unit is null
            if (unit == null)
                throw new ArgumentException("Unit cannot be null");

            // Check if value is invalid
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid value");

            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Converts the quantity to another unit
        /// </summary>
        public Quantity<T> ConvertTo(Func<T, double, double> convertFromBase,
                                     Func<T, double, double> convertToBase,
                                     T targetUnit)
        {
            // Convert current value to base unit
            double baseValue = convertToBase(Unit, Value);

            // Convert base unit to target unit
            double result = convertFromBase(targetUnit, baseValue);

            // Return new quantity with converted value
            return new Quantity<T>(Math.Round(result, 2), targetUnit);
        }

        /// <summary>
        /// Adds two quantities
        /// </summary>
        public Quantity<T> Add(
            Quantity<T> other,
            Func<T, double, double> convertToBase,
            Func<T, double, double> convertFromBase)
        {
            // Convert both values to base unit
            double base1 = convertToBase(Unit, Value);
            double base2 = convertToBase(other.Unit, other.Value);

            // Add the base values
            double sum = base1 + base2;

            // Convert result back to original unit
            double result = convertFromBase(Unit, sum);

            return new Quantity<T>(result, Unit);
        }

        // Returns quantity as text (Value + Unit)
        public override string ToString()
        {
            return $"{Math.Round(Value, 3)} {Unit}";
        }
    }
}