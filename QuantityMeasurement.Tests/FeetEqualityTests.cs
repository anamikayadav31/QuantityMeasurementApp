using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Model;

namespace QuantityMeasurementTest
{
    // Indicates this class contains unit tests
    [TestClass]
    public class QuantityMeasurementAppTest
    {
        
        // Reflexive Property
        // a.equals(a) must return true

        [TestMethod]
        public void testEquality_SameReference()
        {
            Feet feet = new Feet(1.0);

            // Object must be equal to itself
            Assert.IsTrue(feet.Equals(feet));
        }

        
        // Value-Based Equality
        // Same values must be equal
        
        [TestMethod]
        public void testEquality_SameValue()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(1.0);

            // Two different objects with same value should be equal
            Assert.IsTrue(feet1.Equals(feet2));
        }

        
        // Different values must not be equal

        [TestMethod]
        public void testEquality_DifferentValue()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(2.0);

            // Objects with different values should not be equal
            Assert.IsFalse(feet1.Equals(feet2));
        }

        
        // Null Handling
        // a.equals(null) must return false
        
        [TestMethod]
        public void testEquality_NullComparison()
        {
            Feet feet = new Feet(1.0);

            // Comparing with null should return false
            Assert.IsFalse(feet.Equals(null));
        }

        
        // Type Safety
        // Object should not be equal to different type
        
        [TestMethod]
        public void testEquality_NonNumericInput()
        {
            Feet feet = new Feet(1.0);
            object nonFeetObject = "1.0";

            // Comparing Feet object with string should return false
            Assert.IsFalse(feet.Equals(nonFeetObject));
        }

        
        // Symmetric Property
        // if a.equals(b) then b.equals(a)
        
        [TestMethod]
        public void testEquality_SymmetricProperty()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(1.0);

            Assert.IsTrue(feet1.Equals(feet2));
            Assert.IsTrue(feet2.Equals(feet1));
        }

        
        // Transitive Property
        // if a=b and b=c then a=c
        
        [TestMethod]
        public void testEquality_TransitiveProperty()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(1.0);
            Feet feet3 = new Feet(1.0);

            Assert.IsTrue(feet1.Equals(feet2));
            Assert.IsTrue(feet2.Equals(feet3));
            Assert.IsTrue(feet1.Equals(feet3));
        }

        
        // Consistency Property
        // Multiple calls return same result
        
        [TestMethod]
        public void testEquality_ConsistentResult()
        {
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(1.0);

            Assert.IsTrue(feet1.Equals(feet2));
            Assert.IsTrue(feet1.Equals(feet2));
            Assert.IsTrue(feet1.Equals(feet2));

        }
    }
}