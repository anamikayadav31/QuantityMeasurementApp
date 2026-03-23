using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Context;
using QuantityMeasurementApp.RepoLayer.Implementations;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.Tests
{
    // ── UC16: Database Repository Tests ──────────────────────────────────
    // DatabaseConfig and SQLite are removed — replaced by EF Core.
    // Tests use EF Core InMemoryDatabase for isolation and speed.
    // Each test gets its own uniquely named database so tests never interfere.
    [TestClass]
    public class DatabaseRepositoryTests
    {
        private IQuantityMeasurementRepository _repo = null!;
        private AppDbContext _db = null!;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"DbTest_{Guid.NewGuid()}")
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
        public async Task Save_NullEntity_ShouldThrowException()
        {
            try
            {
                await _repo.SaveAsync(null);
                Assert.Fail(); // fail if no exception
            }
            catch (ArgumentNullException)
            {
                Assert.IsTrue(true); // pass if exception occurs
            }
        }

        [TestMethod]
        public async Task Save_MultipleEntities_AllPersisted()
        {
            for (int i = 0; i < 5; i++)
                await _repo.SaveAsync(Make("ADD", $"Q{i}", $"Q{i + 1}", $"{i + 1}"));
            Assert.AreEqual(5, await _repo.GetTotalCountAsync());
        }

        [TestMethod]
        public async Task Save_ErrorEntity_PersistsErrorFlagAndMessage()
        {
            var entity = new QuantityMeasurementEntity(
                "ADD", "10 CELSIUS", "20 CELSIUS",
                "Temperature does not support arithmetic operation 'Add'.", true);
            await _repo.SaveAsync(entity);

            var loaded = await _repo.FindByIdAsync(entity.Id);
            Assert.IsNotNull(loaded);
            Assert.IsTrue(loaded.HasError);
            StringAssert.Contains(loaded.ErrorMessage!, "Temperature");
        }

        [TestMethod]
        public async Task GetAll_EmptyRepo_ReturnsEmptyList()
            => Assert.AreEqual(0, (await _repo.GetAllAsync()).Count);

        [TestMethod]
        public async Task GetAll_AfterSave_ReturnsCorrectCount()
        {
            await _repo.SaveAsync(Make("CONVERT", "1 FEET", "INCHES", "12"));
            await _repo.SaveAsync(Make("ADD", "1 KG", "500 GRAM", "1.5 KG"));
            Assert.AreEqual(2, (await _repo.GetAllAsync()).Count);
        }

        [TestMethod]
        public async Task FindById_ExistingId_ReturnsCorrectEntity()
        {
            var entity = Make("SUBTRACT", "2 KG", "500 GRAM", "1.5");
            await _repo.SaveAsync(entity);

            var found = await _repo.FindByIdAsync(entity.Id);
            Assert.IsNotNull(found);
            Assert.AreEqual(entity.Operation, found.Operation);
            Assert.AreEqual(entity.Operand1, found.Operand1);
        }

        [TestMethod]
        public async Task FindById_NonExistingId_ReturnsNull()
            => Assert.IsNull(await _repo.FindByIdAsync(999999));

        [TestMethod]
        public async Task GetByOperation_ReturnsOnlyMatchingOperation()
        {
            await _repo.SaveAsync(Make("COMPARE", "1 FEET", "12 INCHES", "True"));
            await _repo.SaveAsync(Make("ADD", "1 FEET", "12 INCHES", "2 FEET"));
            await _repo.SaveAsync(Make("COMPARE", "1 KILOGRAM", "1000 GRAM", "True"));

            var result = await _repo.GetByOperationAsync("COMPARE");
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(r => r.Operation == "COMPARE"));
        }

        [TestMethod]
        public async Task GetByOperation_NoMatch_ReturnsEmptyList()
        {
            await _repo.SaveAsync(Make("ADD", "1 FEET", "1 FEET", "2 FEET"));
            Assert.AreEqual(0, (await _repo.GetByOperationAsync("DIVIDE")).Count);
        }

        [TestMethod]
        public async Task GetTotalCount_Empty_ReturnsZero()
            => Assert.AreEqual(0, await _repo.GetTotalCountAsync());

        [TestMethod]
        public async Task GetTotalCount_AfterSavingThree_ReturnsThree()
        {
            await _repo.SaveAsync(Make("COMPARE", "1 LITRE", "1 LITRE", "True"));
            await _repo.SaveAsync(Make("ADD", "1 LITRE", "1000 ML", "2 LITRE"));
            await _repo.SaveAsync(Make("DIVIDE", "2 LITRE", "1 LITRE", "2"));
            Assert.AreEqual(3, await _repo.GetTotalCountAsync());
        }

        [TestMethod]
        public async Task DeleteAll_RemovesAllRecords_CountBecomesZero()
        {
            await _repo.SaveAsync(Make("COMPARE", "0 CELSIUS", "32 FAHRENHEIT", "True"));
            await _repo.SaveAsync(Make("CONVERT", "0 CELSIUS", "FAHRENHEIT", "32"));
            await _repo.DeleteAllAsync();
            Assert.AreEqual(0, await _repo.GetTotalCountAsync());
        }

        [TestMethod]
        public async Task AfterDeleteAll_CanSaveNewRecords()
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