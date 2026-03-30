using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Context;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.RepoLayer.Implementations
{
    // ── EF Core User Repository Implementation ────────────────────────────
    // Spring equivalent: @Repository class extending JpaRepository
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db) => _db = db;

        public async Task SaveAsync(UserEntity user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }

        public async Task<UserEntity?> FindByUsernameAsync(string username)
            => await _db.Users
                        .FirstOrDefaultAsync(u =>
                            u.Username.ToLower() == username.ToLower());

        public async Task<UserEntity?> FindByEmailAsync(string email)
            => await _db.Users
                        .FirstOrDefaultAsync(u => u.Email == email);

        public async Task<UserEntity?> FindByIdAsync(int id)
            => await _db.Users.FindAsync(id);

        public async Task<IReadOnlyList<UserEntity>> GetAllAsync()
            => await _db.Users
                        .OrderBy(u => u.CreatedAt)
                        .ToListAsync();

        public async Task<bool> UsernameExistsAsync(string username)
            => await _db.Users
                        .AnyAsync(u => u.Username.ToLower() == username.ToLower());

        public async Task<bool> EmailExistsAsync(string email)
            => await _db.Users.AnyAsync(u => u.Email == email);
    }
}