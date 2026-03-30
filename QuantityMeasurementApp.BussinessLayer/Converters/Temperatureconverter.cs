using QuantityMeasurementApp.ModelLayer.Enums;
using QuantityMeasurementApp.ModelLayer.Exceptions; // <- Custom exception

namespace QuantityMeasurementApp.BussinessLayer.Converters
{
    /// <summary>
    /// All conversion logic for TemperatureUnit.
    /// Base unit: CELSIUS
    ///
    /// Temperature uses non-linear formulas (offset scales),
    /// NOT a simple multiplication factor like other units.
    ///
    /// Temperature does NOT support arithmetic (Add/Subtract/Divide).
    /// Only Compare and Convert are valid operations.
    /// </summary>
    public static class TemperatureUnitConverter
    {
        /// <summary>
        /// Convert value from given unit → CELSIUS (base unit).
        ///   CELSIUS    → no change
        ///   FAHRENHEIT → (F - 32) × 5/9
        ///   KELVIN     → K - 273.15
        /// </summary>
        public static double ToBase(TemperatureUnit unit, double value)
        {
            return unit switch
            {
                TemperatureUnit.CELSIUS    => value,
                TemperatureUnit.FAHRENHEIT => (value - 32.0) * 5.0 / 9.0,
                TemperatureUnit.KELVIN     => value - 273.15,
                _ => throw new QuantityMeasurementException($"Unsupported TemperatureUnit: {unit}")
            };
        }

        /// <summary>
        /// Convert value from CELSIUS (base unit) → given unit.
        ///   CELSIUS    → no change
        ///   FAHRENHEIT → (C × 9/5) + 32
        ///   KELVIN     → C + 273.15
        /// </summary>
        public static double FromBase(TemperatureUnit unit, double baseValue)
        {
            return unit switch
            {
                TemperatureUnit.CELSIUS    => baseValue,
                TemperatureUnit.FAHRENHEIT => (baseValue * 9.0 / 5.0) + 32.0,
                TemperatureUnit.KELVIN     => baseValue + 273.15,
                _ => throw new QuantityMeasurementException($"Unsupported TemperatureUnit: {unit}")
            };
        }

        /// <summary>
        /// Throws QuantityMeasurementException to prevent arithmetic on Temperature.
        /// Called by ServiceImpl before any Add / Subtract / Divide operation.
        /// </summary>
        public static void ValidateArithmeticNotSupported(string operation)
        {
            throw new QuantityMeasurementException(
                $"Temperature does not support arithmetic operation '{operation}'. " +
                "Only Compare and Convert are allowed for Temperature.");
        }
    }
}