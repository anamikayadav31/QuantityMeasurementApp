using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementTest
{
    /// <summary>
    /// Test class for checking Weight operations
    /// </summary>
    [TestClass]
    public class QuantityWeightTest
    {

        /// <summary>
        /// Test if two values in kilograms are equal
        /// </summary>
        [TestMethod]
        public void Equal_Kg_To_Kg()
        {
            // create two weight objects
            var w1 = new QuantityWeight(1, WeightUnit.KILOGRAM);
            var w2 = new QuantityWeight(1, WeightUnit.KILOGRAM);

            // check if both are equal
            Assert.IsTrue(w1.Equals(w2));
        }

        /// <summary>
        /// Test if 1 kilogram equals 1000 grams
        /// </summary>
        [TestMethod]
        public void Equal_Kg_To_Gram()
        {
            // create weight values
            var kg = new QuantityWeight(1, WeightUnit.KILOGRAM);
            var gram = new QuantityWeight(1000, WeightUnit.GRAM);

            // check equality
            Assert.IsTrue(kg.Equals(gram));
        }

        /// <summary>
        /// Test conversion from kilogram to gram
        /// </summary>
        [TestMethod]
        public void Convert_Kg_To_Gram()
        {
            // create weight
            var kg = new QuantityWeight(1, WeightUnit.KILOGRAM);

            // convert to grams
            var result = kg.ConvertTo(WeightUnit.GRAM);

            // check result value
            Assert.AreEqual(1000, result.Value, 0.001);
        }

        /// <summary>
        /// Test addition of kilogram and gram
        /// </summary>
        [TestMethod]
        public void Add_Kg_And_Gram()
        {
            // create weights
            var kg = new QuantityWeight(1, WeightUnit.KILOGRAM);
            var gram = new QuantityWeight(1000, WeightUnit.GRAM);

            // add both weights
            var result = kg.Add(gram);

            // check if result is 2 kg
            Assert.AreEqual(2, result.Value, 0.001);
        }
    }
}