using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementTest
{
    [TestClass]
    public class QuantityLengthTest
    {
        private const double EPSILON = 1e-6;

        // ----- Equality Tests -----

        [TestMethod]
        public void Equality_FeetToFeet_SameValue()
        {
            // Same value, same unit
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);
            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void Equality_INCHESToINCHES_SameValue()
        {
            // Same value, same unit
            var q1 = new QuantityLength(12.0, LengthUnit.INCHES);
            var q2 = new QuantityLength(12.0, LengthUnit.INCHES);
            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void Equality_INCHESToFeet_Equivalent()
        {
            // 12 INCHESes = 1 foot
            var q1 = new QuantityLength(12.0, LengthUnit.INCHES);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);
            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void Equality_FeetToFeet_Different()
        {
            // Different values, same unit
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(2.0, LengthUnit.FEET);
            Assert.IsFalse(q1.Equals(q2));
        }

        [TestMethod]
        public void Equality_SameReference()
        {
            // Object equals itself
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            Assert.IsTrue(q1.Equals(q1));
        }

        [TestMethod]
        public void Equality_NullComparison()
        {
            // Object compared with null
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            Assert.IsFalse(q1.Equals(null));
        }

        [TestMethod]
     
public void Equality_InvalidNumeric_Throws()
{
    try
    {
        // Creating QuantityLength with NaN should throw exception
        new QuantityLength(double.NaN, LengthUnit.FEET);
        Assert.Fail("Expected ArgumentException was not thrown"); // Fail if no exception
    }
    catch (ArgumentException)
    {
        // Test passes
    }
}

        // ----- Yard Conversions -----
        [TestMethod]
        public void Equality_YardToFeet_Equivalent()
        {
            // 1 yard = 3 feet
            var q1 = new QuantityLength(1.0, LengthUnit.YARDS);
            var q2 = new QuantityLength(3.0, LengthUnit.FEET);
            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void Equality_YardToINCHESes_Equivalent()
        {
            // 1 yard = 36 INCHESes
            var q1 = new QuantityLength(1.0, LengthUnit.YARDS);
            var q2 = new QuantityLength(36.0, LengthUnit.INCHES);
            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void Equality_YardToYard_Different()
        {
            // Different yard values
            var q1 = new QuantityLength(1.0, LengthUnit.YARDS);
            var q2 = new QuantityLength(2.0, LengthUnit.YARDS);
            Assert.IsFalse(q1.Equals(q2));
        }

        // ----- Centimeter Conversions -----
        [TestMethod]
        public void Equality_CentimetersToINCHESes_Equivalent()
        {
            // 1 cm = 0.393701 INCHES
            var q1 = new QuantityLength(1.0, LengthUnit.CENTIMETERS);
            var q2 = new QuantityLength(0.393701, LengthUnit.INCHES);
            Assert.IsTrue(q1.Equals(q2));
        }

        [TestMethod]
        public void Equality_CentimetersToFeet_NonEquivalent()
        {
            // 1 cm != 1 foot
            var q1 = new QuantityLength(1.0, LengthUnit.CENTIMETERS);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);
            Assert.IsFalse(q1.Equals(q2));
        }

        // ----- Basic Conversion Tests -----
        [TestMethod]
        public void Convert_FeetToINCHESes()
        {
            // 1 foot → INCHESes
            double result = QuantityLength.Convert(1.0, LengthUnit.FEET, LengthUnit.INCHES);
            Assert.AreEqual(12.0, result, EPSILON);
        }

        [TestMethod]
        public void Convert_INCHESesToFeet()
        {
            // 24 INCHESes → feet
            double result = QuantityLength.Convert(24.0, LengthUnit.INCHES, LengthUnit.FEET);
            Assert.AreEqual(2.0, result, EPSILON);
        }

        [TestMethod]
        public void Convert_YardsToINCHESes()
        {
            // 1 yard → INCHESes
            double result = QuantityLength.Convert(1.0, LengthUnit.YARDS, LengthUnit.INCHES);
            Assert.AreEqual(36.0, result, EPSILON);
        }

        [TestMethod]
        public void Convert_FeetToCentimeters()
        {
            // 1 foot → centimeters
            double result = QuantityLength.Convert(1.0, LengthUnit.FEET, LengthUnit.CENTIMETERS);
            Assert.AreEqual(30.48, result, EPSILON);
        }

        [TestMethod]
        public void Convert_CentimetersToFeet()
        {
            // 30.48 cm → feet
            double result = QuantityLength.Convert(30.48, LengthUnit.CENTIMETERS, LengthUnit.FEET);
            Assert.AreEqual(1.0, result, EPSILON);
        }

        // ----- Round-Trip Conversion Tests -----
        [TestMethod]
        public void RoundTrip_Conversion_PreservesValue()
        {
            // Convert A → B → A should return original
            double original = 5.0;
            double converted = QuantityLength.Convert(original, LengthUnit.FEET, LengthUnit.INCHES);
            double back = QuantityLength.Convert(converted, LengthUnit.INCHES, LengthUnit.FEET);
            Assert.AreEqual(original, back, EPSILON);
        }

        // ----- Zero, Negative, Large, Small Values -----
        [TestMethod]
        public void Convert_ZeroValue()
        {
            double result = QuantityLength.Convert(0.0, LengthUnit.FEET, LengthUnit.INCHES);
            Assert.AreEqual(0.0, result, EPSILON);
        }

        [TestMethod]
        public void Convert_NegativeValue()
        {
            double result = QuantityLength.Convert(-1.0, LengthUnit.FEET, LengthUnit.INCHES);
            Assert.AreEqual(-12.0, result, EPSILON);
        }

        [TestMethod]
        public void Convert_LargeValue()
        {
            double result = QuantityLength.Convert(1e6, LengthUnit.FEET, LengthUnit.INCHES);
            Assert.AreEqual(12e6, result, EPSILON);
        }

        [TestMethod]
        public void Convert_SmallValue()
        {
            double result = QuantityLength.Convert(1e-6, LengthUnit.FEET, LengthUnit.INCHES);
            Assert.AreEqual(1e-6 * 12, result, EPSILON);
        }

        // ----- Same Unit Conversion -----
        [TestMethod]
        public void Convert_SameUnit()
        {
            double result = QuantityLength.Convert(5.0, LengthUnit.FEET, LengthUnit.FEET);
            Assert.AreEqual(5.0, result, EPSILON);
        }

        // ----- Invalid Inputs -----
        // NaN value should throw ArgumentException
        [TestMethod]
        public void Convert_NaN_Throws()
        {
            try
            {
                QuantityLength.Convert(double.NaN, LengthUnit.FEET, LengthUnit.INCHES);
                Assert.Fail("Expected ArgumentException was not thrown"); // Fail if no exception
            }
            catch (ArgumentException)
            {
                // Test passes
            }
        }

        [TestMethod]
        public void Convert_PositiveInfinity_Throws()
        {
            try
            {
                QuantityLength.Convert(double.PositiveInfinity, LengthUnit.FEET, LengthUnit.INCHES);
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Test passes
            }
        }

        [TestMethod]
        public void Convert_NegativeInfinity_Throws()
        {
            try
            {
                QuantityLength.Convert(double.NegativeInfinity, LengthUnit.FEET, LengthUnit.INCHES);
                Assert.Fail("Expected ArgumentException was not thrown");
            }
            catch (ArgumentException)
            {
                // Test passes
            }
        }
        // ----- Cross-Unit Multi-Step Conversions -----
        [TestMethod]
        public void Convert_FeetToYards()
        {
            double result = QuantityLength.Convert(6.0, LengthUnit.FEET, LengthUnit.YARDS);
            Assert.AreEqual(2.0, result, EPSILON);
        }

        [TestMethod]
        public void Convert_YardsToCentimeters()
        {
            double result = QuantityLength.Convert(1.0, LengthUnit.YARDS, LengthUnit.CENTIMETERS);
            Assert.AreEqual(91.44, result, EPSILON);
        }
    }
}