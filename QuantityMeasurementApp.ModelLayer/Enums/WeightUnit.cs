namespace QuantityMeasurementApp.ModelLayer.Enums
{
    /// <summary>
    /// Supported weight units.
    /// Base unit for conversions: KILOGRAM
    /// All conversion logic lives in BussinessLayer/Converters/WeightUnitConverter.cs
    /// </summary>
    public enum WeightUnit
    {
        KILOGRAM,
        GRAM,
        POUND
    }
}