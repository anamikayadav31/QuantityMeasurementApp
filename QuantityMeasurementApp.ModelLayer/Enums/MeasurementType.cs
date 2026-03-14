namespace QuantityMeasurementApp.ModelLayer.Enums
{
    /// <summary>
    /// Categorises a quantity into one of the four supported measurement domains.
    ///
    /// Used by:
    ///   - QuantityDTO  to tag which category its unit belongs to
    ///   - ServiceImpl  to prevent cross-category operations
    ///                  (e.g. cannot Add LENGTH to WEIGHT)
    ///   - UnitParser   to resolve a unit string to the correct enum type
    /// </summary>
    public enum MeasurementType
    {
        LENGTH,
        WEIGHT,
        VOLUME,
        TEMPERATURE
    }
}