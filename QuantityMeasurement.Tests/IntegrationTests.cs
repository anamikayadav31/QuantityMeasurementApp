using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.ModelLayer.Enums;
using QuantityMeasurementApp.ModelLayer.Exceptions;
using QuantityMeasurementApp.BussinessLayer.Services;
using QuantityMeasurementApp.RepoLayer.Context;
using QuantityMeasurementApp.RepoLayer.Implementations;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.Tests
{
    // ── UC16: Full-stack Integration Tests ───────────────────────────────
    // Service (business logic) + EF Core Repository (persistence)
    // Uses EF Core InMemoryDatabase — no SQL Server needed for tests.
    [TestClass]
    public class IntegrationTests
    {
        private QuantityMeasurementServiceImpl _service = null!;
        private IQuantityMeasurementRepository _repo    = null!;
        private AppDbContext _db = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"Integration_{Guid.NewGuid()}")
                .Options;
            _db      = new AppDbContext(options);
            _repo    = new QuantityMeasurementRepository(_db);
            _service = new QuantityMeasurementServiceImpl();
        }

        [TestCleanup]
        public void Cleanup() => _db.Dispose();

        [TestMethod]
        public async Task Integration_CompareLength_ResultPersistedToDatabase()
        {
            var q1    = new QuantityDTO(1,  "FEET",   MeasurementType.LENGTH);
            var q2    = new QuantityDTO(12, "INCHES", MeasurementType.LENGTH);
            bool equal = _service.Compare(q1, q2);
            Assert.IsTrue(equal);

            await _repo.SaveAsync(new QuantityMeasurementEntity(
                "COMPARE", q1.ToString(), q2.ToString(), equal.ToString()));

            Assert.AreEqual(1, await _repo.GetTotalCountAsync());
            Assert.AreEqual("True", (await _repo.GetAllAsync())[0].Result);
        }

        [TestMethod]
        public async Task Integration_CompareTemperature_CelsiusAndFahrenheit_Equal()
        {
            var celsius    = new QuantityDTO(0,  "CELSIUS",    MeasurementType.TEMPERATURE);
            var fahrenheit = new QuantityDTO(32, "FAHRENHEIT", MeasurementType.TEMPERATURE);
            bool equal = _service.Compare(celsius, fahrenheit);
            Assert.IsTrue(equal);

            await _repo.SaveAsync(new QuantityMeasurementEntity(
                "COMPARE", celsius.ToString(), fahrenheit.ToString(), equal.ToString()));

            Assert.AreEqual(1, (await _repo.GetByOperationAsync("COMPARE")).Count);
        }

        [TestMethod]
        public async Task Integration_ConvertVolume_LitreToMillilitre()
        {
            var q      = new QuantityDTO(1, "LITRE", MeasurementType.VOLUME);
            var result = _service.Convert(q, "MILLILITRE");
            Assert.AreEqual(1000.0, result.Value, 0.0001);

            await _repo.SaveAsync(new QuantityMeasurementEntity(
                "CONVERT", q.ToString(), "MILLILITRE", result.ToString()));

            Assert.AreEqual(1, await _repo.GetTotalCountAsync());
        }

        [TestMethod]
        public async Task Integration_ConvertTemperature_CelsiusToFahrenheit()
        {
            var q      = new QuantityDTO(0, "CELSIUS", MeasurementType.TEMPERATURE);
            var result = _service.Convert(q, "FAHRENHEIT");
            Assert.AreEqual(32.0, result.Value, 0.0001);

            await _repo.SaveAsync(new QuantityMeasurementEntity(
                "CONVERT", q.ToString(), "FAHRENHEIT", result.ToString()));

            Assert.AreEqual(1, (await _repo.GetByOperationAsync("CONVERT")).Count);
        }

        [TestMethod]
        public async Task Integration_AddWeight_ResultPersistedToDatabase()
        {
            var q1     = new QuantityDTO(1,   "KILOGRAM", MeasurementType.WEIGHT);
            var q2     = new QuantityDTO(500, "GRAM",     MeasurementType.WEIGHT);
            var result = _service.Add(q1, q2);
            Assert.AreEqual(1.5, result.Value, 0.0001);

            await _repo.SaveAsync(new QuantityMeasurementEntity(
                "ADD", q1.ToString(), q2.ToString(), result.ToString()));

            Assert.AreEqual(1, (await _repo.GetByOperationAsync("ADD")).Count);
        }

        [TestMethod]
        public async Task Integration_SubtractLength_ResultPersistedToDatabase()
        {
            var q1     = new QuantityDTO(2,  "FEET",   MeasurementType.LENGTH);
            var q2     = new QuantityDTO(12, "INCHES", MeasurementType.LENGTH);
            var result = _service.Subtract(q1, q2);
            Assert.AreEqual(1.0, result.Value, 0.0001);

            await _repo.SaveAsync(new QuantityMeasurementEntity(
                "SUBTRACT", q1.ToString(), q2.ToString(), result.ToString()));

            Assert.AreEqual(1, await _repo.GetTotalCountAsync());
        }

        [TestMethod]
        public async Task Integration_DivideVolume_ResultPersistedToDatabase()
        {
            var q1     = new QuantityDTO(2, "LITRE", MeasurementType.VOLUME);
            var q2     = new QuantityDTO(1, "LITRE", MeasurementType.VOLUME);
            double result = _service.Divide(q1, q2);
            Assert.AreEqual(2.0, result, 0.0001);

            await _repo.SaveAsync(new QuantityMeasurementEntity(
                "DIVIDE", q1.ToString(), q2.ToString(), result.ToString("F4")));

            Assert.AreEqual(1, await _repo.GetTotalCountAsync());
        }

        [TestMethod]
        public async Task Integration_AddTemperature_ErrorPersistedToDatabase()
        {
            var q1 = new QuantityDTO(10, "CELSIUS", MeasurementType.TEMPERATURE);
            var q2 = new QuantityDTO(20, "CELSIUS", MeasurementType.TEMPERATURE);

            try
            {
                _service.Add(q1, q2);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (QuantityMeasurementException ex)
            {
                await _repo.SaveAsync(new QuantityMeasurementEntity(
                    "ADD", q1.ToString(), q2.ToString(), ex.Message, true));
            }

            var errors = await _repo.GetErroredAsync();
            Assert.AreEqual(1, errors.Count);
            Assert.IsTrue(errors[0].HasError);
            StringAssert.Contains(errors[0].ErrorMessage!, "Temperature");
        }

        [TestMethod]
        public async Task Integration_DivideByZero_ErrorPersistedToDatabase()
        {
            var q1 = new QuantityDTO(1, "FEET",   MeasurementType.LENGTH);
            var q2 = new QuantityDTO(0, "INCHES", MeasurementType.LENGTH);

            try
            {
                _service.Divide(q1, q2);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception ex)
            {
                await _repo.SaveAsync(new QuantityMeasurementEntity(
                    "DIVIDE", q1.ToString(), q2.ToString(), ex.Message, true));
            }

            var errors = await _repo.GetErroredAsync();
            Assert.AreEqual(1, errors.Count);
        }

        [TestMethod]
        public async Task Integration_MultipleOperations_AllPersistedAndFilterable()
        {
            var l1 = new QuantityDTO(1,    "FEET",       MeasurementType.LENGTH);
            var l2 = new QuantityDTO(12,   "INCHES",     MeasurementType.LENGTH);
            var w1 = new QuantityDTO(1,    "KILOGRAM",   MeasurementType.WEIGHT);
            var w2 = new QuantityDTO(1000, "GRAM",       MeasurementType.WEIGHT);

            bool cmp = _service.Compare(l1, l2);
            await _repo.SaveAsync(new QuantityMeasurementEntity(
                "COMPARE", l1.ToString(), l2.ToString(), cmp.ToString()));

            var add = _service.Add(w1, w2);
            await _repo.SaveAsync(new QuantityMeasurementEntity(
                "ADD", w1.ToString(), w2.ToString(), add.ToString()));

            Assert.AreEqual(2,  await _repo.GetTotalCountAsync());
            Assert.AreEqual(1, (await _repo.GetByOperationAsync("COMPARE")).Count);
            Assert.AreEqual(1, (await _repo.GetByOperationAsync("ADD")).Count);
        }
    }
}