using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.RepoLayer.Interfaces
{
    /// <summary>
    /// Repository interface defining the contract for data access operations
    /// on QuantityMeasurementEntity records.
    ///
    /// Follows Interface Segregation Principle — clients (service) depend only
    /// on this interface, not on any concrete implementation.
    ///
    /// Current implementation: in-memory cache with optional disk persistence.
    /// Future implementations could use a relational DB, NoSQL, or cloud store
    /// without any changes to the service layer.
    /// </summary>
    public interface IQuantityMeasurementRepository
    {
        /// <summary>
        /// Save a new measurement entity record.
        /// </summary>
        void Save(QuantityMeasurementEntity entity);

        /// <summary>
        /// Retrieve all stored measurement records.
        /// </summary>
        IReadOnlyList<QuantityMeasurementEntity> GetAll();

        /// <summary>
        /// Retrieve a single record by its unique ID.
        /// Returns null if not found.
        /// </summary>
        QuantityMeasurementEntity? FindById(Guid id);

        /// <summary>
        /// Delete all stored records.
        /// </summary>
        void Clear();
    }
}