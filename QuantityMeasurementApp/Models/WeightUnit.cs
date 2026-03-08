using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Supported weight units.
    /// KILOGRAM is the base unit.
    /// </summary>
    public enum WeightUnit
    {
        KILOGRAM,
        GRAM,
        POUND
    }

    /// <summary>
    /// Extension methods for WeightUnit.
    /// Handles all weight conversions.
    /// </summary>
    public static class WeightUnitExtensions
    {
        /// <summary>
        /// Returns conversion factor relative to kilogram.
        /// </summary>
        public static double GetConversionFactor(this WeightUnit unit)
        {
            switch (unit)
            {
                case WeightUnit.KILOGRAM:
                    return 1.0;

                case WeightUnit.GRAM:
                    return 0.001; // 1 g = 0.001 kg

                case WeightUnit.POUND:
                    return 0.453592; // 1 lb = 0.453592 kg

                default:
                    throw new ArgumentException("Invalid unit");
            }
        }

        /// <summary>
        /// Convert value to base unit (kilogram)
        /// </summary>
        public static double ConvertToBaseUnit(this WeightUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        /// <summary>
        /// Convert from base unit (kilogram) to this unit
        /// </summary>
        public static double ConvertFromBaseUnit(this WeightUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }
    }
}