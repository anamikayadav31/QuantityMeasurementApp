using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementTest
{
    /// <summary>
    /// Test class for Quantity using LengthUnit.
    /// This class checks equality, conversion, addition,
    /// subtraction, division and validation.
    /// </summary>
    [TestClass]
    public class QuantityLengthTest
    {

        // ---------------- EQUALITY TESTS ----------------

        /// <summary>
        /// Test when both quantities have same value and same unit.
        /// They should be equal.
        /// </summary>
        [TestMethod]
        public void Equal_WhenSameValueAndUnit()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.FEET);

            double base1 = q1.Unit.ConvertToBaseUnit(q1.Value);
            double base2 = q2.Unit.ConvertToBaseUnit(q2.Value);

            Assert.AreEqual(base1, base2, 0.0001);
        }

        /// <summary>
        /// Test when units are different but represent same length.
        /// Example: 12 inches = 1 foot
        /// </summary>
        [TestMethod]
        public void Equal_WhenDifferentUnitsButSameLength()
        {
            var inches = new Quantity<LengthUnit>(12, LengthUnit.INCHES);
            var feet = new Quantity<LengthUnit>(1, LengthUnit.FEET);

            double base1 = inches.Unit.ConvertToBaseUnit(inches.Value);
            double base2 = feet.Unit.ConvertToBaseUnit(feet.Value);

            Assert.AreEqual(base1, base2, 0.0001);
        }

        /// <summary>
        /// Test when values are different.
        /// They should not be equal.
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


        // ---------------- CONVERSION TESTS ----------------

        /// <summary>
        /// Test conversion from feet to inches.
        /// 1 foot = 12 inches
        /// </summary>
        [TestMethod]
        public void Convert_FeetToInches()
        {
            var q = new Quantity<LengthUnit>(1, LengthUnit.FEET);

            var result = q.ConvertTo(
                (u, v) => u.ConvertFromBaseUnit(v),
                (u, v) => u.ConvertToBaseUnit(v),
                LengthUnit.INCHES
            );

            Assert.AreEqual(12, result.Value, 0.0001);
        }

        /// <summary>
        /// Test conversion from inches to feet.
        /// 24 inches = 2 feet
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
        /// Test conversion from feet to centimeters.
        /// 1 foot = 30.48 cm
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


        // ---------------- ADDITION TESTS ----------------

        /// <summary>
        /// Test addition when both units are same.
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
        /// Test addition when units are different.
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


        // ---------------- SUBTRACTION TEST ----------------

        /// <summary>
        /// Test subtraction when units are different.
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
        /// Test division of two quantities.
        /// Example: 2 feet / 1 foot = 2
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

            Assert.AreEqual(2, result, 0.0001);
        }


        // ---------------- VALIDATION TEST ----------------

        /// <summary>
        /// Test constructor validation for invalid value.
        /// If value is NaN, exception should be thrown.
        /// </summary>
        [TestMethod]
        public void Constructor_Throws_WhenInvalidNumber()
        {
            try
            {
                var q = new Quantity<LengthUnit>(double.NaN, LengthUnit.FEET);

                Assert.Fail("Expected exception not thrown");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }
    }
}