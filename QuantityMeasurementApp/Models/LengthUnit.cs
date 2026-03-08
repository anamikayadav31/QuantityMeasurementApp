using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// LengthUnit represents all supported length units.
    /// 
    /// This enum is responsible for:
    /// 1. Storing conversion factors
    /// 2. Converting values TO base unit (Feet)
    /// 3. Converting values FROM base unit (Feet)
    /// 
    /// Base Unit = FEET
    /// </summary>
    public enum LengthUnit
    {
        FEET,
        INCHES,
        YARDS,
        CENTIMETERS
    }

    /// <summary>
    /// Extension methods for LengthUnit.
    /// These methods contain all unit conversion logic.
    /// </summary>
    public static class LengthUnitExtensions
    {
        /// <summary>
        /// Returns conversion factor of the unit relative to FEET.
        /// </summary>
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

        /// <summary>
        /// Convert value from current unit to BASE UNIT (Feet)
        /// </summary>
        public static double ConvertToBaseUnit(this LengthUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        /// <summary>
        /// Convert value from BASE UNIT (Feet) to current unit
        /// </summary>
        public static double ConvertFromBaseUnit(this LengthUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        /// <summary>
        /// Convert value from one unit to another unit
        /// </summary>
        public static double ConvertTo(this LengthUnit fromUnit, double value, LengthUnit toUnit)
        {
            double baseValue = fromUnit.ConvertToBaseUnit(value);
            return toUnit.ConvertFromBaseUnit(baseValue);
        }
    }
}