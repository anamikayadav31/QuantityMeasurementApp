namespace QuantityMeasurementApp.ModelLayer.Enums
{
    // ── UC11: Volume Measurement ─────────────────────────────────────────
    // Enum lists every supported volume unit.
    // Base unit for all conversions = LITRE.
    // Conversion factors live in BussinessLayer/Converters/VolumeUnitConverter.cs
    public enum VolumeUnit
    {
        LITRE,
        MILLILITRE,
        GALLON
    }
}