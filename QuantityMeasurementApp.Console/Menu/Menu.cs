using QuantityMeasurementApp.ConsoleApp.Controllers;
using QuantityMeasurementApp.ConsoleApp.Interfaces;

namespace QuantityMeasurementApp.ConsoleApp.Menu
{
    /// <summary>
    /// Main console menu for the Quantity Measurement Application.
    ///
    /// Responsibilities:
    ///   - Display all available operations
    ///   - Read and validate user choice
    ///   - Route to the appropriate controller method
    ///   - Loop until user chooses to exit
    ///
    /// Uses IMenu interface to allow substitution with other menu implementations.
    /// Controller is injected via constructor (Dependency Injection).
    /// </summary>
    public class Menu : IMenu
    {
        private readonly QuantityMeasurementController _controller;

        /// <summary>
        /// Constructor injection of the controller.
        /// </summary>
        public Menu(QuantityMeasurementController controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        /// <summary>
        /// Displays the menu and runs the main input loop until exit is chosen.
        /// </summary>
        public void Show()
        {
            bool exit = false;

            while (!exit)
            {
                PrintHeader();

                Console.Write("\nEnter your choice: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.WriteLine("  Invalid input. Please enter a number between 0 and 20.");
                    continue;
                }

                Console.WriteLine();

                switch (choice)
                {
                    // ── COMPARE ──────────────────────────────────────────────
                    case 1: _controller.PerformCompareLength(); break;
                    case 2: _controller.PerformCompareWeight(); break;
                    case 3: _controller.PerformCompareVolume(); break;
                    case 4: _controller.PerformCompareTemperature(); break;

                    // ── CONVERT ──────────────────────────────────────────────
                    case 5: _controller.PerformConvertLength(); break;
                    case 6: _controller.PerformConvertWeight(); break;
                    case 7: _controller.PerformConvertVolume(); break;
                    case 8: _controller.PerformConvertTemperature(); break;

                    // ── ADD ──────────────────────────────────────────────────
                    case 9: _controller.PerformAddLength(); break;
                    case 10: _controller.PerformAddWeight(); break;
                    case 11: _controller.PerformAddVolume(); break;

                    // ── SUBTRACT ─────────────────────────────────────────────
                    case 12: _controller.PerformSubtractLength(); break;
                    case 13: _controller.PerformSubtractWeight(); break;
                    case 14: _controller.PerformSubtractVolume(); break;

                    // ── DIVIDE ───────────────────────────────────────────────
                    case 15: _controller.PerformDivideLength(); break;
                    case 16: _controller.PerformDivideWeight(); break;
                    case 17: _controller.PerformDivideVolume(); break;

                    // ── HISTORY / EXIT ────────────────────────────────────────
                    case 18: _controller.ShowHistory(); break;

                    case 0:
                        Console.WriteLine("\nThank you for using the Quantity Measurement Application!");
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("  Invalid choice. Please select a number from the menu.");
                        break;
                }

                if (!exit) Console.WriteLine();
            }
        }

        private static void PrintHeader()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("        QUANTITY MEASUREMENT APPLICATION");
            Console.WriteLine("==================================================");

            Console.WriteLine("\nCOMPARE OPERATIONS");
            Console.WriteLine("[1]  Compare Length");
            Console.WriteLine("[2]  Compare Weight");
            Console.WriteLine("[3]  Compare Volume");
            Console.WriteLine("[4]  Compare Temperature");

            Console.WriteLine("\nCONVERT OPERATIONS");
            Console.WriteLine("[5]  Convert Length");
            Console.WriteLine("[6]  Convert Weight");
            Console.WriteLine("[7]  Convert Volume");
            Console.WriteLine("[8]  Convert Temperature");

            Console.WriteLine("\nADD OPERATIONS");
            Console.WriteLine("[9]   Add Length");
            Console.WriteLine("[10]  Add Weight");
            Console.WriteLine("[11]  Add Volume");

            Console.WriteLine("\nSUBTRACT OPERATIONS");
            Console.WriteLine("[12]  Subtract Length");
            Console.WriteLine("[13]  Subtract Weight");
            Console.WriteLine("[14]  Subtract Volume");

            Console.WriteLine("\nDIVIDE OPERATIONS");
            Console.WriteLine("[15]  Divide Length");
            Console.WriteLine("[16]  Divide Weight");
            Console.WriteLine("[17]  Divide Volume");

            Console.WriteLine("\nOTHER");
            Console.WriteLine("[18]  View Operation History");

            Console.WriteLine("\n[0]   Exit Application");

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Enter the number of the operation you want to perform.");
        }
    }
}