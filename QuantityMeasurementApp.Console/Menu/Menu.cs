using QuantityMeasurementApp.ConsoleApp.Controllers;
using QuantityMeasurementApp.ConsoleApp.Interfaces;

namespace QuantityMeasurementApp.ConsoleApp.Menu
{
    // ── UC15: N-Tier Architecture – UI / Menu Layer ───────────────────────
    //
    // The menu is the only part of the app the user directly sees.
    // It only knows how to:
    //   - Print options to the screen
    //   - Read the user's choice
    //   - Call the right controller method
    //
    // It does NOT contain any business logic or data access.
    public class ConsoleMenu : IMenu
    {
        private readonly QuantityMeasurementController _controller;

        // Constructor injection (Dependency Inversion Principle)
        public ConsoleMenu(QuantityMeasurementController controller)
        {
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
        }

        // ── Main loop ─────────────────────────────────────────────────────

        public void Show()
        {
            bool exit = false;

            while (!exit)
            {
                PrintMenu();
                Console.Write("\n  Enter your choice : ");
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("\n  Please enter a number from the menu.");
                    Pause();
                    continue;
                }

                Console.WriteLine();

                switch (choice)
                {
                    // ── COMPARE ──────────────────────────────────────────
                    case 1:  _controller.PerformCompareLength();      break;
                    case 2:  _controller.PerformCompareWeight();      break;
                    case 3:  _controller.PerformCompareVolume();      break;
                    case 4:  _controller.PerformCompareTemperature(); break;

                    // ── CONVERT ──────────────────────────────────────────
                    case 5:  _controller.PerformConvertLength();      break;
                    case 6:  _controller.PerformConvertWeight();      break;
                    case 7:  _controller.PerformConvertVolume();      break;
                    case 8:  _controller.PerformConvertTemperature(); break;

                    // ── ADD ──────────────────────────────────────────────
                    case 9:  _controller.PerformAddLength();          break;
                    case 10: _controller.PerformAddWeight();          break;
                    case 11: _controller.PerformAddVolume();          break;

                    // ── SUBTRACT ─────────────────────────────────────────
                    case 12: _controller.PerformSubtractLength();     break;
                    case 13: _controller.PerformSubtractWeight();     break;
                    case 14: _controller.PerformSubtractVolume();     break;

                    // ── DIVIDE ───────────────────────────────────────────
                    case 15: _controller.PerformDivideLength();       break;
                    case 16: _controller.PerformDivideWeight();       break;
                    case 17: _controller.PerformDivideVolume();       break;

                    // ── OTHER ────────────────────────────────────────────
                    case 18: _controller.ShowHistory();               break;

                    case 0:
                        Console.WriteLine("\n  Thank you for using Quantity Measurement App. Goodbye!");
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("\n  Invalid choice. Please select a number from the menu.");
                        break;
                }

                if (!exit) Pause();
            }
        }

        // ── Print the full menu ───────────────────────────────────────────

        private static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("══════════════════════════════════════════════════");
            Console.WriteLine("        QUANTITY MEASUREMENT APPLICATION           ");
            Console.WriteLine("══════════════════════════════════════════════════");

            Console.WriteLine("\n  COMPARE OPERATIONS");
            Console.WriteLine("  [1]  Compare Length       (FEET, INCHES, YARDS, CM)");
            Console.WriteLine("  [2]  Compare Weight       (KG, GRAM, POUND)");
            Console.WriteLine("  [3]  Compare Volume       (LITRE, ML, GALLON)");
            Console.WriteLine("  [4]  Compare Temperature  (CELSIUS, FAHRENHEIT, KELVIN)");

            Console.WriteLine("\n  CONVERT OPERATIONS");
            Console.WriteLine("  [5]  Convert Length");
            Console.WriteLine("  [6]  Convert Weight");
            Console.WriteLine("  [7]  Convert Volume");
            Console.WriteLine("  [8]  Convert Temperature");

            Console.WriteLine("\n  ADD OPERATIONS");
            Console.WriteLine("  [9]  Add Length");
            Console.WriteLine("  [10] Add Weight");
            Console.WriteLine("  [11] Add Volume");

            Console.WriteLine("\n  SUBTRACT OPERATIONS");
            Console.WriteLine("  [12] Subtract Length");
            Console.WriteLine("  [13] Subtract Weight");
            Console.WriteLine("  [14] Subtract Volume");

            Console.WriteLine("\n  DIVIDE OPERATIONS");
            Console.WriteLine("  [15] Divide Length");
            Console.WriteLine("  [16] Divide Weight");
            Console.WriteLine("  [17] Divide Volume");

            Console.WriteLine("\n  OTHER");
            Console.WriteLine("  [18] View Operation History");
            Console.WriteLine("  [0]  Exit");

            Console.WriteLine("──────────────────────────────────────────────────");
        }

        private static void Pause()
        {
            Console.WriteLine("\n  (Press ENTER to continue...)");
            Console.ReadLine();
        }
    }
}