namespace QuantityMeasurementApp.ModelLayer.Enums
{
    // ── UC3: Generic Quantity Class for DRY Principle ──────────────────
    // This enum groups all measurement categories so the service layer
    // can prevent mixing incompatible units (e.g. FEET + KILOGRAM).
    public enum MeasurementType
    {
        LENGTH,
        WEIGHT,
        VOLUME,
        TEMPERATURE
    }
}