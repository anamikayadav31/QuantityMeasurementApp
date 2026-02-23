using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    // Handles length conversion, comparison, and addition
    public class QuantityMeasurementService
    {
        private const double EPSILON = 0.0001; // small tolerance for equality check

        // Convert any QuantityLength to base unit (FEET)
        private double ConvertToFeet(QuantityLength quantity)
        {
            if (quantity == null)
                throw new ArgumentNullException(nameof(quantity));

            // Use enum's conversion factor
            return quantity.Value * quantity.Unit.ToFeetFactor();
        }

        // Convert a QuantityLength to a target unit
        public QuantityLength ConvertTo(QuantityLength quantity, LengthUnit targetUnit)
        {
            if (quantity == null)
                throw new ArgumentNullException(nameof(quantity));

            double valueInFeet = ConvertToFeet(quantity);               // convert to feet
            double convertedValue = valueInFeet / targetUnit.ToFeetFactor(); // convert to target unit
            return new QuantityLength(convertedValue, targetUnit);      // return new object
        }

        // Check if two quantities are equal (within EPSILON)
        public bool AreEqual(QuantityLength q1, QuantityLength q2)
        {
            if (q1 == null || q2 == null)
                return false;

            double val1 = ConvertToFeet(q1);
            double val2 = ConvertToFeet(q2);

            return Math.Abs(val1 - val2) < EPSILON;
        }

        // UC6: Add two quantities (result unit = first operand's unit)
        public QuantityLength Add(QuantityLength q1, QuantityLength q2)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentException("Quantities cannot be null");

            // delegate addition to QuantityLength's Add method
            return q1.Add(q2);
        }

        // Compare two quantities
        // Returns: -1 if q1 < q2, 0 if equal, 1 if q1 > q2
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