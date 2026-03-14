using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Enums;
using QuantityMeasurementApp.ModelLayer.Exceptions;
using QuantityMeasurementApp.BussinessLayer.Services;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class Architecuretests
    {
        private QuantityMeasurementServiceImpl _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _service = new QuantityMeasurementServiceImpl();
        }

        // ── LENGTH TESTS ───────────────────────────────────────────────────
        [TestMethod]
        public void Add_Length_ValidOperation()
        {
            var q1 = new QuantityDTO(1, "FEET", MeasurementType.LENGTH);
            var q2 = new QuantityDTO(12, "INCHES", MeasurementType.LENGTH);

            var result = _service.Add(q1, q2);
            Assert.AreEqual(2, result.Value, 0.0001); // 1 ft + 12 in = 2 ft
        }

        [TestMethod]
        public void Subtract_Length_ValidOperation()
        {
            var q1 = new QuantityDTO(2, "FEET", MeasurementType.LENGTH);
            var q2 = new QuantityDTO(12, "INCHES", MeasurementType.LENGTH);

            var result = _service.Subtract(q1, q2);
            Assert.AreEqual(1, result.Value, 0.0001); // 2 ft - 12 in = 1 ft
        }

        // ── TEMPERATURE TESTS ─────────────────────────────────────────────
        [TestMethod]
        public void Add_Temperature_ShouldThrowException()
        {
            var q1 = new QuantityDTO(10, "CELSIUS", MeasurementType.TEMPERATURE);
            var q2 = new QuantityDTO(20, "CELSIUS", MeasurementType.TEMPERATURE);

            try
            {
                _service.Add(q1, q2);
                Assert.Fail("Expected QuantityMeasurementException was not thrown.");
            }
            catch (QuantityMeasurementException ex)
            {
                StringAssert.Contains(ex.Message, "Temperature does not support arithmetic");
            }
        }

        [TestMethod]
        public void Subtract_Temperature_ShouldThrowException()
        {
            var q1 = new QuantityDTO(50, "CELSIUS", MeasurementType.TEMPERATURE);
            var q2 = new QuantityDTO(30, "CELSIUS", MeasurementType.TEMPERATURE);

            try
            {
                _service.Subtract(q1, q2);
                Assert.Fail("Expected QuantityMeasurementException was not thrown.");
            }
            catch (QuantityMeasurementException ex)
            {
                StringAssert.Contains(ex.Message, "Temperature does not support arithmetic");
            }
        }

        [TestMethod]
        public void Divide_Temperature_ShouldThrowException()
        {
            var q1 = new QuantityDTO(50, "CELSIUS", MeasurementType.TEMPERATURE);
            var q2 = new QuantityDTO(10, "CELSIUS", MeasurementType.TEMPERATURE);

            try
            {
                _service.Divide(q1, q2);
                Assert.Fail("Expected QuantityMeasurementException was not thrown.");
            }
            catch (QuantityMeasurementException ex)
            {
                StringAssert.Contains(ex.Message, "Temperature does not support arithmetic");
            }
        }

        // ── WEIGHT TESTS ─────────────────────────────────────────────────
        [TestMethod]
        public void Add_Weight_ValidOperation()
        {
            var q1 = new QuantityDTO(1, "KILOGRAM", MeasurementType.WEIGHT);
            var q2 = new QuantityDTO(500, "GRAM", MeasurementType.WEIGHT);

            var result = _service.Add(q1, q2);
            Assert.AreEqual(1.5, result.Value, 0.0001);
        }

        [TestMethod]
        public void Subtract_Weight_ValidOperation()
        {
            var q1 = new QuantityDTO(2, "KILOGRAM", MeasurementType.WEIGHT);
            var q2 = new QuantityDTO(500, "GRAM", MeasurementType.WEIGHT);

            var result = _service.Subtract(q1, q2);
            Assert.AreEqual(1.5, result.Value, 0.0001);
        }

        // ── VOLUME TESTS ─────────────────────────────────────────────────
        [TestMethod]
        public void Convert_Volume_ValidOperation()
        {
            var q = new QuantityDTO(1, "LITRE", MeasurementType.VOLUME);
            var result = _service.Convert(q, "MILLILITRE");

            Assert.AreEqual(1000, result.Value, 0.0001);
        }
    }
}