using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    // Handles length conversion and comparison
    public class QuantityMeasurementService
    {
        private const double EPSILON = 0.0001; // tolerance for comparison

        // Convert any quantity to base unit (FEET)
        private double ConvertToFeet(QuantityLength quantity)
        {
            if (quantity == null)
                throw new ArgumentNullException(nameof(quantity));

            // Use LengthUnit extension method for conversion
            return quantity.Value * quantity.Unit.ToFeetFactor();
        }

        // Convert a quantity to any target unit
        public QuantityLength ConvertTo(QuantityLength quantity, LengthUnit targetUnit)
        {
            if (quantity == null)
                throw new ArgumentNullException(nameof(quantity));

            double valueInFeet = ConvertToFeet(quantity);          // Convert to FEET
            double convertedValue = valueInFeet / targetUnit.ToFeetFactor(); // Convert to target
            return new QuantityLength(convertedValue, targetUnit); // Return new QuantityLength
        }

        // Check if two quantities are equal
        public bool AreEqual(QuantityLength q1, QuantityLength q2)
        {
            if (q1 == null || q2 == null)
                return false;

            double val1 = ConvertToFeet(q1);
            double val2 = ConvertToFeet(q2);

            return Math.Abs(val1 - val2) < EPSILON;
        }

        // Compare two quantities
        // Returns -1 if q1 < q2, 0 if equal, 1 if q1 > q2
        public int Compare(QuantityLength q1, QuantityLength q2)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentNullException("Quantity cannot be null");

            double val1 = ConvertToFeet(q1);
            double val2 = ConvertToFeet(q2);

            if (Math.Abs(val1 - val2) < EPSILON)
                return 0;

            return val1 < val2 ? -1 : 1;
        }
    }
}