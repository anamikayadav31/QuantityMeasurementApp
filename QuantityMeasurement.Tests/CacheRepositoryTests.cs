using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Context;
using QuantityMeasurementApp.RepoLayer.Implementations;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.Tests
{
    // ── UC16: Repository Tests ────────────────────────────────────────────
    // Old CacheRepository is replaced by EF Core.
    // Tests now use EF Core InMemoryDatabase — fast, no SQL Server needed.
    [TestClass]
    public class CacheRepositoryTests
    {
        private IQuantityMeasurementRepository _repo = null!;
        private AppDbContext _db = null!;

        [TestInitialize]
        public void Setup()
        {
            // Each test gets its own fresh in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"CacheTest_{Guid.NewGuid()}")
                .Options;
            _db = new AppDbContext(options);
            _repo = new QuantityMeasurementRepository(_db);
        }

        [TestCleanup]
        public void Cleanup() => _db.Dispose();

        [TestMethod]
        public async Task Save_ValidEntity_IncreasesCount()
        {
            await _repo.SaveAsync(Make("COMPARE", "1 FEET", "12 INCHES", "True"));
            Assert.AreEqual(1, await _repo.GetTotalCountAsync());
        }

        // Fixed: use Assert.ThrowsExceptionAsync instead of [ExpectedException]
        [TestMethod]
        public async Task Save_NullEntity_ShouldThrowArgumentNullException()
        {
            try
            {
                await _repo.SaveAsync(null);
                Assert.Fail(); // fail if no exception is thrown
            }
            catch (ArgumentNullException)
            {
                // test passed
            }
        }

        [TestMethod]
        public async Task Save_MultipleEntities_AllSaved()
        {
            await _repo.SaveAsync(Make("ADD", "1 FEET", "12 INCHES", "2 FEET"));
            await _repo.SaveAsync(Make("ADD", "1 KILOGRAM", "500 GRAM", "1.5 KILOGRAM"));
            await _repo.SaveAsync(Make("SUBTRACT", "2 LITRE", "1 LITRE", "1 LITRE"));
            Assert.AreEqual(3, await _repo.GetTotalCountAsync());
        }

        [TestMethod]
        public async Task GetAll_AfterSave_ReturnsEntity()
        {
            await _repo.SaveAsync(Make("ADD", "1 FEET", "12 INCHES", "2 FEET"));
            var all = await _repo.GetAllAsync();
            Assert.AreEqual(1, all.Count);
        }

        [TestMethod]
        public async Task GetByOperation_ReturnsOnlyMatchingRecords()
        {
            await _repo.SaveAsync(Make("COMPARE", "1 FEET", "12 INCHES", "True"));
            await _repo.SaveAsync(Make("ADD", "1 FEET", "12 INCHES", "2 FEET"));
            var result = await _repo.GetByOperationAsync("COMPARE");
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("COMPARE", result[0].Operation);
        }

        [TestMethod]
        public async Task GetByOperation_CaseInsensitive_ReturnsResults()
        {
            await _repo.SaveAsync(Make("COMPARE", "1 FEET", "12 INCHES", "True"));
            // Searching with lowercase should still find the record
            var result = await _repo.GetByOperationAsync("compare");
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public async Task GetByOperation_NoMatch_ReturnsEmpty()
        {
            await _repo.SaveAsync(Make("ADD", "1 FEET", "1 FEET", "2 FEET"));
            var result = await _repo.GetByOperationAsync("DIVIDE");
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetTotalCount_ReflectsActualCount()
        {
            Assert.AreEqual(0, await _repo.GetTotalCountAsync());
            await _repo.SaveAsync(Make("COMPARE", "1", "1", "True"));
            Assert.AreEqual(1, await _repo.GetTotalCountAsync());
            await _repo.SaveAsync(Make("ADD", "1", "1", "2"));
            Assert.AreEqual(2, await _repo.GetTotalCountAsync());
        }

        [TestMethod]
        public async Task DeleteAll_RemovesAllEntities()
        {
            await _repo.SaveAsync(Make("DIVIDE", "2 FEET", "1 FEET", "2"));
            await _repo.DeleteAllAsync();
            Assert.AreEqual(0, await _repo.GetTotalCountAsync());
        }

        [TestMethod]
        public async Task AfterDeleteAll_CanSaveAgain()
        {
            await _repo.SaveAsync(Make("COMPARE", "1 FEET", "12 INCHES", "True"));
            await _repo.DeleteAllAsync();
            await _repo.SaveAsync(Make("ADD", "1 KG", "1000 GRAM", "2 KG"));
            Assert.AreEqual(1, await _repo.GetTotalCountAsync());
        }

        private static QuantityMeasurementEntity Make(
            string op, string op1, string op2, string result)
            => new QuantityMeasurementEntity(op, op1, op2, result);
    }
}