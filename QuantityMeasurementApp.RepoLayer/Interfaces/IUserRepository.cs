using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.RepoLayer.Interfaces
{
    // ── EF Core User Repository Interface ────────────────────────────────
    // All methods are async — EF Core uses async database I/O.
    // Spring equivalent: JpaRepository<UserEntity, Integer>
    public interface IUserRepository
    {
        Task SaveAsync(UserEntity user);
        Task<UserEntity?> FindByUsernameAsync(string username);
        Task<UserEntity?> FindByEmailAsync(string email);
        Task<UserEntity?> FindByIdAsync(int id);
        Task<IReadOnlyList<UserEntity>> GetAllAsync();
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> EmailExistsAsync(string email);
    }
}