using QuantityMeasurementApp.BussinessLayer.Services;
using QuantityMeasurementApp.BussinessLayer.Interfaces;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using QuantityMeasurementApp.RepoLayer.Implementations;
using QuantityMeasurementApp.ConsoleApp.Menu;
using QuantityMeasurementApp.ConsoleApp.Interfaces;
using QuantityMeasurementApp.ConsoleApp.Controllers;

namespace QuantityMeasurementApp.ConsoleApp
{
    /// <summary>
    /// Application entry point for the Quantity Measurement System (UC15).
    ///
    /// Responsibilities:
    ///   - Bootstrap and wire all layers via Factory + Dependency Injection
    ///   - Implements Factory pattern to create service and controller instances
    ///   - Implements Facade pattern: hides wiring complexity behind IMenu.Show()
    ///   - Delegates all logic to the controller via the menu
    ///
    /// Design patterns used:
    ///   - Factory:    CreateService(), CreateController() factory methods
    ///   - Singleton:  Repository accessed via QuantityMeasurementCacheRepository.Instance
    ///   - Facade:     IMenu.Show() is the simplified entry point
    ///   - DI:         All dependencies injected via constructors
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // -- Factory: create repository (Singleton) -----------------------
            IQuantityMeasurementRepository repository = CreateRepository();

            // -- Factory: create service with repo dependency -----------------
            IQuantityMeasurementService service = CreateService();

            // -- Factory: create controller with service + repo ---------------
            QuantityMeasurementController controller = CreateController(service, repository);

            // -- Facade: start the menu loop ----------------------------------
            IMenu menu = new QuantityMeasurementApp.ConsoleApp.Menu.Menu(controller);
            menu.Show();
        }

        /// <summary>
        /// Factory method for repository creation.
        /// Returns the Singleton cache repository.
        /// Can be swapped to return a database or cloud repository.
        /// </summary>
        private static IQuantityMeasurementRepository CreateRepository()
        {
            return QuantityMeasurementCacheRepository.Instance;
        }

        /// <summary>
        /// Factory method for service creation.
        /// Returns the default service implementation.
        /// Can be swapped for a mock, test double, or alternative implementation.
        /// </summary>
        private static IQuantityMeasurementService CreateService()
        {
            return new QuantityMeasurementServiceImpl();
        }

        /// <summary>
        /// Factory method for controller creation.
        /// Injects service and repository dependencies.
        /// </summary>
        private static QuantityMeasurementController CreateController(
            IQuantityMeasurementService service,
            IQuantityMeasurementRepository repository)
        {
            return new QuantityMeasurementController(service, repository);
        }
    }
}
