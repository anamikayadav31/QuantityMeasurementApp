namespace QuantityMeasurementApp.ModelLayer.Enums
{
    /// <summary>
    /// Supported temperature units.
    /// Base unit for conversions: CELSIUS
    /// All conversion logic lives in BussinessLayer/Converters/TemperatureUnitConverter.cs
    ///
    /// NOTE: Temperature does NOT support arithmetic (Add/Subtract/Divide).
    ///       Only Compare and Convert operations are valid.
    /// </summary>
    public enum TemperatureUnit
    {
        CELSIUS,
        FAHRENHEIT,
        KELVIN
    }
}