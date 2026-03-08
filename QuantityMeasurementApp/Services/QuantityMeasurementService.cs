using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    /// <summary>
    /// Service class responsible for handling all operations
    /// related to QuantityLength.
    /// 
    /// Operations:
    /// - Conversion
    /// - Equality check
    /// - Addition
    /// - Subtraction
    /// - Multiplication
    /// - Comparison
    /// </summary>
    public class QuantityMeasurementService
    {
        // Small tolerance value for decimal comparison
        private const double EPSILON = 0.0001;

        /// <summary>
        /// Convert quantity to another unit
        /// </summary>
        public QuantityLength ConvertTo(QuantityLength quantity, LengthUnit targetUnit)
        {
            if (quantity == null)
                throw new ArgumentNullException(nameof(quantity));

            double convertedValue = quantity.ConvertTo(targetUnit);

            return new QuantityLength(convertedValue, targetUnit);
        }

        /// <summary>
        /// Check if two quantities are equal
        /// </summary>
        public bool AreEqual(QuantityLength q1, QuantityLength q2)
        {
            if (q1 == null || q2 == null)
                return false;

            double val1 = q1.Unit.ConvertToBaseUnit(q1.Value);
            double val2 = q2.Unit.ConvertToBaseUnit(q2.Value);

            return Math.Abs(val1 - val2) < EPSILON;
        }

        /// <summary>
        /// Add two quantities
        /// </summary>
        public QuantityLength Add(QuantityLength q1, QuantityLength q2)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentException("Quantities cannot be null");

            return q1.Add(q2);
        }

        /// <summary>
        /// Add two quantities with target unit
        /// </summary>
        public QuantityLength Add(QuantityLength q1, QuantityLength q2, LengthUnit targetUnit)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentException("Quantities cannot be null");

            return q1.Add(q2, targetUnit);
        }

        /// <summary>
        /// Subtract two quantities
        /// </summary>
        public QuantityLength Subtract(QuantityLength q1, QuantityLength q2)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentException("Quantities cannot be null");

            return q1.Subtract(q2);
        }

        /// <summary>
        /// Multiply quantity with a number
        /// </summary>
        public QuantityLength Multiply(QuantityLength quantity, double factor)
        {
            if (quantity == null)
                throw new ArgumentNullException(nameof(quantity));

            return quantity.Multiply(factor);
        }

        /// <summary>
        /// Compare two quantities
        /// -1 → q1 smaller
        ///  0 → equal
        ///  1 → q1 greater
        /// </summary>
        public int Compare(QuantityLength q1, QuantityLength q2)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentNullException("Quantity cannot be null");

            double val1 = q1.Unit.ConvertToBaseUnit(q1.Value);
            double val2 = q2.Unit.ConvertToBaseUnit(q2.Value);

            if (Math.Abs(val1 - val2) < EPSILON)
                return 0;

            return val1 < val2 ? -1 : 1;
        }
    }
}