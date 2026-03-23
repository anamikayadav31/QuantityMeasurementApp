using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.BussinessLayer.Services;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Enums;

namespace QuantityMeasurementApp.Tests
{
    // ── UC9: Weight Measurement (Compare, Convert, Add, Subtract, Divide) ─
    [TestClass]
    public class WeightTests
    {
        private QuantityMeasurementServiceImpl _service = null!;

        [TestInitialize]
        public void Setup() => _service = new QuantityMeasurementServiceImpl();

        // ── COMPARE ──────────────────────────────────────────────────────

        [TestMethod]
        public void Compare_1Kg_And_1Kg_AreEqual()
        {
            var q1 = new QuantityDTO(1, "KILOGRAM", MeasurementType.WEIGHT);
            var q2 = new QuantityDTO(1, "KILOGRAM", MeasurementType.WEIGHT);
            Assert.IsTrue(_service.Compare(q1, q2));
        }

        [TestMethod]
        public void Compare_1Kg_And_1000Grams_AreEqual()
        {
            var kg   = new QuantityDTO(1,    "KILOGRAM", MeasurementType.WEIGHT);
            var gram = new QuantityDTO(1000, "GRAM",     MeasurementType.WEIGHT);
            Assert.IsTrue(_service.Compare(kg, gram));
        }

        [TestMethod]
        public void Compare_DifferentWeights_ReturnsFalse()
        {
            var q1 = new QuantityDTO(1, "KILOGRAM", MeasurementType.WEIGHT);
            var q2 = new QuantityDTO(2, "KILOGRAM", MeasurementType.WEIGHT);
            Assert.IsFalse(_service.Compare(q1, q2));
        }

        // ── CONVERT ──────────────────────────────────────────────────────

        [TestMethod]
        public void Convert_1Kg_To_1000Grams()
        {
            var q      = new QuantityDTO(1, "KILOGRAM", MeasurementType.WEIGHT);
            var result = _service.Convert(q, "GRAM");
            Assert.AreEqual(1000.0, result.Value, 0.0001);
        }

        [TestMethod]
        public void Convert_500Grams_To_HalfKg()
        {
            var q      = new QuantityDTO(500, "GRAM", MeasurementType.WEIGHT);
            var result = _service.Convert(q, "KILOGRAM");
            Assert.AreEqual(0.5, result.Value, 0.0001);
        }

        // ── ADD ───────────────────────────────────────────────────────────

        [TestMethod]
        public void Add_1Kg_And_1000Grams_Returns2Kg()
        {
            var kg   = new QuantityDTO(1,    "KILOGRAM", MeasurementType.WEIGHT);
            var gram = new QuantityDTO(1000, "GRAM",     MeasurementType.WEIGHT);
            var result = _service.Add(kg, gram);
            Assert.AreEqual(2.0,       result.Value, 0.0001);
            Assert.AreEqual("KILOGRAM", result.Unit);
        }

        // ── SUBTRACT ─────────────────────────────────────────────────────

        [TestMethod]
        public void Subtract_2Kg_Minus_1000Grams_Returns1Kg()
        {
            var kg   = new QuantityDTO(2,    "KILOGRAM", MeasurementType.WEIGHT);
            var gram = new QuantityDTO(1000, "GRAM",     MeasurementType.WEIGHT);
            var result = _service.Subtract(kg, gram);
            Assert.AreEqual(1.0, result.Value, 0.0001);
        }

        // ── DIVIDE ───────────────────────────────────────────────────────

        [TestMethod]
        public void Divide_2Kg_By_1000Grams_Returns2()
        {
            var kg   = new QuantityDTO(2,    "KILOGRAM", MeasurementType.WEIGHT);
            var gram = new QuantityDTO(1000, "GRAM",     MeasurementType.WEIGHT);
            Assert.AreEqual(2.0, _service.Divide(kg, gram), 0.0001);
        }
    }
}