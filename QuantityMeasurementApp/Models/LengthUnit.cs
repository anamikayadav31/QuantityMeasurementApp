using System;

namespace QuantityMeasurementApp.Models
{
    
    // Represents supported length units.
    // Conversion is done relative to FEET (base unit).
    // Each unit stores a conversion factor to FEET.
  
    public enum LengthUnit
    {
        FEET,        // Base unit
        INCHES,        // 1 inch = 1/12 feet
        YARDS,       // 1 yard = 3 feet
        CENTIMETERS  // 1 cm ≈ 1/30.48 feet
    }

    public static class LengthUnitExtensions
    {
        
        // Returns the conversion factor to base unit (FEET)
       
        public static double ToFeetFactor(this LengthUnit unit)
        {
            return unit switch
            {
                LengthUnit.FEET => 1.0,
                LengthUnit.INCHES => 1.0 / 12.0,
                LengthUnit.YARDS => 3.0,
                LengthUnit.CENTIMETERS => 1.0 / 30.48,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }

        
        // Converts a numeric value from this unit to the target unit
       
        public static double ConvertTo(this double value, LengthUnit fromUnit, LengthUnit toUnit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            double valueInFeet = value * fromUnit.ToFeetFactor();
            double convertedValue = valueInFeet / toUnit.ToFeetFactor();
            return convertedValue;
        }
    }
}