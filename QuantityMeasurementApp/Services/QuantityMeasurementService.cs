using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    /// <summary>
    /// Generic service class to perform operations on Quantity<T>.
    /// This service works for any unit type like LengthUnit or WeightUnit.
    /// </summary>
    public class QuantityMeasurementService
    {
        /// <summary>
        /// Small tolerance value used for comparing decimal numbers
        /// </summary>
        private const double EPSILON = 0.0001;

        /// <summary>
        /// Convert quantity to another unit
        /// </summary>
        public Quantity<T> ConvertTo<T>(
            Quantity<T> quantity,
            Func<T, double, double> convertFromBase,
            Func<T, double, double> convertToBase,
            T targetUnit)
        {
            if (quantity == null)
                throw new ArgumentNullException(nameof(quantity));

            return quantity.ConvertTo(convertFromBase, convertToBase, targetUnit);
        }

        /// <summary>
        /// Add two quantities
        /// </summary>
        public Quantity<T> Add<T>(
            Quantity<T> q1,
            Quantity<T> q2,
            Func<T, double, double> convertToBase,
            Func<T, double, double> convertFromBase)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentException("Quantities cannot be null");

            return q1.Add(q2, convertToBase, convertFromBase);
        }

        /// <summary>
        /// Check if two quantities are equal
        /// </summary>
        public bool AreEqual<T>(
            Quantity<T> q1,
            Quantity<T> q2,
            Func<T, double, double> convertToBase)
        {
            if (q1 == null || q2 == null)
                return false;

            double val1 = convertToBase(q1.Unit, q1.Value);
            double val2 = convertToBase(q2.Unit, q2.Value);

            return Math.Abs(val1 - val2) < EPSILON;
        }

        /// <summary>
        /// Compare two quantities
        /// -1 → q1 smaller
        ///  0 → equal
        ///  1 → q1 greater
        /// </summary>
        public int Compare<T>(
            Quantity<T> q1,
            Quantity<T> q2,
            Func<T, double, double> convertToBase)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentNullException("Quantity cannot be null");

            double val1 = convertToBase(q1.Unit, q1.Value);
            double val2 = convertToBase(q2.Unit, q2.Value);

            if (Math.Abs(val1 - val2) < EPSILON)
                return 0;

            if (val1 < val2)
                return -1;
            else
                return 1;
        }
    }
}