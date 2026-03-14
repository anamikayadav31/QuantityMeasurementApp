namespace QuantityMeasurementApp.ModelLayer.Enums
{
    /// <summary>
    /// Supported length units.
    /// Base unit for conversions: FEET
    /// All conversion logic lives in BussinessLayer/Converters/LengthUnitConverter.cs
    /// </summary>
    public enum LengthUnit
    {
        FEET,
        INCHES,
        YARDS,
        CENTIMETERS
    }
}