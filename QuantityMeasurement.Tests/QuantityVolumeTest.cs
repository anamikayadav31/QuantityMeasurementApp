using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementTest
{
    /// <summary>
    /// Test class for Quantity<T> using VolumeUnit.
    /// 
    /// This class verifies:
    /// 1. Equality of volume quantities
    /// 2. Conversion between different volume units
    /// 3. Addition of quantities with same or different units
    /// 4. Correct conversion factors defined in VolumeUnit enum
    /// 
    /// The base unit used internally for volume is LITRE.
    /// </summary>
    [TestClass]
    public class QuantityVolumeTest
    {

        // -------------------------------------------------------
        // EQUALITY TESTS
        // -------------------------------------------------------

        /// <summary>
        /// Verify that two quantities with the same value and unit
        /// are equal after conversion to the base unit.
        /// Example: 1 litre == 1 litre
        /// </summary>
        [TestMethod]
        public void Equal_WhenSameLitreValue()
        {
            var v1 = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);

            // Convert both values to base unit (litre)
            double base1 = v1.Unit.ConvertToBaseUnit(v1.Value);
            double base2 = v2.Unit.ConvertToBaseUnit(v2.Value);

            Assert.AreEqual(base1, base2, 0.0001);
        }

        /// <summary>
        /// Verify equality when different units represent
        /// the same volume.
        /// Example: 1 litre == 1000 millilitres
        /// </summary>
        [TestMethod]
        public void Equal_WhenLitreAndMillilitre()
        {
            var litre = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);
            var ml = new Quantity<VolumeUnit>(1000, VolumeUnit.MILLILITRE);

            double base1 = litre.Unit.ConvertToBaseUnit(litre.Value);
            double base2 = ml.Unit.ConvertToBaseUnit(ml.Value);

            Assert.AreEqual(base1, base2, 0.0001);
        }

        // -------------------------------------------------------
        // CONVERSION TESTS
        // -------------------------------------------------------

        /// <summary>
        /// Verify conversion from litre to millilitre.
        /// Example: 1 litre = 1000 millilitres
        /// </summary>
        [TestMethod]
        public void Convert_LitreToMillilitre()
        {
            var litre = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);

            var result = litre.ConvertTo(
                (u, v) => u.ConvertFromBaseUnit(v),
                (u, v) => u.ConvertToBaseUnit(v),
                VolumeUnit.MILLILITRE
            );

            Assert.AreEqual(1000, result.Value, 0.0001);
        }

        /// <summary>
        /// Verify conversion from gallon to litre.
        /// Example: 1 gallon = 3.78541 litres
        /// </summary>
        [TestMethod]
        public void Convert_GallonToLitre()
        {
            var gallon = new Quantity<VolumeUnit>(1, VolumeUnit.GALLON);

            var result = gallon.ConvertTo(
                (u, v) => u.ConvertFromBaseUnit(v),
                (u, v) => u.ConvertToBaseUnit(v),
                VolumeUnit.LITRE
            );

            Assert.AreEqual(3.78541, result.Value, 0.0001);
        }

        // -------------------------------------------------------
        // ADDITION TESTS
        // -------------------------------------------------------

        /// <summary>
        /// Verify addition of litre and millilitre.
        /// Example: 1 litre + 1000 millilitres = 2 litres
        /// </summary>
        [TestMethod]
        public void Add_LitreAndMillilitre()
        {
            var litre = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);
            var ml = new Quantity<VolumeUnit>(1000, VolumeUnit.MILLILITRE);

            var result = litre.Add(
                ml,
                (u, v) => u.ConvertToBaseUnit(v),
                (u, v) => u.ConvertFromBaseUnit(v)
            );

            Assert.AreEqual(2, result.Value, 0.0001);
        }

        /// <summary>
        /// Verify addition of litre and gallon.
        /// Example: 1 litre + 1 gallon = 4.78541 litres
        /// </summary>
        [TestMethod]
        public void Add_LitreAndGallon()
        {
            var litre = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);
            var gallon = new Quantity<VolumeUnit>(1, VolumeUnit.GALLON);

            var result = litre.Add(
                gallon,
                (u, v) => u.ConvertToBaseUnit(v),
                (u, v) => u.ConvertFromBaseUnit(v)
            );

            Assert.AreEqual(4.78541, result.Value, 0.0001);
        }

        // -------------------------------------------------------
        // ENUM CONVERSION FACTOR TESTS
        // -------------------------------------------------------

        /// <summary>
        /// Verify conversion factor for LITRE unit.
        /// </summary>
        [TestMethod]
        public void VolumeUnit_LitreFactor()
        {
            double factor = VolumeUnit.LITRE.GetConversionFactor();

            Assert.AreEqual(1.0, factor, 0.0001);
        }

        /// <summary>
        /// Verify conversion factor for MILLILITRE unit.
        /// </summary>
        [TestMethod]
        public void VolumeUnit_MillilitreFactor()
        {
            double factor = VolumeUnit.MILLILITRE.GetConversionFactor();

            Assert.AreEqual(0.001, factor, 0.0001);
        }

        /// <summary>
        /// Verify conversion factor for GALLON unit.
        /// </summary>
        [TestMethod]
        public void VolumeUnit_GallonFactor()
        {
            double factor = VolumeUnit.GALLON.GetConversionFactor();

            Assert.AreEqual(3.78541, factor, 0.0001);
        }
    }
}