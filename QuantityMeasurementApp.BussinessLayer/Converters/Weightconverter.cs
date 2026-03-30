using QuantityMeasurementApp.ModelLayer.Enums;

namespace QuantityMeasurementApp.BussinessLayer.Converters
{
    /// <summary>
    /// All conversion logic for WeightUnit.
    /// Base unit: KILOGRAM
    ///
    /// Conversion factors relative to 1 KILOGRAM:
    ///   KILOGRAM = 1.0
    ///   GRAM     = 0.001      (1000 grams = 1 kg)
    ///   POUND    = 0.453592   (1 lb ≈ 0.453592 kg)
    /// </summary>
    public static class WeightUnitConverter
    {
        public static double GetConversionFactor(WeightUnit unit)
        {
            return unit switch
            {
                WeightUnit.KILOGRAM => 1.0,
                WeightUnit.GRAM     => 0.001,
                WeightUnit.POUND    => 0.453592,
                _ => throw new InvalidOperationException($"Unsupported WeightUnit: {unit}")
            };
        }

        /// <summary>Convert value in given unit → KILOGRAM (base unit).</summary>
        public static double ToBase(WeightUnit unit, double value)
            => value * GetConversionFactor(unit);

        /// <summary>Convert value in KILOGRAM (base unit) → given unit.</summary>
        public static double FromBase(WeightUnit unit, double baseValue)
            => baseValue / GetConversionFactor(unit);
    }
}
