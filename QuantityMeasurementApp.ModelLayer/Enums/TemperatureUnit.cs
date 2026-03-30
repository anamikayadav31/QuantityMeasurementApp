namespace QuantityMeasurementApp.ModelLayer.Enums
{
    // ── UC14: Temperature Measurement ───────────────────────────────────
    // Enum lists every supported temperature unit.
    // Base unit for all conversions = CELSIUS.
    // NOTE: Temperature does NOT support Add/Subtract/Divide —
    //       only Compare and Convert are valid.
    // Conversion logic: BussinessLayer/Converters/TemperatureUnitConverter.cs
    public enum TemperatureUnit
    {
        CELSIUS,
        FAHRENHEIT,
        KELVIN
    }
}