using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementTest
{
    /// <summary>
    /// Test class for Quantity<T> using LengthUnit.
    /// 
    /// This class verifies:
    /// 1. Equality of quantities with same or different units
    /// 2. Unit conversion correctness
    /// 3. Addition of quantities
    /// 4. Enum conversion factor correctness
    /// 5. Validation for invalid input values
    /// 
    /// The base unit used internally for length is FEET.
    /// </summary>
    [TestClass]
    public class QuantityLengthTest
    {

        
        // EQUALITY TESTS
        

        /// <summary>
        /// Test if two quantities having the same value and unit
        /// are considered equal after conversion to base unit.
        /// </summary>
        [TestMethod]
        public void Equal_WhenSameValueAndUnit()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.FEET);

            // Convert both quantities to base unit (feet)
            double base1 = q1.Unit.ConvertToBaseUnit(q1.Value);
            double base2 = q2.Unit.ConvertToBaseUnit(q2.Value);

            // Assert both base values are equal
            Assert.AreEqual(base1, base2, 0.0001);
        }

        /// <summary>
        /// Test equality when quantities have different units
        /// but represent the same physical length.
        /// Example: 12 inches == 1 foot
        /// </summary>
        [TestMethod]
        public void Equal_WhenDifferentUnitsButSameLength()
        {
            var inches = new Quantity<LengthUnit>(12, LengthUnit.INCHES);
            var feet = new Quantity<LengthUnit>(1, LengthUnit.FEET);

            // Convert both to base unit
            double base1 = inches.Unit.ConvertToBaseUnit(inches.Value);
            double base2 = feet.Unit.ConvertToBaseUnit(feet.Value);

            Assert.AreEqual(base1, base2, 0.0001);
        }

        /// <summary>
        /// Test that two quantities with different values
        /// are not equal after conversion.
        /// </summary>
        [TestMethod]
        public void NotEqual_WhenDifferentValues()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.FEET);

            double base1 = q1.Unit.ConvertToBaseUnit(q1.Value);
            double base2 = q2.Unit.ConvertToBaseUnit(q2.Value);

            Assert.AreNotEqual(base1, base2, 0.0001);
        }

        
        // CONVERSION TESTS
        

        /// <summary>
        /// Verify that 1 foot converts to 12 inches.
        /// </summary>
        [TestMethod]
        public void Convert_FeetToInches()
        {
            var q = new Quantity<LengthUnit>(1, LengthUnit.FEET);

            // Convert quantity to inches
            var result = q.ConvertTo(
                (u, v) => u.ConvertFromBaseUnit(v),
                (u, v) => u.ConvertToBaseUnit(v),
                LengthUnit.INCHES
            );

            Assert.AreEqual(12, result.Value, 0.0001);
        }

        /// <summary>
        /// Verify that 24 inches converts to 2 feet.
        /// </summary>
        [TestMethod]
        public void Convert_InchesToFeet()
        {
            var q = new Quantity<LengthUnit>(24, LengthUnit.INCHES);

            var result = q.ConvertTo(
                (u, v) => u.ConvertFromBaseUnit(v),
                (u, v) => u.ConvertToBaseUnit(v),
                LengthUnit.FEET
            );

            Assert.AreEqual(2, result.Value, 0.0001);
        }

        /// <summary>
        /// Verify that 1 foot converts to 30.48 centimeters.
        /// </summary>
        [TestMethod]
        public void Convert_FeetToCentimeters()
        {
            var q = new Quantity<LengthUnit>(1, LengthUnit.FEET);

            var result = q.ConvertTo(
                (u, v) => u.ConvertFromBaseUnit(v),
                (u, v) => u.ConvertToBaseUnit(v),
                LengthUnit.CENTIMETERS
            );

            Assert.AreEqual(30.48, result.Value, 0.01);
        }

        
        // ADDITION TESTS
        

        /// <summary>
        /// Verify addition when both quantities use the same unit.
        /// Example: 1 foot + 2 feet = 3 feet
        /// </summary>
        [TestMethod]
        public void Add_SameUnit()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.FEET);

            var result = q1.Add(
                q2,
                (u, v) => u.ConvertToBaseUnit(v),
                (u, v) => u.ConvertFromBaseUnit(v)
            );

            Assert.AreEqual(3, result.Value, 0.0001);
        }

        /// <summary>
        /// Verify addition when units are different.
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

            Assert.AreEqual(2, result.Value, 0.0001);
        }

        
        // ENUM CONVERSION FACTOR TESTS
        

        /// <summary>
        /// Verify conversion factor for FEET unit.
        /// </summary>
        [TestMethod]
        public void LengthUnit_FeetConversionFactor()
        {
            double factor = LengthUnit.FEET.GetConversionFactor();

            Assert.AreEqual(1.0, factor, 0.0001);
        }

        /// <summary>
        /// Verify conversion factor for INCHES unit.
        /// </summary>
        [TestMethod]
        public void LengthUnit_InchesConversionFactor()
        {
            double factor = LengthUnit.INCHES.GetConversionFactor();

            Assert.AreEqual(1.0 / 12.0, factor, 0.0001);
        }

        /// <summary>
        /// Verify conversion factor for YARDS unit.
        /// </summary>
        [TestMethod]
        public void LengthUnit_YardsConversionFactor()
        {
            double factor = LengthUnit.YARDS.GetConversionFactor();

            Assert.AreEqual(3.0, factor, 0.0001);
        }

        /// <summary>
        /// Verify conversion factor for CENTIMETERS unit.
        /// </summary>
        [TestMethod]
        public void LengthUnit_CentimetersConversionFactor()
        {
            double factor = LengthUnit.CENTIMETERS.GetConversionFactor();

            Assert.AreEqual(1.0 / 30.48, factor, 0.0001);
        }

        
        // BASE UNIT CONVERSION TESTS
        

        /// <summary>
        /// Verify converting inches to base unit (feet).
        /// Example: 12 inches = 1 foot
        /// </summary>
        [TestMethod]
        public void ConvertToBaseUnit_InchesToFeet()
        {
            double result = LengthUnit.INCHES.ConvertToBaseUnit(12);

            Assert.AreEqual(1, result, 0.0001);
        }

        /// <summary>
        /// Verify converting yards to base unit (feet).
        /// Example: 1 yard = 3 feet
        /// </summary>
        [TestMethod]
        public void ConvertToBaseUnit_YardsToFeet()
        {
            double result = LengthUnit.YARDS.ConvertToBaseUnit(1);

            Assert.AreEqual(3, result, 0.0001);
        }

        /// <summary>
        /// Verify converting base unit (feet) to inches.
        /// </summary>
        [TestMethod]
        public void ConvertFromBaseUnit_FeetToInches()
        {
            double result = LengthUnit.INCHES.ConvertFromBaseUnit(1);

            Assert.AreEqual(12, result, 0.0001);
        }

        /// <summary>
        /// Verify converting base unit (feet) to centimeters.
        /// </summary>
        [TestMethod]
        public void ConvertFromBaseUnit_FeetToCentimeters()
        {
            double result = LengthUnit.CENTIMETERS.ConvertFromBaseUnit(1);

            Assert.AreEqual(30.48, result, 0.01);
        }

        
        // VALIDATION TESTS
        

        /// <summary>
        /// Verify that constructor throws an exception
        /// when an invalid numeric value (NaN) is passed.
        /// </summary>
        [TestMethod]
        public void Constructor_Throws_WhenInvalidNumber()
        {
            try
            {
                var q = new Quantity<LengthUnit>(double.NaN, LengthUnit.FEET);

                // If exception not thrown, test fails
                Assert.Fail("Expected exception not thrown");
            }
            catch (ArgumentException)
            {
                // Test passes if exception is thrown
                Assert.IsTrue(true);
            }
        }
    }
}