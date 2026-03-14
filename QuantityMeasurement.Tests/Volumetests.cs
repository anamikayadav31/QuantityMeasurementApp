using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Enums;
using QuantityMeasurementApp.BussinessLayer.Services;


namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// Unit tests for Volume operations.
    /// Covers: compare (UC3), convert (UC6), add (UC9), subtract (UC12).
    /// </summary>
    [TestClass]
    public class VolumeTests
    {
        private QuantityMeasurementServiceImpl _service = null!;

        [TestInitialize]
        public void Setup() => _service = new QuantityMeasurementServiceImpl();

        // ── COMPARE (UC3) ──────────────────────────────────────────────────

        [TestMethod]
        public void Compare_1Litre_And_1Litre_AreEqual()
        {
            var q1 = new QuantityDTO(1, "LITRE", MeasurementType.VOLUME);
            var q2 = new QuantityDTO(1, "LITRE", MeasurementType.VOLUME);
            Assert.IsTrue(_service.Compare(q1, q2));
        }

        [TestMethod]
        public void Compare_1Litre_And_1000Millilitres_AreEqual()
        {
            var litre = new QuantityDTO(1,    "LITRE",      MeasurementType.VOLUME);
            var ml    = new QuantityDTO(1000, "MILLILITRE", MeasurementType.VOLUME);
            Assert.IsTrue(_service.Compare(litre, ml));
        }

        [TestMethod]
        public void Compare_DifferentVolumes_ReturnsFalse()
        {
            var q1 = new QuantityDTO(1, "LITRE", MeasurementType.VOLUME);
            var q2 = new QuantityDTO(2, "LITRE", MeasurementType.VOLUME);
            Assert.IsFalse(_service.Compare(q1, q2));
        }

        // ── CONVERT (UC6) ──────────────────────────────────────────────────

        [TestMethod]
        public void Convert_1Litre_To_1000Millilitres()
        {
            var q      = new QuantityDTO(1, "LITRE", MeasurementType.VOLUME);
            var result = _service.Convert(q, "MILLILITRE");
            Assert.AreEqual(1000.0, result.Value, 0.0001);
        }

        [TestMethod]
        public void Convert_1Gallon_To_Litres()
        {
            var q      = new QuantityDTO(1, "GALLON", MeasurementType.VOLUME);
            var result = _service.Convert(q, "LITRE");
            Assert.AreEqual(3.78541, result.Value, 0.0001);
        }

        // ── ADD (UC9) ──────────────────────────────────────────────────────

        [TestMethod]
        public void Add_1Litre_And_1000Millilitres_Returns2Litres()
        {
            var litre = new QuantityDTO(1,    "LITRE",      MeasurementType.VOLUME);
            var ml    = new QuantityDTO(1000, "MILLILITRE", MeasurementType.VOLUME);
            var result = _service.Add(litre, ml);
            Assert.AreEqual(2.0, result.Value, 0.0001);
            Assert.AreEqual("LITRE", result.Unit);
        }

        [TestMethod]
        public void Add_1Litre_And_1Gallon()
        {
            var litre  = new QuantityDTO(1, "LITRE",  MeasurementType.VOLUME);
            var gallon = new QuantityDTO(1, "GALLON", MeasurementType.VOLUME);
            var result = _service.Add(litre, gallon);
            Assert.AreEqual(4.78541, result.Value, 0.0001);
            Assert.AreEqual("LITRE", result.Unit);
        }

        // ── SUBTRACT (UC12) ────────────────────────────────────────────────

        [TestMethod]
        public void Subtract_2Litres_Minus_1000Millilitres_Returns1Litre()
        {
            var litre = new QuantityDTO(2,    "LITRE",      MeasurementType.VOLUME);
            var ml    = new QuantityDTO(1000, "MILLILITRE", MeasurementType.VOLUME);
            var result = _service.Subtract(litre, ml);
            Assert.AreEqual(1.0, result.Value, 0.0001);
        }

        // ── DIVIDE ─────────────────────────────────────────────────────────

        [TestMethod]
        public void Divide_2Litres_By_1Litre_Returns2()
        {
            var q1 = new QuantityDTO(2, "LITRE", MeasurementType.VOLUME);
            var q2 = new QuantityDTO(1, "LITRE", MeasurementType.VOLUME);
            Assert.AreEqual(2.0, _service.Divide(q1, q2), 0.0001);
        }
    }
}