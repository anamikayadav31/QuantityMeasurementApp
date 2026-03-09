using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Generic class that represents a quantity.
    /// A quantity has a numeric value and a unit.
    /// Example: 5 Meter, 2 Liter, etc.
    /// </summary>
    public class Quantity<T>
    {
        // Stores the numeric value (example: 5)
        public double Value { get; }

        // Stores the unit of measurement (example: Meter, Liter)
        public T Unit { get; }

        /// <summary>
        /// Constructor used to create a quantity object
        /// </summary>
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
        /// Example: Meter → Feet
        /// </summary>
        public Quantity<T> ConvertTo(
            Func<T, double, double> convertFromBase,
            Func<T, double, double> convertToBase,
            T targetUnit)
        {
            // Check target unit
            if (targetUnit == null)
                throw new ArgumentException("Target unit cannot be null");

            // Convert current value to base unit
            double baseValue = convertToBase(Unit, Value);

            // Convert base unit to target unit
            double result = convertFromBase(targetUnit, baseValue);

            // Return new quantity in target unit
            return new Quantity<T>(result, targetUnit);
        }

        /// <summary>
        /// Adds two quantities
        /// Example: 1 meter + 50 cm
        /// Result will be returned in the current unit
        /// </summary>
        public Quantity<T> Add(
            Quantity<T> other,
            Func<T, double, double> convertToBase,
            Func<T, double, double> convertFromBase)
        {
            // Check if other quantity exists
            if (other == null)
                throw new ArgumentException("Quantity cannot be null");

            // Convert both quantities to base unit
            double base1 = convertToBase(Unit, Value);
            double base2 = convertToBase(other.Unit, other.Value);

            // Add them
            double sum = base1 + base2;

            // Convert result back to current unit
            double result = convertFromBase(Unit, sum);

            return new Quantity<T>(result, Unit);
        }

        /// <summary>
        /// Subtract another quantity
        /// Result is returned in the current unit
        /// </summary>
        public Quantity<T> Subtract(
            Quantity<T> other,
            Func<T, double, double> convertToBase,
            Func<T, double, double> convertFromBase)
        {
            // Check if other quantity exists
            if (other == null)
                throw new ArgumentException("Quantity cannot be null");

            // Convert both values to base unit
            double base1 = convertToBase(Unit, Value);
            double base2 = convertToBase(other.Unit, other.Value);

            // Subtract values
            double resultBase = base1 - base2;

            // Convert result back to current unit
            double result = convertFromBase(Unit, resultBase);

            // Round result to 2 decimal places
            return new Quantity<T>(Math.Round(result, 2), Unit);
        }

        /// <summary>
        /// Subtract another quantity and return result in a target unit
        /// Example: 2 meter - 50 cm → result in cm
        /// </summary>
        public Quantity<T> Subtract(
            Quantity<T> other,
            Func<T, double, double> convertToBase,
            Func<T, double, double> convertFromBase,
            T targetUnit)
        {
            // Validate inputs
            if (other == null)
                throw new ArgumentException("Quantity cannot be null");

            if (targetUnit == null)
                throw new ArgumentException("Target unit cannot be null");

            // Convert both to base unit
            double base1 = convertToBase(Unit, Value);
            double base2 = convertToBase(other.Unit, other.Value);

            // Subtract
            double resultBase = base1 - base2;

            // Convert result to target unit
            double result = convertFromBase(targetUnit, resultBase);

            // Return rounded value
            return new Quantity<T>(Math.Round(result, 2), targetUnit);
        }

        /// <summary>
        /// Divides this quantity by another quantity
        /// Returns a ratio (no unit)
        /// Example: 4 meter / 2 meter = 2
        /// </summary>
        public double Divide(
            Quantity<T> other,
            Func<T, double, double> convertToBase)
        {
            // Check if other quantity exists
            if (other == null)
                throw new ArgumentException("Quantity cannot be null");

            // Convert both values to base unit
            double base1 = convertToBase(Unit, Value);
            double base2 = convertToBase(other.Unit, other.Value);

            // Prevent division by zero
            if (base2 == 0)
                throw new ArithmeticException("Cannot divide by zero");

            // Return division result
            return base1 / base2;
        }

        /// <summary>
        /// Converts quantity into a readable string
        /// Example: "5 Meter"
        /// </summary>
        public override string ToString()
        {
            return $"{Math.Round(Value, 3)} {Unit}";
        }
    }
}