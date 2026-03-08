using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Enum for different length units
    /// </summary>
    public enum LengthUnit : int
    {
        FEET,
        INCHES,
        YARDS,
        CENTIMETERS
    }

    /// <summary>
    /// Helper methods for LengthUnit conversions
    /// </summary>
    public static class LengthUnitExtensions
    {
        // Returns conversion factor for each unit
        public static double GetConversionFactor(this LengthUnit unit)
        {
            switch (unit)
            {
                case LengthUnit.FEET:
                    return 1.0;

                case LengthUnit.INCHES:
                    return 1.0 / 12.0;

                case LengthUnit.YARDS:
                    return 3.0;

                case LengthUnit.CENTIMETERS:
                    return 1.0 / 30.48;

                default:
                    throw new ArgumentException("Invalid unit");
            }
        }

        // Converts given value to base unit (feet)
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        // Converts base unit value back to selected unit
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        // Returns unit name as text
        public static string GetUnitName(this LengthUnit unit)
        {
            return unit.ToString();
        }
    }
}