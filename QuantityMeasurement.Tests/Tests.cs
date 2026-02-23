using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementTest
{
    [TestClass]
    public class QuantityLengthTest
    {
   
        //  EQUALITY TESTS
   

        [TestMethod]
        public void Equal_WhenSameValueAndUnit()
        {
            var q1 = new QuantityLength(1, LengthUnit.FEET);
            var q2 = new QuantityLength(1, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void Equal_WhenDifferentUnitsButSameLength()
        {
            var inches = new QuantityLength(12, LengthUnit.INCHES);
            var feet = new QuantityLength(1, LengthUnit.FEET);

            Assert.IsTrue(inches.Equals(feet)); // 12 inches = 1 foot
        }

        [TestMethod]
        public void NotEqual_WhenDifferentValues()
        {
            var q1 = new QuantityLength(1, LengthUnit.FEET);
            var q2 = new QuantityLength(2, LengthUnit.FEET);

            Assert.IsFalse(q1.Equals(q2));
        }

        [TestMethod]
        public void NotEqual_WhenComparedWithNull()
        {
            var q = new QuantityLength(1, LengthUnit.FEET);

            Assert.IsFalse(q.Equals(null));
        }

        [TestMethod]
        public void Constructor_Throws_WhenInvalidNumber()
        {
            try
            {
                var q = new QuantityLength(double.NaN, LengthUnit.FEET);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }

   
        //  CONVERSION TESTS
   

        [TestMethod]
        public void Convert_FeetToInches()
        {
            double result = QuantityLength.Convert(1, LengthUnit.FEET, LengthUnit.INCHES);
            Assert.AreEqual(12, result, 0.0001);
        }

        [TestMethod]
        public void Convert_InchesToFeet()
        {
            double result = QuantityLength.Convert(24, LengthUnit.INCHES, LengthUnit.FEET);
            Assert.AreEqual(2, result, 0.0001);
        }

        [TestMethod]
        public void Convert_FeetToCentimeters()
        {
            double result = QuantityLength.Convert(1, LengthUnit.FEET, LengthUnit.CENTIMETERS);
            Assert.AreEqual(30.48, result, 0.01);
        }

        [TestMethod]
        public void RoundTrip_Conversion_ReturnsOriginalValue()
        {
            double original = 5;

            double inches = QuantityLength.Convert(original, LengthUnit.FEET, LengthUnit.INCHES);
            double backToFeet = QuantityLength.Convert(inches, LengthUnit.INCHES, LengthUnit.FEET);

            Assert.AreEqual(original, backToFeet, 0.0001);
        }

   
        //  ADDITION TESTS
   

        [TestMethod]
        public void Add_SameUnit()
        {
            var q1 = new QuantityLength(1, LengthUnit.FEET);
            var q2 = new QuantityLength(2, LengthUnit.FEET);

            var result = q1.Add(q2);

            Assert.AreEqual(3, result.Value, 0.0001);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        [TestMethod]
        public void Add_DifferentUnits()
        {
            var feet = new QuantityLength(1, LengthUnit.FEET);
            var inches = new QuantityLength(12, LengthUnit.INCHES);

            var result = feet.Add(inches);

            Assert.AreEqual(2, result.Value, 0.0001);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        [TestMethod]
        public void Add_WithZero()
        {
            var q1 = new QuantityLength(5, LengthUnit.FEET);
            var zero = new QuantityLength(0, LengthUnit.INCHES);

            var result = q1.Add(zero);

            Assert.AreEqual(5, result.Value, 0.0001);
        }

        [TestMethod]
        public void Addition_IsCommutative()
        {
            var a = new QuantityLength(1, LengthUnit.FEET);
            var b = new QuantityLength(12, LengthUnit.INCHES);

            var sum1 = a.Add(b);
            var sum2 = b.Add(a);

            // Convert sum2 into sum1's unit before comparing
            Assert.AreEqual(sum1.Value, sum2.ConvertTo(sum1.Unit), 0.0001);
        }
    }
}