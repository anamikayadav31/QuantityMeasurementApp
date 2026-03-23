using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Context;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.RepoLayer.Implementations
{
    // ── EF Core Repository Implementation ────────────────────────────────
    //
    // Spring equivalent:
    //   @Repository
    //   class QuantityMeasurementRepositoryImpl
    //       implements IQuantityMeasurementRepository
    //   Uses EntityManager / JpaRepository methods internally
    //
    // EF Core replaces ALL raw ADO.NET SQL:
    //   SqlCommand("SELECT * FROM ...")  →  _db.QuantityMeasurements.ToListAsync()
    //   SqlCommand("INSERT INTO ...")    →  _db.Add(entity); _db.SaveChangesAsync()
    //   Parameterised queries            →  LINQ (EF generates safe SQL automatically)
    //
    // No SQL injection possible — EF Core parameterises everything.

    public class QuantityMeasurementRepository : IQuantityMeasurementRepository
    {
        private readonly AppDbContext _db;

        // DbContext is injected — AddDbContext() in Program.cs handles this
        public QuantityMeasurementRepository(AppDbContext db)
            => _db = db;

        // ── Save ──────────────────────────────────────────────────────────

        // Spring: repository.save(entity)
        public async Task SaveAsync(QuantityMeasurementEntity entity)
        {
             if (entity == null)
        throw new ArgumentNullException(nameof(entity)); 
            _db.QuantityMeasurements.Add(entity);
            await _db.SaveChangesAsync();
        }

        // ── GetAll ────────────────────────────────────────────────────────

        // Spring: repository.findAll(Sort.by("timestamp"))
        public async Task<IReadOnlyList<QuantityMeasurementEntity>> GetAllAsync()
            => await _db.QuantityMeasurements
                        .OrderBy(e => e.Timestamp)
                        .ToListAsync();

        // ── FindById ──────────────────────────────────────────────────────

        // Spring: repository.findById(id)
        public async Task<QuantityMeasurementEntity?> FindByIdAsync(int id)
            => await _db.QuantityMeasurements.FindAsync(id);

        // ── GetByOperation ────────────────────────────────────────────────

        // Spring: repository.findByOperation(operation)
        // EF Core generates: WHERE UPPER(Operation) = UPPER(@op)
        public async Task<IReadOnlyList<QuantityMeasurementEntity>> GetByOperationAsync(string operation)
            => await _db.QuantityMeasurements
                        .Where(e => e.Operation.ToUpper() == operation.ToUpper())
                        .OrderBy(e => e.Timestamp)
                        .ToListAsync();

        // ── GetErrored ────────────────────────────────────────────────────

        // Spring: repository.findByHasErrorTrue()
        public async Task<IReadOnlyList<QuantityMeasurementEntity>> GetErroredAsync()
            => await _db.QuantityMeasurements
                        .Where(e => e.HasError)
                        .OrderBy(e => e.Timestamp)
                        .ToListAsync();

        // ── CountByOperation ──────────────────────────────────────────────

        // Spring: repository.countByOperationAndIsErrorFalse(operation)
        public async Task<int> CountByOperationAsync(string operation)
            => await _db.QuantityMeasurements
                        .CountAsync(e => e.Operation.ToUpper() == operation.ToUpper());

        // ── GetTotalCount ─────────────────────────────────────────────────

        // Spring: repository.count()
        public async Task<int> GetTotalCountAsync()
            => await _db.QuantityMeasurements.CountAsync();

        // ── DeleteAll ─────────────────────────────────────────────────────

        // Spring: repository.deleteAll()
        public async Task DeleteAllAsync()
        {
            _db.QuantityMeasurements.RemoveRange(_db.QuantityMeasurements);
            await _db.SaveChangesAsync();
        }
    }
}