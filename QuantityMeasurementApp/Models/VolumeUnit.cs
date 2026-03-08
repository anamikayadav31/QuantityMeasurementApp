using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Enumeration representing different volume units.
    /// 
    /// Base Unit: LITRE
    /// All conversions are internally calculated using litres
    /// as the base measurement unit.
    /// </summary>
    public enum VolumeUnit
    {
        LITRE,
        MILLILITRE,
        GALLON
    }

    /// <summary>
    /// Extension methods for VolumeUnit enum.
    /// These methods help perform unit conversions
    /// and provide utility operations related to volume units.
    /// </summary>
    public static class VolumeUnitExtensions
    {
        /// <summary>
        /// Returns the conversion factor of a unit relative to the base unit (Litre).
        /// 
        /// Examples:
        /// 1 Litre = 1 Litre
        /// 1 Millilitre = 0.001 Litre
        /// 1 Gallon = 3.78541 Litres
        /// </summary>
        public static double GetConversionFactor(this VolumeUnit unit)
        {
            switch (unit)
            {
                case VolumeUnit.LITRE:
                    return 1.0;

                case VolumeUnit.MILLILITRE:
                    return 0.001;

                case VolumeUnit.GALLON:
                    return 3.78541;

                default:
                    throw new ArgumentException("Invalid unit");
            }
        }

        /// <summary>
        /// Converts a value from the specified unit to the base unit (Litre).
        /// 
        /// Formula:
        /// BaseValue = GivenValue * ConversionFactor
        /// 
        /// Example:
        /// 1000 ml → 1000 * 0.001 = 1 litre
        /// </summary>
        public static double ConvertToBaseUnit(this VolumeUnit unit, double value)
        {
            return value * unit.GetConversionFactor();
        }

        /// <summary>
        /// Converts a value from the base unit (Litre) to the specified unit.
        /// 
        /// Formula:
        /// ConvertedValue = BaseValue / ConversionFactor
        /// 
        /// Example:
        /// 1 litre → 1 / 0.001 = 1000 ml
        /// </summary>
        public static double ConvertFromBaseUnit(this VolumeUnit unit, double baseValue)
        {
            return baseValue / unit.GetConversionFactor();
        }

        /// <summary>
        /// Returns the string name of the unit.
        /// Example: LITRE, MILLILITRE, GALLON
        /// </summary>
        public static string GetUnitName(this VolumeUnit unit)
        {
            return unit.ToString();
        }
    }
}