using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;

namespace QuantityMeasurementApp.RepoLayer.Implementations
{
    /// <summary>
    /// Singleton in-memory cache repository for QuantityMeasurementEntity records.
    ///
    /// Design:
    ///   - Singleton: only one instance throughout the application lifetime
    ///   - Stores records in a List held in memory
    ///   - Optionally persists to disk via pipe-delimited text file
    ///     (BinaryFormatter is obsolete in .NET 5+ so it is NOT used)
    ///   - Thread-safe instance creation via double-checked lock
    /// </summary>
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        // ── Singleton ────────────────────────────────────────────────────────
        private static QuantityMeasurementCacheRepository? _instance;
        private static readonly object _lock = new object();

        public static QuantityMeasurementCacheRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new QuantityMeasurementCacheRepository();
                    }
                }
                return _instance;
            }
        }

        // ── In-memory store ──────────────────────────────────────────────────
        private readonly List<QuantityMeasurementEntity> _cache = new();

        private readonly string _filePath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "quantity_measurements.dat");

        private QuantityMeasurementCacheRepository()
        {
            LoadFromDisk();
        }

        // ── IQuantityMeasurementRepository ───────────────────────────────────

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null");

            _cache.Add(entity);
            SaveToDisk(entity);
        }

        public IReadOnlyList<QuantityMeasurementEntity> GetAll()
            => _cache.AsReadOnly();

        public QuantityMeasurementEntity? FindById(Guid id)
            => _cache.FirstOrDefault(e => e.Id == id);

        public void Clear()
        {
            _cache.Clear();
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }

        // ── Disk persistence ─────────────────────────────────────────────────

        private void SaveToDisk(QuantityMeasurementEntity entity)
        {
            try
            {
                File.AppendAllText(_filePath, Serialize(entity) + Environment.NewLine);
            }
            catch { }
        }

        private void LoadFromDisk()
        {
            try
            {
                if (!File.Exists(_filePath)) return;
                foreach (string line in File.ReadAllLines(_filePath))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var entity = Deserialize(line);
                    if (entity != null) _cache.Add(entity);
                }
            }
            catch { _cache.Clear(); }
        }

        // Format: id|operation|operand1|operand2|targetUnit|result|hasError|errorMessage|timestamp
        private static string Serialize(QuantityMeasurementEntity e)
            => string.Join("|",
                e.Id,
                e.Operation    ?? "",
                e.Operand1     ?? "",
                e.Operand2     ?? "",
                e.TargetUnit   ?? "",
                e.Result       ?? "",
                e.HasError,
                e.ErrorMessage ?? "",
                e.Timestamp.ToString("o"));

        private static QuantityMeasurementEntity? Deserialize(string line)
        {
            try
            {
                string[] p = line.Split('|');
                if (p.Length < 9) return null;
                return new QuantityMeasurementEntity
                {
                    Id           = Guid.Parse(p[0]),
                    Operation    = p[1],
                    Operand1     = p[2],
                    Operand2     = string.IsNullOrEmpty(p[3]) ? null : p[3],
                    TargetUnit   = string.IsNullOrEmpty(p[4]) ? null : p[4],
                    Result       = string.IsNullOrEmpty(p[5]) ? null : p[5],
                    HasError     = bool.Parse(p[6]),
                    ErrorMessage = string.IsNullOrEmpty(p[7]) ? null : p[7],
                    Timestamp    = DateTime.Parse(p[8])
                };
            }
            catch { return null; }
        }
    }
}