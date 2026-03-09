using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementTest
{
    /// <summary>
    /// This class contains unit tests for LengthUnit operations in QuantityMeasurementService.
    /// Unit tests are used to check if the code behaves correctly.
    /// 
    /// Tests included:
    /// 1. Equality of lengths (same unit and different units)
    /// 2. Addition of lengths
    /// 3. Subtraction of lengths
    /// 4. Division of lengths
    /// 5. Validation for invalid values in constructor
    /// </summary>
    [TestClass] // Marks this class as containing test methods
    public class QuantityLengthTest
    {
        // Service instance. Currently not used directly but can be used for more complex operations.
        private QuantityMeasurementService service = new QuantityMeasurementService();

        // ---------------- EQUALITY TESTS ----------------

        /// <summary>
        /// Test: Two lengths with same value and same unit should be equal
        /// </summary>
        [TestMethod] // Marks this method as a test case
        public void Equal_WhenSameValueAndUnit()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.FEET); // 1 foot
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.FEET); // 1 foot

            // Convert both to base unit (usually inches) to compare values
            double base1 = q1.Unit.ConvertToBaseUnit(q1.Value);
            double base2 = q2.Unit.ConvertToBaseUnit(q2.Value);

            // Assert that the values are equal
            Assert.AreEqual(base1, base2, 0.0001); // 0.0001 is tolerance for floating-point comparison
        }

        /// <summary>
        /// Test: Two lengths with different units but equivalent values should be equal
        /// Example: 12 inches = 1 foot
        /// </summary>
        [TestMethod]
        public void Equal_WhenDifferentUnitsButSameLength()
        {
            var inches = new Quantity<LengthUnit>(12, LengthUnit.INCHES); // 12 inches
            var feet = new Quantity<LengthUnit>(1, LengthUnit.FEET);       // 1 foot

            double base1 = inches.Unit.ConvertToBaseUnit(inches.Value);
            double base2 = feet.Unit.ConvertToBaseUnit(feet.Value);

            Assert.AreEqual(base1, base2, 0.0001); // They should be equal in base unit
        }

        /// <summary>
        /// Test: Two lengths with different values should not be equal
        /// Example: 1 foot != 2 feet
        /// </summary>
        [TestMethod]
        public void NotEqual_WhenDifferentValues()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.FEET);

            double base1 = q1.Unit.ConvertToBaseUnit(q1.Value);
            double base2 = q2.Unit.ConvertToBaseUnit(q2.Value);

            Assert.AreNotEqual(base1, base2, 0.0001); // They should not be equal
        }

        // ---------------- ADDITION TESTS ----------------

        /// <summary>
        /// Test: Add two lengths with the same unit
        /// Example: 1 foot + 2 feet = 3 feet
        /// </summary>
        [TestMethod]
        public void Add_SameUnit()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.FEET);

            // Add q2 to q1, converting units as necessary
            var result = q1.Add(
                q2,
                (u, v) => u.ConvertToBaseUnit(v),   // Convert q2 to base unit
                (u, v) => u.ConvertFromBaseUnit(v)  // Convert back to original unit
            );

            Assert.AreEqual(3, result.Value, 0.0001); // Result should be 3 feet
        }

        /// <summary>
        /// Test: Add two lengths with different units
        /// Example: 1 foot + 12 inches = 2 feet
        /// </summary>
        [TestMethod]
        public void Add_DifferentUnits()
        {
            var feet = new Quantity<LengthUnit>(1, LengthUnit.FEET);
            var inches = new Quantity<LengthUnit>(12, LengthUnit.INCHES);

            var result = feet.Add(
                inches,
                (u, v) => u.ConvertToBaseUnit(v),
                (u, v) => u.ConvertFromBaseUnit(v)
            );

            Assert.AreEqual(2, result.Value, 0.0001); // 1 foot + 12 inches = 2 feet
        }

        // ---------------- SUBTRACTION TEST ----------------

        /// <summary>
        /// Test: Subtract lengths with different units
        /// Example: 2 feet - 12 inches = 1 foot
        /// </summary>
        [TestMethod]
        public void Subtract_DifferentUnits()
        {
            var feet = new Quantity<LengthUnit>(2, LengthUnit.FEET);
            var inches = new Quantity<LengthUnit>(12, LengthUnit.INCHES);

            var result = feet.Subtract(
                inches,
                (u, v) => u.ConvertToBaseUnit(v),
                (u, v) => u.ConvertFromBaseUnit(v)
            );

            Assert.AreEqual(1, result.Value, 0.0001);
        }

        // ---------------- DIVISION TEST ----------------

        /// <summary>
        /// Test: Divide two lengths
        /// Example: 2 feet ÷ 1 foot = 2
        /// </summary>
        [TestMethod]
        public void Divide_TwoLengths()
        {
            var q1 = new Quantity<LengthUnit>(2, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.FEET);

            double result = q1.Divide(
                q2,
                (u, v) => u.ConvertToBaseUnit(v)
            );

            Assert.AreEqual(2, result, 0.0001); // Result should be 2
        }

        // ---------------- VALIDATION TEST ----------------

        /// <summary>
        /// Test: Constructor should throw exception when given invalid value
        /// Example: double.NaN is invalid
        /// </summary>
        [TestMethod]
        public void Constructor_Throws_WhenInvalidNumber()
        {
            bool exceptionThrown = false;
            try
            {
                var q = new Quantity<LengthUnit>(double.NaN, LengthUnit.FEET); // Invalid value
            }
            catch (ArgumentException) // Expected exception
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Expected ArgumentException for invalid value");
        }
    }
}