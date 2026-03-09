using System;

namespace QuantityMeasurementApp.Models
{
    // Enum representing supported temperature units.
    // Celsius is used as the base unit for conversions.
    public enum TemperatureUnit
    {
        CELSIUS,    // Base unit
        FAHRENHEIT, // Will convert through Celsius
        KELVIN      // Will convert through Celsius
    }

    // Extension methods for TemperatureUnit
    // Handles conversions to/from base unit and validates operations
    public static class TemperatureUnitExtensions
    {
        /// <summary>
        /// Converts a value from this unit to the base unit (Celsius).
        /// </summary>
        /// <param name="unit">Temperature unit of the value.</param>
        /// <param name="value">Numeric value in the given unit.</param>
        /// <returns>Value in Celsius.</returns>
        public static double ConvertToBaseUnit(this TemperatureUnit unit, double value)
        {
            return unit switch
            {
                TemperatureUnit.CELSIUS => value,                   // Already in base
                TemperatureUnit.FAHRENHEIT => (value - 32) * 5 / 9, // Fahrenheit → Celsius
                TemperatureUnit.KELVIN => value - 273.15,           // Kelvin → Celsius
                _ => throw new ArgumentException("Invalid temperature unit")
            };
        }

        /// <summary>
        /// Converts a value from the base unit (Celsius) to this unit.
        /// </summary>
        /// <param name="unit">Target temperature unit.</param>
        /// <param name="value">Value in Celsius.</param>
        /// <returns>Value in target unit.</returns>
        public static double ConvertFromBaseUnit(this TemperatureUnit unit, double value)
        {
            return unit switch
            {
                TemperatureUnit.CELSIUS => value,                   // Already in target unit
                TemperatureUnit.FAHRENHEIT => (value * 9 / 5) + 32, // Celsius → Fahrenheit
                TemperatureUnit.KELVIN => value + 273.15,           // Celsius → Kelvin
                _ => throw new ArgumentException("Invalid temperature unit")
            };
        }

        /// <summary>
        /// Checks if arithmetic operations are allowed on temperatures.
        /// Throws NotSupportedException for add, subtract, divide.
        /// </summary>
        /// <param name="unit">The temperature unit.</param>
        /// <param name="operation">Operation being attempted.</param>
        public static void ValidateOperationSupport(this TemperatureUnit unit, string operation)
        {
            // Temperature arithmetic is not meaningful, only comparison/conversion is allowed
            throw new NotSupportedException(
                $"Cannot perform '{operation}' on TemperatureUnit. " +
                "Use comparison or conversion methods instead."
            );
        }
    }
}