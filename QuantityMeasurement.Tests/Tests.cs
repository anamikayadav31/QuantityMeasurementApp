using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Model;

namespace QuantityMeasurementTest
{
    // Indicates this class contains unit tests
    [TestClass]
    public class QuantityMeasurementAppTest
    {

        // FEET TEST CASES


        // Reflexive Property
        [TestMethod]
        public void testEquality_SameReference_Feet()
        {
            Feet feet = new Feet(1.0);
            Assert.IsTrue(feet.Equals(feet));
        }

        // Value-Based Equality
        [TestMethod]
        public void testEquality_SameValue_Feet()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(1.0);

            Assert.IsTrue(feet1.Equals(feet2));
        }

        // Different values
        [TestMethod]
        public void testEquality_DifferentValue_Feet()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(2.0);

            Assert.IsFalse(feet1.Equals(feet2));
        }

        // Null Handling
        [TestMethod]
        public void testEquality_NullComparison_Feet()
        {
            Feet feet = new Feet(1.0);
            Assert.IsFalse(feet.Equals(null));
        }

        // Type Safety
        [TestMethod]
        public void testEquality_TypeMismatch_Feet()
        {
            Feet feet = new Feet(1.0);
            object nonFeetObject = "1.0";

            Assert.IsFalse(feet.Equals(nonFeetObject));
        }

        // Symmetric Property
        [TestMethod]
        public void testEquality_SymmetricProperty_Feet()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(1.0);

            Assert.IsTrue(feet1.Equals(feet2));
            Assert.IsTrue(feet2.Equals(feet1));
        }

        // Transitive Property
        [TestMethod]
        public void testEquality_TransitiveProperty_Feet()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(1.0);
            Feet feet3 = new Feet(1.0);

            Assert.IsTrue(feet1.Equals(feet2));
            Assert.IsTrue(feet2.Equals(feet3));
            Assert.IsTrue(feet1.Equals(feet3));
        }

        // Consistency Property
        [TestMethod]
        public void testEquality_ConsistentResult_Feet()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(1.0);

            Assert.IsTrue(feet1.Equals(feet2));
            Assert.IsTrue(feet1.Equals(feet2));
            Assert.IsTrue(feet1.Equals(feet2));
        }


        // INCHES TEST CASES


        [TestMethod]
        public void testEquality_SameReference_Inches()
        {
            Inches inch = new Inches(12.0);
            Assert.IsTrue(inch.Equals(inch));
        }

        [TestMethod]
        public void testEquality_SameValue_Inches()
        {
            Inches inch1 = new Inches(12.0);
            Inches inch2 = new Inches(12.0);

            Assert.IsTrue(inch1.Equals(inch2));
        }

        [TestMethod]
        public void testEquality_DifferentValue_Inches()
        {
            Inches inch1 = new Inches(12.0);
            Inches inch2 = new Inches(10.0);

            Assert.IsFalse(inch1.Equals(inch2));
        }

        [TestMethod]
        public void testEquality_NullComparison_Inches()
        {
            Inches inch = new Inches(12.0);
            Assert.IsFalse(inch.Equals(null));
        }

        [TestMethod]
        public void testEquality_TypeMismatch_Inches()
        {
            Inches inch = new Inches(12.0);
            object nonInchObject = 12;

            Assert.IsFalse(inch.Equals(nonInchObject));
        }

        [TestMethod]

        public void testEquality_InvalidNumericInput_Inches()
        {
            try
            {
                new Inches(double.NaN);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }
    }
}
