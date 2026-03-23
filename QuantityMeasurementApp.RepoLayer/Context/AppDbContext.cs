using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.ModelLayer.Entities;

namespace QuantityMeasurementApp.RepoLayer.Context
{
    // ── EF Core DbContext ─────────────────────────────────────────────────
    //
    // This is the BRIDGE between your C# classes and SQL Server tables.
    //
    // Spring equivalent:
    //   EntityManager / JpaRepository behind the scenes
    //   @EnableJpaRepositories
    //
    // How it works:
    //   1. DbContext knows about your entity classes (DbSet<T>)
    //   2. EF Core compares your C# model to the database schema
    //   3. Migrations generate SQL ALTER/CREATE statements to sync them
    //   4. dotnet ef database update runs those SQL statements in SSMS

    public class AppDbContext : DbContext
    {
        // Constructor — receives connection settings from DI container
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // ── Tables (DbSet = one table per entity) ─────────────────────────

        // Spring equivalent: @Entity class = one DbSet here
        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }
        public DbSet<UserEntity>                Users                 { get; set; }

        // ── Table configuration (indexes, constraints) ─────────────────────

        // Spring equivalent: @Table(uniqueConstraints = @UniqueConstraint(...))
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // QuantityMeasurements table
            modelBuilder.Entity<QuantityMeasurementEntity>(entity =>
            {
                // Index on Operation for fast GetByOperation() queries
                entity.HasIndex(e => e.Operation)
                      .HasDatabaseName("IX_QuantityMeasurements_Operation");

                // Index on Timestamp for ordered history queries
                entity.HasIndex(e => e.Timestamp)
                      .HasDatabaseName("IX_QuantityMeasurements_Timestamp");
            });

            // Users table
            modelBuilder.Entity<UserEntity>(entity =>
            {
                // Username must be unique across all users
                entity.HasIndex(e => e.Username)
                      .IsUnique()
                      .HasDatabaseName("IX_Users_Username");

                // Email must be unique (stored encrypted but still unique)
                entity.HasIndex(e => e.Email)
                      .IsUnique()
                      .HasDatabaseName("IX_Users_Email");
            });
        }
    }
}