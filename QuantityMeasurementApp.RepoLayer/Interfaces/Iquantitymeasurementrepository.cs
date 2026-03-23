using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.RepoLayer.Interfaces
{
    // ── Repository Interface ──────────────────────────────────────────────
    //
    // Spring equivalent: extends JpaRepository<QuantityMeasurementEntity, Integer>
    //
    // The controller and service depend on this interface only.
    // The EF Core implementation is injected at runtime via DI.
    // This means you can swap SQL Server for any other database
    // without touching a single controller or service file.

    public interface IQuantityMeasurementRepository
    {
        // Save one record
        Task SaveAsync(QuantityMeasurementEntity entity);

        // Get all records ordered by timestamp
        Task<IReadOnlyList<QuantityMeasurementEntity>> GetAllAsync();

        // Find by primary key
        Task<QuantityMeasurementEntity?> FindByIdAsync(int id);

        // Filter by operation name (e.g. "COMPARE", "ADD")
        Task<IReadOnlyList<QuantityMeasurementEntity>> GetByOperationAsync(string operation);

        // All records where HasError = true
        Task<IReadOnlyList<QuantityMeasurementEntity>> GetErroredAsync();

        // Count records for a specific operation
        Task<int> CountByOperationAsync(string operation);

        // Total records in the table
        Task<int> GetTotalCountAsync();

        // Delete all records (used in tests)
        Task DeleteAllAsync();
    }
}