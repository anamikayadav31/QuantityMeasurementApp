using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementTest
{
    /// <summary>
    /// Test class for Quantity<T> using WeightUnit.
    /// 
    /// This class verifies:
    /// 1. Equality of weight quantities
    /// 2. Conversion between different weight units
    /// 3. Addition of quantities with same or different units
    /// 4. Correct conversion factors defined in WeightUnit enum
    /// 
    /// The base unit used internally for weight is KILOGRAM.
    /// </summary>
    [TestClass]
    public class QuantityWeightTest
    {

        // -------------------------------------------------------
        // EQUALITY TESTS
        // -------------------------------------------------------

        /// <summary>
        /// Verify that two quantities with the same value and unit
        /// are equal when converted to the base unit.
        /// Example: 1 kg == 1 kg
        /// </summary>
        [TestMethod]
        public void Equal_WhenSameWeightUnit()
        {
            var w1 = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);
            var w2 = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);

            // Convert both values to base unit (kilograms)
            double base1 = w1.Unit.ConvertToBaseUnit(w1.Value);
            double base2 = w2.Unit.ConvertToBaseUnit(w2.Value);

            // Assert equality
            Assert.AreEqual(base1, base2, 0.0001);
        }

        /// <summary>
        /// Verify equality when two different units represent
        /// the same physical weight.
        /// Example: 1 kg == 1000 grams
        /// </summary>
        [TestMethod]
        public void Equal_WhenKgAndGramAreSame()
        {
            var kg = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);
            var gram = new Quantity<WeightUnit>(1000, WeightUnit.GRAM);

            // Convert both quantities to base unit
            double base1 = kg.Unit.ConvertToBaseUnit(kg.Value);
            double base2 = gram.Unit.ConvertToBaseUnit(gram.Value);

            Assert.AreEqual(base1, base2, 0.0001);
        }

        // -------------------------------------------------------
        // CONVERSION TESTS
        // -------------------------------------------------------

        /// <summary>
        /// Verify conversion from kilograms to grams.
        /// Example: 1 kg = 1000 grams
        /// </summary>
        [TestMethod]
        public void Convert_KgToGram()
        {
            var kg = new Quantity<WeightUnit>(1, WeightUnit.KILOGRAM);

            var result = kg.ConvertTo(
                (u, v) => u.ConvertFromBaseUnit(v),
                (u, v) => u.ConvertToBaseUnit(v),
                WeightUnit.GRAM
            );

            Assert.AreEqual(1000, result.Value, 0.0001);
        }

        /// <summary>
        /// Verify conversion from grams to kilograms.
        /// Example: 2000 grams = 2 kg
        /// </summary>
        [TestMethod]
        public void Convert_GramToKg()
        {
            var gram = new Quantity<WeightUnit>(2000, WeightUnit.GRAM);

            var result = gram.ConvertTo(
                (u, v) => u.ConvertFromBaseUnit(v),
                (u, v) => u.ConvertToBaseUnit(v),
                WeightUnit.KILOGRAM
            );

            Assert.AreEqual(2, result.Value, 0.0001);
        }

        // -------------------------------------------------------
        // ADDITION TESTS
        // -------------------------------------------------------

        /// <summary>
        /// Verify addition of quantities with different units.
        /// Example: 1 kg + 1000 grams = 2 kg
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

        // -------------------------------------------------------
        // ENUM CONVERSION FACTOR TESTS
        // -------------------------------------------------------

        /// <summary>
        /// Verify conversion factor for KILOGRAM.
        /// </summary>
        [TestMethod]
        public void WeightUnit_KgConversionFactor()
        {
            double factor = WeightUnit.KILOGRAM.GetConversionFactor();

            Assert.AreEqual(1.0, factor, 0.0001);
        }

        /// <summary>
        /// Verify conversion factor for GRAM.
        /// </summary>
        [TestMethod]
        public void WeightUnit_GramConversionFactor()
        {
            double factor = WeightUnit.GRAM.GetConversionFactor();

            Assert.AreEqual(0.001, factor, 0.0001);
        }

        /// <summary>
        /// Verify conversion factor for POUND.
        /// </summary>
        [TestMethod]
        public void WeightUnit_PoundConversionFactor()
        {
            double factor = WeightUnit.POUND.GetConversionFactor();

            Assert.AreEqual(0.453592, factor, 0.0001);
        }

        // -------------------------------------------------------
        // BASE UNIT CONVERSION TESTS
        // -------------------------------------------------------

        /// <summary>
        /// Verify converting grams to base unit (kilograms).
        /// Example: 1000 grams = 1 kg
        /// </summary>
        [TestMethod]
        public void ConvertToBaseUnit_GramToKg()
        {
            double result = WeightUnit.GRAM.ConvertToBaseUnit(1000);

            Assert.AreEqual(1, result, 0.0001);
        }

        /// <summary>
        /// Verify converting base unit (kilograms) to grams.
        /// Example: 1 kg = 1000 grams
        /// </summary>
        [TestMethod]
        public void ConvertFromBaseUnit_KgToGram()
        {
            double result = WeightUnit.GRAM.ConvertFromBaseUnit(1);

            Assert.AreEqual(1000, result, 0.0001);
        }
    }
}