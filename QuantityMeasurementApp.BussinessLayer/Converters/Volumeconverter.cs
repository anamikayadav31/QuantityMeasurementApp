using QuantityMeasurementApp.ModelLayer.Enums;

namespace QuantityMeasurementApp.BussinessLayer.Converters
{
    /// <summary>
    /// All conversion logic for VolumeUnit.
    /// Base unit: LITRE
    ///
    /// Conversion factors relative to 1 LITRE:
    ///   LITRE      = 1.0
    ///   MILLILITRE = 0.001     (1000 ml = 1 L)
    ///   GALLON     = 3.78541   (1 US gallon ≈ 3.78541 L)
    /// </summary>
    public static class VolumeUnitConverter
    {
        public static double GetConversionFactor(VolumeUnit unit)
        {
            return unit switch
            {
                VolumeUnit.LITRE      => 1.0,
                VolumeUnit.MILLILITRE => 0.001,
                VolumeUnit.GALLON     => 3.78541,
                _ => throw new InvalidOperationException($"Unsupported VolumeUnit: {unit}")
            };
        }

        /// <summary>Convert value in given unit → LITRE (base unit).</summary>
        public static double ToBase(VolumeUnit unit, double value)
            => value * GetConversionFactor(unit);

        /// <summary>Convert value in LITRE (base unit) → given unit.</summary>
        public static double FromBase(VolumeUnit unit, double baseValue)
            => baseValue / GetConversionFactor(unit);
    }
}
