using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementTest
{
    /// <summary>
    /// Unit tests for WeightUnit operations in QuantityMeasurementService.
    /// 
    /// Covers:
    /// 1. Equality checks for same values (kg vs kg, kg vs g)
    /// 2. Addition of weights across units (kg + g)
    /// 3. Division of weights across units (kg ÷ g)
    /// </summary>
    [TestClass]
    public class QuantityWeightTest
    {
        // Service instance used for all tests
        private QuantityMeasurementService service = new QuantityMeasurementService();

        /// <summary>
        /// Test equality for same value in kilograms
        /// </summary>
        [TestMethod]
        public void Equal_WhenSameWeightUnit()
        {
            var w1 = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);

            double base1 = w1.Unit.ConvertToBaseUnit(w1.Value);
            double base2 = w2.Unit.ConvertToBaseUnit(w2.Value);

            Assert.AreEqual(base1, base2, 0.0001);
        }

        /// <summary>
        /// Test equality between kilograms and grams (1 kg = 1000 g)
        /// </summary>
        [TestMethod]
        public void Equal_WhenKgAndGramAreSame()
        {
            var kg = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);
            var gram = new Quantity<WeightUnit>(1000, WeightUnit.GRAM);

            double base1 = kg.Unit.ConvertToBaseUnit(kg.Value);
            double base2 = gram.Unit.ConvertToBaseUnit(gram.Value);

            Assert.AreEqual(base1, base2, 0.0001);
        }

        /// <summary>
        /// Test addition of kilograms and grams
        /// </summary>
        [TestMethod]
        public void Add_KgAndGram()
        {
            var kg = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);
            var gram = new Quantity<WeightUnit>(1000, WeightUnit.GRAM);

            var result = kg.Add(
                gram,
                (u, v) => u.ConvertToBaseUnit(v),
                (u, v) => u.ConvertFromBaseUnit(v)
            );

            Assert.AreEqual(2, result.Value, 0.0001);
        }

        /// <summary>
        /// Test division of kilograms by grams
        /// </summary>
        [TestMethod]
        public void Divide_KgByGram()
        {
            var kg = new Quantity<WeightUnit>(2, WeightUnit.KILOGRAM);
            var gram = new Quantity<WeightUnit>(1000, WeightUnit.GRAM);

            double result = kg.Divide(
                gram,
                (u, v) => u.ConvertToBaseUnit(v)
            );

            Assert.AreEqual(2, result, 0.0001);
        }
    }
}