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
        public void Setup() => _service = new QuantityMeasurementServiceImpl();

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
        public void Compare_DifferentLengths_ReturnsFalse()
        {
            var q1 = new QuantityDTO(1, "FEET", MeasurementType.LENGTH);
            var q2 = new QuantityDTO(2, "FEET", MeasurementType.LENGTH);
            Assert.IsFalse(_service.Compare(q1, q2));
        }

        [TestMethod]
        public void Convert_1Foot_To_12Inches()
        {
            var q = new QuantityDTO(1, "FEET", MeasurementType.LENGTH);
            var result = _service.Convert(q, "INCHES");
            Assert.AreEqual(12.0, result.Value, 0.0001);
        }

        [TestMethod]
        public void Convert_1Yard_To_3Feet()
        {
            var q = new QuantityDTO(1, "YARDS", MeasurementType.LENGTH);
            var result = _service.Convert(q, "FEET");
            Assert.AreEqual(3.0, result.Value, 0.0001);
        }

        [TestMethod]
        public void Add_1Foot_And_12Inches_Returns2Feet()
        {
            var feet = new QuantityDTO(1, "FEET", MeasurementType.LENGTH);
            var inches = new QuantityDTO(12, "INCHES", MeasurementType.LENGTH);
            var result = _service.Add(feet, inches);
            Assert.AreEqual(2.0, result.Value, 0.0001);
            Assert.AreEqual("FEET", result.Unit);
        }

        [TestMethod]
        public void Subtract_2Feet_Minus_12Inches_Returns1Foot()
        {
            var feet = new QuantityDTO(2, "FEET", MeasurementType.LENGTH);
            var inches = new QuantityDTO(12, "INCHES", MeasurementType.LENGTH);
            var result = _service.Subtract(feet, inches);
            Assert.AreEqual(1.0, result.Value, 0.0001);
        }

        [TestMethod]
        public void Divide_2Feet_By_1Foot_Returns2()
        {
            var q1 = new QuantityDTO(2, "FEET", MeasurementType.LENGTH);
            var q2 = new QuantityDTO(1, "FEET", MeasurementType.LENGTH);
            Assert.AreEqual(2.0, _service.Divide(q1, q2), 0.0001);
        }

        // Fixed: ExpectedExceptionAttribute does not exist in MSTest
        // Use Assert.ThrowsException instead
        [TestMethod]
        public void Divide_ByZero_ShouldThrowException()
        {
            var q1 = new QuantityDTO(1, "FEET", MeasurementType.LENGTH);
            var q2 = new QuantityDTO(0, "INCHES", MeasurementType.LENGTH);

            try
            {
                _service.Divide(q1, q2);
                Assert.Fail(); // fail if no exception
            }
            catch (DivideByZeroException)
            {
                Assert.IsTrue(true); // pass if exception occurs
            }
        }
    }
}