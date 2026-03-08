using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementTest
{
    /// <summary>
    /// Test class for QuantityLength and LengthUnit
    /// This class verifies:
    /// 1. Equality of quantities
    /// 2. Unit conversions
    /// 3. Addition operations
    /// 4. Enum conversion factors
    /// 5. Validation checks
    /// </summary>
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

            Assert.IsTrue(inches.Equals(feet));
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
        }

        [TestMethod]
        public void Add_WithTargetUnit()
        {
            var feet = new QuantityLength(1, LengthUnit.FEET);
            var inches = new QuantityLength(12, LengthUnit.INCHES);

            var result = feet.Add(inches, LengthUnit.INCHES);

            Assert.AreEqual(24, result.Value, 0.0001);
            Assert.AreEqual(LengthUnit.INCHES, result.Unit);
        }


        
        //  ENUM CONVERSION FACTOR TESTS
        

        [TestMethod]
        public void LengthUnit_FeetConversionFactor()
        {
            double factor = LengthUnit.FEET.GetConversionFactor();

            Assert.AreEqual(1.0, factor, 0.0001);
        }

        [TestMethod]
        public void LengthUnit_InchesConversionFactor()
        {
            double factor = LengthUnit.INCHES.GetConversionFactor();

            Assert.AreEqual(1.0 / 12.0, factor, 0.0001);
        }

        [TestMethod]
        public void LengthUnit_YardsConversionFactor()
        {
            double factor = LengthUnit.YARDS.GetConversionFactor();

            Assert.AreEqual(3.0, factor, 0.0001);
        }

        [TestMethod]
        public void LengthUnit_CentimetersConversionFactor()
        {
            double factor = LengthUnit.CENTIMETERS.GetConversionFactor();

            Assert.AreEqual(1.0 / 30.48, factor, 0.0001);
        }


        
        //  BASE UNIT CONVERSION TESTS
        

        [TestMethod]
        public void ConvertToBaseUnit_InchesToFeet()
        {
            double result = LengthUnit.INCHES.ConvertToBaseUnit(12);

            Assert.AreEqual(1, result, 0.0001);
        }

        [TestMethod]
        public void ConvertToBaseUnit_YardsToFeet()
        {
            double result = LengthUnit.YARDS.ConvertToBaseUnit(1);

            Assert.AreEqual(3, result, 0.0001);
        }

        [TestMethod]
        public void ConvertFromBaseUnit_FeetToInches()
        {
            double result = LengthUnit.INCHES.ConvertFromBaseUnit(1);

            Assert.AreEqual(12, result, 0.0001);
        }

        [TestMethod]
        public void ConvertFromBaseUnit_FeetToCentimeters()
        {
            double result = LengthUnit.CENTIMETERS.ConvertFromBaseUnit(1);

            Assert.AreEqual(30.48, result, 0.01);
        }


        
        //  VALIDATION TESTS
        

        [TestMethod]
        public void Constructor_Throws_WhenInvalidNumber()
        {
            try
            {
                var q = new QuantityLength(double.NaN, LengthUnit.FEET);

                // If no exception occurs, test should fail
                Assert.Fail("Expected exception not thrown");
            }
            catch (ArgumentException)
            {
                // Test passed because exception was thrown
                Assert.IsTrue(true);
            }
        }

    }
}