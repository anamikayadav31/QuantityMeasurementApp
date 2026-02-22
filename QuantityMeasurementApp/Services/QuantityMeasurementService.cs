using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    // Handles all length conversion & comparison logic
    public class QuantityMeasurementService
    {
        private const double INCH_TO_FEET = 1.0 / 12.0;

        // Convert any unit to base unit (Feet)
        private double ConvertToFeet(QuantityLength quantity)
        {
            return quantity.Unit switch
            {
                LengthUnit.FEET => quantity.Value,
                LengthUnit.INCH => quantity.Value * INCH_TO_FEET,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }

        // Compare two quantities
        public bool AreEqual(QuantityLength q1, QuantityLength q2)
        {
            if (q1 == null || q2 == null)
                return false;

            double value1 = ConvertToFeet(q1);
            double value2 = ConvertToFeet(q2);

            return Math.Abs(value1 - value2) < 0.0001;
        }
    }
}