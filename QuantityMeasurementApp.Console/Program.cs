using Microsoft.Extensions.Configuration;
using QuantityMeasurementApp.BussinessLayer.Services;
using QuantityMeasurementApp.BussinessLayer.Interfaces;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using QuantityMeasurementApp.RepoLayer.Implementations;
using QuantityMeasurementApp.RepoLayer.Config;
using QuantityMeasurementApp.ConsoleApp.Controllers;
using QuantityMeasurementApp.ConsoleApp.Interfaces;
using QuantityMeasurementApp.ConsoleApp.Menu;

// ── UC15/UC16: N-Tier Architecture + Database Integration ────────────────
//
// Program.cs is the "Composition Root" — the one place where all
// dependencies are created and wired together.
//
// Why is this important?
//   Every class in the app receives its dependencies through its constructor
//   (constructor injection). No class creates its own dependencies.
//   This makes it easy to:
//     - Swap the database for an in-memory store (for testing)
//     - Replace the service with a mock (for unit tests)
//     - Understand the whole app's structure from one place
//
// In UC17 (Web API), ASP.NET Core's built-in DI container does this
// automatically — here we do it manually ("Poor Man's DI").

// ── 1. Load appsettings.json ─────────────────────────────────────────────
IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .Build();

bool   useDatabase = bool.Parse(config["Repository:UseDatabase"] ?? "false");
string dbPath      = config["Repository:DatabasePath"] ?? "QuantityMeasurementDB.db";

// ── 2. Create repository ──────────────────────────────────────────────────
IQuantityMeasurementRepository repository;

if (useDatabase)
{
    // SQLite — data persists across restarts in a .db file
    repository = new QuantityMeasurementDatabaseRepository(new DatabaseConfig(dbPath));
    Console.WriteLine($"[Startup] Using SQLite database → {dbPath}");
}
else
{
    // In-memory cache — also writes to quantity_measurements.json for backup
    repository = QuantityMeasurementCacheRepository.GetInstance();
    Console.WriteLine("[Startup] Using in-memory cache (+ JSON backup file)");
}

// ── 3. Wire up the layers ─────────────────────────────────────────────────
IQuantityMeasurementService   service    = new QuantityMeasurementServiceImpl();
QuantityMeasurementController controller = new QuantityMeasurementController(service, repository);
IMenu                         menu       = new ConsoleMenu(controller);

// ── 4. Start the application ──────────────────────────────────────────────
menu.Show();