using Microsoft.Extensions.Configuration;
using QuantityMeasurementApp.BussinessLayer.Services;
using QuantityMeasurementApp.BussinessLayer.Interfaces;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using QuantityMeasurementApp.RepoLayer.Implementations;
using QuantityMeasurementApp.RepoLayer.Config;
using QuantityMeasurementApp.ConsoleApp.Menu;
using QuantityMeasurementApp.ConsoleApp.Interfaces;
using QuantityMeasurementApp.ConsoleApp.Controllers;

namespace QuantityMeasurementApp.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Load appsettings.json
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .Build();

            // 2. Read settings (defaults: use database, file = QuantityMeasurementDB.db)
            bool useDatabase  = bool.Parse(config["Repository:UseDatabase"] ?? "false");
            string dbPath     = config["Repository:DatabasePath"] ?? "QuantityMeasurementDB.db";

            // 3. Create the repository based on config
            IQuantityMeasurementRepository repository = CreateRepository(useDatabase, dbPath);

            Console.WriteLine($"[Startup] Repository: {(useDatabase ? $"SQLite → {dbPath}" : "In-memory cache")}");

            // 4. Wire up the rest (unchanged from UC15)
            IQuantityMeasurementService      service    = new QuantityMeasurementServiceImpl();
            QuantityMeasurementController    controller = new(service, repository);
            IMenu                            menu       = new Menu.Menu(controller);

            menu.Show();
        }

        /// <summary>
        /// Factory method — returns either the DB repo or the cache repo.
        /// Everything else in the app is unaware of which one was chosen.
        /// </summary>
        private static IQuantityMeasurementRepository CreateRepository(bool useDatabase, string dbPath)
        {
            if (useDatabase)
            {
                var dbConfig = new DatabaseConfig(dbPath);
                return new QuantityMeasurementDatabaseRepository(dbConfig);
            }
            else
            {
                return QuantityMeasurementCacheRepository.Instance;
            }
        }
    }
}