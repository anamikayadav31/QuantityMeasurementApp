using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Enums;
using QuantityMeasurementApp.BussinessLayer.Services;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class LengthTests
    {
        private QuantityMeasurementServiceImpl _service = null!;

        [TestInitialize]
        public void Setup()
        {
            _service = new QuantityMeasurementServiceImpl();
        }

        [TestMethod]
        public void Compare_SameValueSameUnit_ReturnsTrue()
        {
            var q1 = new QuantityDTO(1, "FEET", MeasurementType.LENGTH);
            var q2 = new QuantityDTO(1, "FEET", MeasurementType.LENGTH);
            Assert.IsTrue(_service.Compare(q1, q2));
        }

        [TestMethod]
        public void Compare_12Inches_And_1Foot_AreEqual()
        {
            var inches = new QuantityDTO(12, "INCHES", MeasurementType.LENGTH);
            var feet = new QuantityDTO(1, "FEET", MeasurementType.LENGTH);

            Assert.IsTrue(_service.Compare(inches, feet));
        }

        [TestMethod]
        public void Compare_1Yard_And_3Feet_AreEqual()
        {
            var yard = new QuantityDTO(1, "YARDS", MeasurementType.LENGTH);
            var feet = new QuantityDTO(3, "FEET", MeasurementType.LENGTH);

            Assert.IsTrue(_service.Compare(yard, feet));
        }

        [TestMethod]
        public void Add_1Foot_And_12Inches_Returns2Feet()
        {
            var feet = new QuantityDTO(1, "FEET", MeasurementType.LENGTH);
            var inches = new QuantityDTO(12, "INCHES", MeasurementType.LENGTH);

            var result = _service.Add(feet, inches);

            Assert.AreEqual(2.0, result.Value, 0.0001);
        }

        [TestMethod]
        public void Subtract_2Feet_Minus_12Inches_Returns1Foot()
        {
            var feet = new QuantityDTO(2, "FEET", MeasurementType.LENGTH);
            var inches = new QuantityDTO(12, "INCHES", MeasurementType.LENGTH);

            var result = _service.Subtract(feet, inches);

            Assert.AreEqual(1.0, result.Value, 0.0001);
        }
    }
}