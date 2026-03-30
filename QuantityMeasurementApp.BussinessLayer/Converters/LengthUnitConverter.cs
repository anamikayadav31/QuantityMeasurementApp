using QuantityMeasurementApp.ModelLayer.Enums;

namespace QuantityMeasurementApp.BussinessLayer.Converters
{
    /// <summary>
    /// All conversion logic for LengthUnit.
    /// Base unit: FEET
    ///
    /// Conversion factors relative to 1 FOOT:
    ///   FEET        = 1.0
    ///   INCHES      = 1/12   (12 inches = 1 foot)
    ///   YARDS       = 3.0    (1 yard = 3 feet)
    ///   CENTIMETERS = 1/30.48 (30.48 cm = 1 foot)
    /// </summary>
    public static class LengthUnitConverter
    {
        public static double GetConversionFactor(LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.FEET        => 1.0,
                LengthUnit.INCHES      => 1.0 / 12.0,
                LengthUnit.YARDS       => 3.0,
                LengthUnit.CENTIMETERS => 1.0 / 30.48,
                _ => throw new InvalidOperationException($"Unsupported LengthUnit: {unit}")
            };
        }

        /// <summary>Convert value in given unit → FEET (base unit).</summary>
        public static double ToBase(LengthUnit unit, double value)
            => value * GetConversionFactor(unit);

        /// <summary>Convert value in FEET (base unit) → given unit.</summary>
        public static double FromBase(LengthUnit unit, double baseValue)
            => baseValue / GetConversionFactor(unit);
    }
}
