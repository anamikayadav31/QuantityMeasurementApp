namespace QuantityMeasurementApp.ModelLayer.Enums
{
    // ── UC1/UC2: Feet Measurement Equality & UC4: Extended Unit Support ──
    // Enum lists every supported length unit.
    // Base unit for all conversions = FEET.
    // Conversion factors live in BussinessLayer/Converters/LengthUnitConverter.cs
    public enum LengthUnit
    {
        FEET,
        INCHES,
        YARDS,
        CENTIMETERS
    }
}