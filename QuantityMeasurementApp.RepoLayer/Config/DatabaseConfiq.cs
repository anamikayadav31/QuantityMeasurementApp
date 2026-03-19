namespace QuantityMeasurementApp.RepoLayer.Config
{
    /// <summary>
    /// Holds database configuration loaded from appsettings.json.
    /// Just a plain data class — no logic here.
    /// </summary>
    public class DatabaseConfig
    {
        /// <summary>
        /// Full path to the SQLite file, e.g. "QuantityMeasurementDB.db"
        /// </summary>
        public string DatabasePath { get; }

        /// <summary>
        /// The ADO.NET connection string built from the path.
        /// </summary>
        public string ConnectionString { get; }

        public DatabaseConfig(string databasePath)
        {
            if (string.IsNullOrWhiteSpace(databasePath))
                throw new ArgumentException("Database path cannot be empty.");

            DatabasePath     = databasePath;
            ConnectionString = $"Data Source={databasePath}";
        }
    }
}