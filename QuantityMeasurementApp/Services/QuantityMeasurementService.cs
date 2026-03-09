using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp.Services
{
    /// <summary>
    /// This service class performs operations on Quantity<T> objects.
    /// It works with:
    /// - LengthUnit
    /// - WeightUnit
    /// - VolumeUnit
    /// - TemperatureUnit (only equality, no arithmetic)
    /// </summary>
    public class QuantityMeasurementService
    {
        private const double EPSILON = 0.0001; // Small tolerance for comparing floating-point numbers

        // ---------------- ADDITION ----------------

        /// <summary>
        /// Add two quantities of the same type and return result in target unit
        /// </summary>
        public Quantity<T> Add<T>(Quantity<T> q1, Quantity<T> q2, T targetUnit) where T : Enum
        {
            if (q1 == null || q2 == null)
                throw new ArgumentNullException("Quantities cannot be null");

            // Temperature cannot be added
            if (typeof(T) == typeof(TemperatureUnit))
                throw new NotSupportedException("Temperature does not support addition");

            // Convert both quantities to base unit, then add
            double sumBase = ConvertToBase(q1) + ConvertToBase(q2);

            // Convert the sum back to target unit and return
            return ConvertFromBase(targetUnit, sumBase);
        }

        // ---------------- SUBTRACTION ----------------

        /// <summary>
        /// Subtract q2 from q1 and return result in target unit
        /// </summary>
        public Quantity<T> Subtract<T>(Quantity<T> q1, Quantity<T> q2, T targetUnit) where T : Enum
        {
            if (q1 == null || q2 == null)
                throw new ArgumentNullException("Quantities cannot be null");

            if (typeof(T) == typeof(TemperatureUnit))
                throw new NotSupportedException("Temperature does not support subtraction");

            // Convert both to base unit, then subtract
            double diffBase = ConvertToBase(q1) - ConvertToBase(q2);

            // Convert difference back to target unit
            return ConvertFromBase(targetUnit, diffBase);
        }

        // ---------------- DIVISION ----------------

        /// <summary>
        /// Divide q1 by q2 and return result as double
        /// </summary>
        public double Divide<T>(Quantity<T> q1, Quantity<T> q2) where T : Enum
        {
            if (q1 == null || q2 == null)
                throw new ArgumentNullException("Quantities cannot be null");

            if (typeof(T) == typeof(TemperatureUnit))
                throw new NotSupportedException("Temperature does not support division");

            double base2 = ConvertToBase(q2);
            if (Math.Abs(base2) < EPSILON) // Check for division by zero
                throw new DivideByZeroException("Cannot divide by zero quantity");

            return ConvertToBase(q1) / base2; // Divide in base unit
        }

        // ---------------- EQUALITY CHECK ----------------

        /// <summary>
        /// Check if two quantities are equal (even if units differ)
        /// Returns false if quantities are of different types
        /// </summary>
        public bool AreEqual<T1, T2>(Quantity<T1> q1, Quantity<T2> q2)
            where T1 : Enum
            where T2 : Enum
        {
            if (q1 == null || q2 == null) return false;

            // Quantities from different categories cannot be equal
            if (typeof(T1) != typeof(T2)) return false;

            // Convert both to base unit and compare with small tolerance
            double base1 = ConvertToBase((dynamic)q1);
            double base2 = ConvertToBase((dynamic)q2);

            return Math.Abs(base1 - base2) < EPSILON;
        }

        // ---------------- COMPARISON ----------------

        /// <summary>
        /// Compare two quantities of the same type
        /// Returns -1 if q1 < q2, 0 if equal, 1 if q1 > q2
        /// </summary>
        public int Compare<T>(Quantity<T> q1, Quantity<T> q2) where T : Enum
        {
            if (q1 == null || q2 == null)
                throw new ArgumentNullException("Quantity cannot be null");

            double base1 = ConvertToBase(q1);
            double base2 = ConvertToBase(q2);

            if (Math.Abs(base1 - base2) < EPSILON) return 0; // They are equal
            return base1 < base2 ? -1 : 1; // Compare normally
        }

        // ================= PRIVATE HELPERS =================

        /// <summary>
        /// Convert a quantity to its base unit value
        /// Example: 1 foot → 12 inches
        /// </summary>
        private double ConvertToBase<T>(Quantity<T> q)
        {
            return q.Unit switch
            {
                LengthUnit l => l.ConvertToBaseUnit(q.Value),
                WeightUnit w => w.ConvertToBaseUnit(q.Value),
                VolumeUnit v => v.ConvertToBaseUnit(q.Value),
                TemperatureUnit t => t.ConvertToBaseUnit(q.Value),
                _ => throw new NotSupportedException($"Unit type {typeof(T)} not supported")
            };
        }

        /// <summary>
        /// Convert a base unit value to the target unit
        /// Example: 24 inches → 2 feet
        /// </summary>
        private Quantity<T> ConvertFromBase<T>(T targetUnit, double baseValue)
        {
            double value = targetUnit switch
            {
                LengthUnit l => l.ConvertFromBaseUnit(baseValue),
                WeightUnit w => w.ConvertFromBaseUnit(baseValue),
                VolumeUnit v => v.ConvertFromBaseUnit(baseValue),
                TemperatureUnit t => t.ConvertFromBaseUnit(baseValue),
                _ => throw new NotSupportedException($"Unit type {typeof(T)} not supported")
            };

            return new Quantity<T>(value, targetUnit);
        }
    }
}