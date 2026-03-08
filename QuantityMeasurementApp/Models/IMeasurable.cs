namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Interface for measurement units like Length and Weight
    /// </summary>
    public interface IMeasurable
    {
        // Returns conversion factor of the unit
        double GetConversionFactor();

        // Converts a value to base unit
        double ConvertToBaseUnit(double value);

        // Converts base unit value to this unit
        double ConvertFromBaseUnit(double baseValue);

        // Returns the unit name
        string GetUnitName();
    }
}