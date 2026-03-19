using Microsoft.Data.Sqlite;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.ModelLayer.Exceptions;
using QuantityMeasurementApp.RepoLayer.Config;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.RepoLayer.Implementations
{
    /// <summary>
    /// Stores QuantityMeasurementEntity records in a SQLite database using ADO.NET.
    ///
    /// Key ideas:
    ///   - Creates the table automatically on first run (no manual setup needed).
    ///   - Uses parameterised queries everywhere (no SQL injection possible).
    ///   - Every public method opens and closes its own connection
    ///     (simple and safe — no connection pool needed for SQLite).
    ///   - Throws DatabaseException if anything goes wrong, so callers
    ///     don't need to know about SQLite specifics.
    /// </summary>
    public class QuantityMeasurementDatabaseRepository : IQuantityMeasurementRepository
    {
        private readonly string _connectionString;

        public QuantityMeasurementDatabaseRepository(DatabaseConfig config)
        {
            _connectionString = config.ConnectionString;
            InitialiseDatabase();   // create table if it doesn't exist yet
        }

        // ── Schema creation ──────────────────────────────────────────────

        /// <summary>
        /// Runs once at startup. Creates the table if it doesn't exist.
        /// Safe to call many times — CREATE TABLE IF NOT EXISTS is idempotent.
        /// </summary>
        private void InitialiseDatabase()
        {
            const string sql = """
                CREATE TABLE IF NOT EXISTS QuantityMeasurements (
                    Id           TEXT    NOT NULL PRIMARY KEY,
                    Operation    TEXT    NOT NULL,
                    Operand1     TEXT    NOT NULL,
                    Operand2     TEXT,
                    TargetUnit   TEXT,
                    Result       TEXT,
                    HasError     INTEGER NOT NULL DEFAULT 0,
                    ErrorMessage TEXT,
                    Timestamp    TEXT    NOT NULL
                );
                """;

            try
            {
                using var conn = OpenConnection();
                using var cmd  = new SqliteCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to initialise the database.", ex);
            }
        }

        // ── Save ─────────────────────────────────────────────────────────

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            const string sql = """
                INSERT INTO QuantityMeasurements
                    (Id, Operation, Operand1, Operand2, TargetUnit, Result, HasError, ErrorMessage, Timestamp)
                VALUES
                    (@Id, @Operation, @Operand1, @Operand2, @TargetUnit, @Result, @HasError, @ErrorMessage, @Timestamp);
                """;

            try
            {
                using var conn = OpenConnection();
                using var cmd  = new SqliteCommand(sql, conn);

                // Each @param is bound separately — this is what prevents SQL injection.
                cmd.Parameters.AddWithValue("@Id",           entity.Id.ToString());
                cmd.Parameters.AddWithValue("@Operation",    entity.Operation);
                cmd.Parameters.AddWithValue("@Operand1",     entity.Operand1);
                cmd.Parameters.AddWithValue("@Operand2",     entity.Operand2     ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@TargetUnit",   entity.TargetUnit   ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Result",       entity.Result       ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@HasError",     entity.HasError ? 1 : 0);
                cmd.Parameters.AddWithValue("@ErrorMessage", entity.ErrorMessage ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Timestamp",    entity.Timestamp.ToString("o")); // ISO 8601

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Failed to save entity {entity.Id}.", ex);
            }
        }

        // ── GetAll ───────────────────────────────────────────────────────

        public IReadOnlyList<QuantityMeasurementEntity> GetAll()
        {
            const string sql = "SELECT * FROM QuantityMeasurements ORDER BY Timestamp;";
            return RunQuery(sql);
        }

        // ── FindById ─────────────────────────────────────────────────────

        public QuantityMeasurementEntity? FindById(Guid id)
        {
            const string sql = "SELECT * FROM QuantityMeasurements WHERE Id = @Id;";

            try
            {
                using var conn = OpenConnection();
                using var cmd  = new SqliteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id.ToString());

                using var reader = cmd.ExecuteReader();
                return reader.Read() ? MapRow(reader) : null;
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Failed to find entity {id}.", ex);
            }
        }

        // ── GetByOperation ───────────────────────────────────────────────

        public IReadOnlyList<QuantityMeasurementEntity> GetByOperation(string operation)
        {
            const string sql = "SELECT * FROM QuantityMeasurements WHERE Operation = @Op ORDER BY Timestamp;";

            try
            {
                using var conn = OpenConnection();
                using var cmd  = new SqliteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Op", operation);

                using var reader = cmd.ExecuteReader();
                return ReadAll(reader).AsReadOnly();
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Failed to query by operation '{operation}'.", ex);
            }
        }

        // ── GetTotalCount ────────────────────────────────────────────────

        public int GetTotalCount()
        {
            const string sql = "SELECT COUNT(*) FROM QuantityMeasurements;";

            try
            {
                using var conn = OpenConnection();
                using var cmd  = new SqliteCommand(sql, conn);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to get total count.", ex);
            }
        }

        // ── Clear / DeleteAll ────────────────────────────────────────────

        /// <summary>Deletes all rows. Alias kept so the interface is satisfied.</summary>
        public void Clear() => DeleteAll();

        public void DeleteAll()
        {
            const string sql = "DELETE FROM QuantityMeasurements;";

            try
            {
                using var conn = OpenConnection();
                using var cmd  = new SqliteCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Failed to delete all records.", ex);
            }
        }

        // ── Private helpers ──────────────────────────────────────────────

        /// <summary>Opens a connection and returns it already open.</summary>
        private SqliteConnection OpenConnection()
        {
            var conn = new SqliteConnection(_connectionString);
            conn.Open();
            return conn;
        }

        /// <summary>Runs a SELECT with no parameters and returns all rows.</summary>
        private IReadOnlyList<QuantityMeasurementEntity> RunQuery(string sql)
        {
            try
            {
                using var conn   = OpenConnection();
                using var cmd    = new SqliteCommand(sql, conn);
                using var reader = cmd.ExecuteReader();
                return ReadAll(reader).AsReadOnly();
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Query failed.", ex);
            }
        }

        /// <summary>Reads every row from an open reader into a list.</summary>
        private static List<QuantityMeasurementEntity> ReadAll(SqliteDataReader reader)
        {
            var list = new List<QuantityMeasurementEntity>();
            while (reader.Read())
                list.Add(MapRow(reader));
            return list;
        }

        /// <summary>
        /// Maps a single database row to a QuantityMeasurementEntity.
        /// Uses GetString / GetInt32 etc. — straightforward column-by-column mapping.
        /// </summary>
        private static QuantityMeasurementEntity MapRow(SqliteDataReader r)
        {
            return new QuantityMeasurementEntity
            {
                Id           = Guid.Parse(r.GetString(r.GetOrdinal("Id"))),
                Operation    = r.GetString(r.GetOrdinal("Operation")),
                Operand1     = r.GetString(r.GetOrdinal("Operand1")),
                Operand2     = r.IsDBNull(r.GetOrdinal("Operand2"))     ? null : r.GetString(r.GetOrdinal("Operand2")),
                TargetUnit   = r.IsDBNull(r.GetOrdinal("TargetUnit"))   ? null : r.GetString(r.GetOrdinal("TargetUnit")),
                Result       = r.IsDBNull(r.GetOrdinal("Result"))       ? null : r.GetString(r.GetOrdinal("Result")),
                HasError     = r.GetInt32(r.GetOrdinal("HasError")) == 1,
                ErrorMessage = r.IsDBNull(r.GetOrdinal("ErrorMessage")) ? null : r.GetString(r.GetOrdinal("ErrorMessage")),
                Timestamp    = DateTime.Parse(r.GetString(r.GetOrdinal("Timestamp")))
            };
        }
    }
}