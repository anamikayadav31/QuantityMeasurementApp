using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementTest
{
    /// <summary>
    /// Unit tests for VolumeUnit operations in QuantityMeasurementService.
    /// 
    /// Covers:
    /// 1. Equality checks for same values (litres vs litres, litres vs millilitres)
    /// 2. Addition of volumes across units (litres + millilitres, litres + gallons)
    /// </summary>
    [TestClass]
    public class QuantityVolumeTest
    {
        // Service instance used for all tests
        private QuantityMeasurementService service = new QuantityMeasurementService();

        /// <summary>
        /// Test equality for same value in litres
        /// </summary>
        [TestMethod]
        public void Equal_WhenSameLitreValue()
        {
            var v1 = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);
            var v2 = new Quantity<VolumeUnit>(1, VolumeUnit.LITRE);

            double base1 = v1.Unit.ConvertToBaseUnit(v1.Value);
            double base2 = v2.Unit.ConvertToBaseUnit(v2.Value);

            Assert.AreEqual(base1, base2, 0.0001);
        }

        /// <summary>
        /// Test equality between litres and millilitres (1 L = 1000 mL)
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

        /// <summary>
        /// Test addition of litres and millilitres
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
        /// Test addition of litres and gallons (1 gallon ≈ 3.78541 L)
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
    }
}