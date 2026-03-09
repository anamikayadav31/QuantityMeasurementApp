using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementTest
{
    /// <summary>
    /// Unit tests for QuantityMeasurementService handling TemperatureUnit quantities.
    /// 
    /// Covers:
    /// 1. Equality within the same unit (Celsius to Celsius)
    /// 2. Equality across units (Celsius to Fahrenheit)
    /// 3. Cross-category equality prevention (Temperature vs Length)
    /// 4. Unsupported operations (Addition for temperatures)
    /// </summary>
    [TestClass]
    public class QuantityTemperatureServiceTest
    {
        // Service instance used for all tests
        private QuantityMeasurementService service = new QuantityMeasurementService();

        /// <summary>
        /// Test that two temperatures with the same Celsius value are equal.
        /// </summary>
        [TestMethod]
        public void TemperatureEquality_SameValue_Celsius()
        {
            var t1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(100, TemperatureUnit.CELSIUS);

            Assert.IsTrue(service.AreEqual(t1, t2), "Temperatures with the same Celsius value should be equal");
        }

        /// <summary>
        /// Test that two temperatures with different units but equivalent values are equal.
        /// Example: 0°C = 32°F
        /// </summary>
        [TestMethod]
        public void TemperatureEquality_DifferentUnits_SameValue()
        {
            var t1 = new Quantity<TemperatureUnit>(0, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(32, TemperatureUnit.FAHRENHEIT);

            Assert.IsTrue(service.AreEqual(t1, t2), "Temperatures with equivalent values across units should be equal");
        }

        /// <summary>
        /// Test that comparing temperatures to a different category (length) returns false.
        /// Ensures type safety across categories.
        /// </summary>
        [TestMethod]
        public void TemperatureEquality_DifferentCategories_ReturnsFalse()
        {
            var t1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.CELSIUS);
            var l1 = new Quantity<LengthUnit>(1, LengthUnit.FEET);

            Assert.IsFalse(service.AreEqual(t1, l1), "Temperature should not be equal to a quantity of a different category (Length)");
        }

        /// <summary>
        /// Test that adding two temperatures throws NotSupportedException.
        /// Temperatures do not support arithmetic operations.
        /// </summary>
        [TestMethod]
        public void Temperature_Add_ThrowsNotSupported_Basic()
        {
            var t1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.CELSIUS);
            var t2 = new Quantity<TemperatureUnit>(50, TemperatureUnit.CELSIUS);

            bool exceptionThrown = false;

            try
            {
                service.Add(t1, t2, TemperatureUnit.CELSIUS);
            }
            catch (NotSupportedException)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown, "Expected NotSupportedException when adding temperatures");
        }
    }
}