using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementTest
{
    [TestClass]
    public class QuantityLengthTest
    {
        // Test: Same feet value should be equal
        [TestMethod]
        public void testEquality_FeetToFeet_SameValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        // Test: Same inch value should be equal
        [TestMethod]
        public void testEquality_InchToInch_SameValue()
        {
            var q1 = new QuantityLength(12.0, LengthUnit.INCH);
            var q2 = new QuantityLength(12.0, LengthUnit.INCH);

            Assert.IsTrue(q1.Equals(q2));
        }

        // Test: 12 inches should equal 1 foot
        [TestMethod]
        public void testEquality_InchToFeet_EquivalentValue()
        {
            var q1 = new QuantityLength(12.0, LengthUnit.INCH);
            var q2 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q2));
        }

        // Test: Different feet values should NOT be equal
        [TestMethod]
        public void testEquality_FeetToFeet_DifferentValue()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);
            var q2 = new QuantityLength(2.0, LengthUnit.FEET);

            Assert.IsFalse(q1.Equals(q2));
        }

        // Test: Object should equal itself (Reflexive property)
        [TestMethod]
        public void testEquality_SameReference()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsTrue(q1.Equals(q1));
        }

        // Test: Object compared with null should return false
        [TestMethod]
        public void testEquality_NullComparison()
        {
            var q1 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsFalse(q1.Equals(null));
        }

        // Test: Invalid numeric value (NaN) should throw exception
        // Test: NaN value should throw ArgumentException

        [TestMethod]
        public void testEquality_InvalidNumeric()
        {
            try
            {
                new QuantityLength(double.NaN, LengthUnit.FEET);
                Assert.Fail("Expected exception not thrown");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }
    }
}