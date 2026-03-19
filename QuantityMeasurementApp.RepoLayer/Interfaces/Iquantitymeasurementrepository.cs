using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.RepoLayer.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        // ── Existing (UC15) ──────────────────────────────────────────────
        void Save(QuantityMeasurementEntity entity);
        IReadOnlyList<QuantityMeasurementEntity> GetAll();
        QuantityMeasurementEntity? FindById(Guid id);
        void Clear();

        // ── New in UC16 ──────────────────────────────────────────────────

        /// <summary>Returns all records whose Operation matches the given string.</summary>
        IReadOnlyList<QuantityMeasurementEntity> GetByOperation(string operation);

        /// <summary>Total number of records stored.</summary>
        int GetTotalCount();

        /// <summary>Delete every record (useful for tests / reset).</summary>
        void DeleteAll();
    }
}