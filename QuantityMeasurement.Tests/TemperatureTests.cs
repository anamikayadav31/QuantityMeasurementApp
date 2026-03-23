using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.BussinessLayer.Services;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Enums;
using QuantityMeasurementApp.ModelLayer.Exceptions;

namespace QuantityMeasurementApp.Tests
{
    // ── UC14: Temperature – Selective Arithmetic Support ──────────────────
    // Temperature only allows Compare and Convert.
    // Add / Subtract / Divide must throw QuantityMeasurementException.
    [TestClass]
    public class TemperatureTests
    {
        private QuantityMeasurementServiceImpl _service = null!;

        [TestInitialize]
        public void Setup() => _service = new QuantityMeasurementServiceImpl();

        // ── COMPARE ──────────────────────────────────────────────────────

        [TestMethod]
        public void Compare_0Celsius_And_32Fahrenheit_AreEqual()
        {
            var c = new QuantityDTO(0,  "CELSIUS",    MeasurementType.TEMPERATURE);
            var f = new QuantityDTO(32, "FAHRENHEIT", MeasurementType.TEMPERATURE);
            Assert.IsTrue(_service.Compare(c, f));
        }

        [TestMethod]
        public void Compare_100Celsius_And_212Fahrenheit_AreEqual()
        {
            var c = new QuantityDTO(100, "CELSIUS",    MeasurementType.TEMPERATURE);
            var f = new QuantityDTO(212, "FAHRENHEIT", MeasurementType.TEMPERATURE);
            Assert.IsTrue(_service.Compare(c, f));
        }

        [TestMethod]
        public void Compare_0Celsius_And_273point15Kelvin_AreEqual()
        {
            var c = new QuantityDTO(0,      "CELSIUS", MeasurementType.TEMPERATURE);
            var k = new QuantityDTO(273.15, "KELVIN",  MeasurementType.TEMPERATURE);
            Assert.IsTrue(_service.Compare(c, k));
        }

        // ── CONVERT ──────────────────────────────────────────────────────

        [TestMethod]
        public void Convert_0Celsius_To_32Fahrenheit()
        {
            var q      = new QuantityDTO(0, "CELSIUS", MeasurementType.TEMPERATURE);
            var result = _service.Convert(q, "FAHRENHEIT");
            Assert.AreEqual(32.0, result.Value, 0.0001);
        }

        [TestMethod]
        public void Convert_0Celsius_To_273point15Kelvin()
        {
            var q      = new QuantityDTO(0, "CELSIUS", MeasurementType.TEMPERATURE);
            var result = _service.Convert(q, "KELVIN");
            Assert.AreEqual(273.15, result.Value, 0.0001);
        }

        [TestMethod]
        public void Convert_32Fahrenheit_To_0Celsius()
        {
            var q      = new QuantityDTO(32, "FAHRENHEIT", MeasurementType.TEMPERATURE);
            var result = _service.Convert(q, "CELSIUS");
            Assert.AreEqual(0.0, result.Value, 0.0001);
        }

        // ── BLOCKED: Add / Subtract / Divide must throw ───────────────────

        [TestMethod]
        public void Add_Temperature_ThrowsException()
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
        public void Subtract_Temperature_ThrowsException()
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
        public void Divide_Temperature_ThrowsException()
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
    }
}