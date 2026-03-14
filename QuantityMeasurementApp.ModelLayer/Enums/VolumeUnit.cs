namespace QuantityMeasurementApp.ModelLayer.Enums
{
    /// <summary>
    /// Supported volume units.
    /// Base unit for conversions: LITRE
    /// All conversion logic lives in BussinessLayer/Converters/VolumeUnitConverter.cs
    /// </summary>
    public enum VolumeUnit
    {
        LITRE,
        MILLILITRE,
        GALLON
    }
}