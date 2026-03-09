using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Generic class that represents a quantity.
    /// A quantity has a numeric value and a unit.
    /// Example: 5 Feet, 2 Kilogram, 3 Liter
    /// </summary>
    public class Quantity<T>
    {
        // Stores the numeric value
        public double Value { get; }

        // Stores the unit type (Feet, Gram, Liter etc.)
        public T Unit { get; }

        /// <summary>
        /// Constructor used to create a Quantity object
        /// </summary>
        public Quantity(double value, T unit)
        {
            // Check if unit is null
            if (unit == null)
                throw new ArgumentException("Unit cannot be null");

            // Check if value is invalid
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid value");

            // Assign values
            Value = value;
            Unit = unit;
        }

        // ---------------- ENUM FOR OPERATIONS ----------------

        /// <summary>
        /// Enum used to represent arithmetic operations
        /// </summary>
        private enum ArithmeticOperation
        {
            ADD,        // Addition
            SUBTRACT,   // Subtraction
            DIVIDE      // Division
        }

        // ---------------- VALIDATION ----------------

        /// <summary>
        /// Validates the other quantity before doing operations
        /// </summary>
        private void ValidateOperands(Quantity<T> other)
        {
            // Check if other quantity is null
            if (other == null)
                throw new ArgumentException("Other quantity cannot be null");

            // Check if value is invalid
            if (double.IsNaN(other.Value) || double.IsInfinity(other.Value))
                throw new ArgumentException("Invalid value");
        }

        // ---------------- CENTRALIZED ARITHMETIC ----------------

        /// <summary>
        /// Performs arithmetic operations after converting values to base unit
        /// </summary>
        private double PerformBaseArithmetic(
            Quantity<T> other,
            Func<T, double, double> convertToBase,
            ArithmeticOperation operation)
        {
            // Validate the input quantity
            ValidateOperands(other);

            // Convert both quantities to base unit
            double base1 = convertToBase(Unit, Value);
            double base2 = convertToBase(other.Unit, other.Value);

            // Perform operation depending on enum type
            switch (operation)
            {
                case ArithmeticOperation.ADD:
                    return base1 + base2;

                case ArithmeticOperation.SUBTRACT:
                    return base1 - base2;

                case ArithmeticOperation.DIVIDE:

                    // Prevent division by zero
                    if (base2 == 0)
                        throw new ArithmeticException("Division by zero");

                    return base1 / base2;

                default:
                    throw new InvalidOperationException();
            }
        }

        // ---------------- ADD ----------------

        /// <summary>
        /// Adds two quantities
        /// Result is returned in the current unit
        /// </summary>
        public Quantity<T> Add(
            Quantity<T> other,
            Func<T, double, double> convertToBase,
            Func<T, double, double> convertFromBase)
        {
            // Perform addition in base unit
            double baseResult = PerformBaseArithmetic(
                other,
                convertToBase,
                ArithmeticOperation.ADD);

            // Convert result back to current unit
            double result = convertFromBase(Unit, baseResult);

            return new Quantity<T>(result, Unit);
        }

        // ---------------- SUBTRACT ----------------

        /// <summary>
        /// Subtracts another quantity from current quantity
        /// </summary>
        public Quantity<T> Subtract(
            Quantity<T> other,
            Func<T, double, double> convertToBase,
            Func<T, double, double> convertFromBase)
        {
            // Perform subtraction in base unit
            double baseResult = PerformBaseArithmetic(
                other,
                convertToBase,
                ArithmeticOperation.SUBTRACT);

            // Convert result back to current unit
            double result = convertFromBase(Unit, baseResult);

            return new Quantity<T>(result, Unit);
        }

        // ---------------- DIVIDE ----------------

        /// <summary>
        /// Divides one quantity by another quantity
        /// Returns a ratio (no unit)
        /// Example: 4 meter / 2 meter = 2
        /// </summary>
        public double Divide(
            Quantity<T> other,
            Func<T, double, double> convertToBase)
        {
            // Perform division in base unit
            return PerformBaseArithmetic(
                other,
                convertToBase,
                ArithmeticOperation.DIVIDE);
        }

        // ---------------- CONVERSION ----------------

        /// <summary>
        /// Converts quantity from one unit to another unit
        /// Example: Feet → Inches
        /// </summary>
        public Quantity<T> ConvertTo(
            Func<T, double, double> convertFromBase,
            Func<T, double, double> convertToBase,
            T targetUnit)
        {
            // Convert current value to base unit
            double baseValue = convertToBase(Unit, Value);

            // Convert base unit value to target unit
            double result = convertFromBase(targetUnit, baseValue);

            return new Quantity<T>(result, targetUnit);
        }

        // ---------------- STRING OUTPUT ----------------

        /// <summary>
        /// Returns quantity as readable text
        /// Example: "5.25 Feet"
        /// </summary>
        public override string ToString()
        {
            return $"{Math.Round(Value, 3)} {Unit}";
        }
    }
}