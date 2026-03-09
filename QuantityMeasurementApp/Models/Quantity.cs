using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Generic class used to represent any measurable quantity.
    /// Example: Length, Weight, Volume, Temperature.
    /// Each quantity has a numeric value and a unit.
    /// Example: 5 Feet, 2 Kilograms, 3 Litres.
    /// </summary>
    public class Quantity<T>
    {
        // ---------------- PROPERTIES ----------------

        /// <summary>
        /// Stores the numeric value of the quantity
        /// Example: 5 in "5 Feet"
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Stores the unit type of the quantity
        /// Example: FEET, KILOGRAM, LITRE
        /// </summary>
        public T Unit { get; }

        // ---------------- CONSTRUCTOR ----------------

        /// <summary>
        /// Constructor used to create a Quantity object
        /// It receives the value and the unit
        /// </summary>
        public Quantity(double value, T unit)
        {
            // Unit should not be null
            if (unit == null)
                throw new ArgumentException("Unit cannot be null");

            // Value must be a valid number
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            Value = value;
            Unit = unit;
        }

        // ---------------- ENUM FOR ARITHMETIC ----------------

        /// <summary>
        /// Enum used internally to represent arithmetic operations
        /// </summary>
        private enum ArithmeticOperation
        {
            ADD,
            SUBTRACT,
            DIVIDE
        }

        // ---------------- VALIDATION ----------------

        /// <summary>
        /// Checks if the other quantity is valid before performing operations
        /// </summary>
        private void ValidateOperands(Quantity<T> other)
        {
            if (other == null)
                throw new ArgumentException("Other quantity cannot be null");

            if (double.IsNaN(other.Value) || double.IsInfinity(other.Value))
                throw new ArgumentException("Invalid numeric value in other quantity");
        }

        // ---------------- CENTRALIZED ARITHMETIC ----------------

        /// <summary>
        /// This method performs arithmetic operations.
        /// First both quantities are converted to a base unit.
        /// Then the required arithmetic operation is performed.
        /// </summary>
        private double PerformBaseArithmetic(
            Quantity<T> other,
            Func<T, double, double> convertToBase,
            ArithmeticOperation operation)
        {
            // Validate input quantity
            ValidateOperands(other);

            // Convert both quantities to base unit
            double base1 = convertToBase(Unit, Value);
            double base2 = convertToBase(other.Unit, other.Value);

            // Perform arithmetic based on operation type
            switch (operation)
            {
                case ArithmeticOperation.ADD:
                    return base1 + base2;

                case ArithmeticOperation.SUBTRACT:
                    return base1 - base2;

                case ArithmeticOperation.DIVIDE:

                    // Prevent division by zero
                    if (base2 == 0)
                        throw new ArithmeticException("Division by zero is not allowed");

                    return base1 / base2;

                default:
                    throw new InvalidOperationException("Invalid arithmetic operation");
            }
        }

        // ---------------- ADD ----------------

        /// <summary>
        /// Adds two quantities.
        /// Both quantities are converted to base unit before adding.
        /// The result is converted back to the unit of the current object.
        /// </summary>
        public Quantity<T> Add(
            Quantity<T> other,
            Func<T, double, double> convertToBase,
            Func<T, double, double> convertFromBase)
        {
            double baseResult = PerformBaseArithmetic(other, convertToBase, ArithmeticOperation.ADD);

            // Convert result from base unit back to original unit
            double result = convertFromBase(Unit, baseResult);

            return new Quantity<T>(result, Unit);
        }

        // ---------------- SUBTRACT ----------------

        /// <summary>
        /// Subtracts one quantity from another
        /// </summary>
        public Quantity<T> Subtract(
            Quantity<T> other,
            Func<T, double, double> convertToBase,
            Func<T, double, double> convertFromBase)
        {
            double baseResult = PerformBaseArithmetic(other, convertToBase, ArithmeticOperation.SUBTRACT);

            double result = convertFromBase(Unit, baseResult);

            return new Quantity<T>(result, Unit);
        }

        // ---------------- DIVIDE ----------------

        /// <summary>
        /// Divides two quantities
        /// Returns only the numeric ratio
        /// </summary>
        public double Divide(
            Quantity<T> other,
            Func<T, double, double> convertToBase)
        {
            return PerformBaseArithmetic(other, convertToBase, ArithmeticOperation.DIVIDE);
        }

        // ---------------- CONVERT TO ANOTHER UNIT ----------------

        /// <summary>
        /// Converts the quantity into another unit
        /// Example: Feet → Inches
        /// </summary>
        public Quantity<T> ConvertTo(
            Func<T, double, double> convertFromBase,
            Func<T, double, double> convertToBase,
            T targetUnit)
        {
            // Convert current value to base unit
            double baseValue = convertToBase(Unit, Value);

            // Convert base unit to target unit
            double result = convertFromBase(targetUnit, baseValue);

            return new Quantity<T>(result, targetUnit);
        }

        // ---------------- EQUALITY CHECK ----------------

        /// <summary>
        /// Checks if two quantities are equal
        /// even if their units are different.
        /// Example: 12 Inches == 1 Foot
        /// </summary>
        public bool Equals(
            Quantity<T> other,
            Func<T, double, double> convertToBase)
        {
            if (other == null) return false;

            double base1 = convertToBase(Unit, Value);
            double base2 = convertToBase(other.Unit, other.Value);

            // Small tolerance to handle floating point precision
            return Math.Abs(base1 - base2) < 0.0001;
        }

        // ---------------- TEMPERATURE SAFETY ----------------

        /// <summary>
        /// Prevent arithmetic operations for Temperature
        /// </summary>
        public void ValidateArithmeticSupported(string operation)
        {
            if (typeof(T) == typeof(TemperatureUnit))
                throw new NotSupportedException($"Temperature does not support arithmetic like {operation}");
        }

        // ---------------- TO STRING ----------------

        /// <summary>
        /// Used to display quantity in readable format
        /// Example: 5.000 FEET
        /// </summary>
        public override string ToString()
        {
            return $"{Math.Round(Value, 3)} {Unit}";
        }
    }
}