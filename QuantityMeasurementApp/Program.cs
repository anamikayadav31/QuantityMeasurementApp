using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var service = new QuantityMeasurementService();

                // ----- FIRST VALUE -----
                QuantityLength quantity1 = ReadQuantity("first");

                // ----- SECOND VALUE -----
                QuantityLength quantity2 = ReadQuantity("second");

                // ----- Demonstrate Conversion -----
                Console.WriteLine("\nDemonstrating Conversions:");
                QuantityLength.DemonstrateLengthConversion(quantity1, LengthUnit.FEET);
                QuantityLength.DemonstrateLengthConversion(quantity1, LengthUnit.INCHES);
                QuantityLength.DemonstrateLengthConversion(quantity1, LengthUnit.YARDS);
                QuantityLength.DemonstrateLengthConversion(quantity1, LengthUnit.CENTIMETERS);

                // ----- Compare -----
                Console.WriteLine("\nComparing Values:");
                bool result = service.AreEqual(quantity1, quantity2);
                QuantityLength.DemonstrateLengthEquality(quantity1, quantity2);
                QuantityLength.DemonstrateLengthComparison(quantity1, quantity2);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter numeric values only.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Reads a QuantityLength from user input
        /// </summary>
        static QuantityLength ReadQuantity(string order)
        {
            Console.WriteLine($"\nSelect Unit for {order} value:");
            Console.WriteLine("1. Feet");
            Console.WriteLine("2. Inches");
            Console.WriteLine("3. Yards");
            Console.WriteLine("4. Centimeters");
            Console.Write("Enter choice (1-4): ");
            int unitChoice = Convert.ToInt32(Console.ReadLine());

            Console.Write($"Enter {order} value: ");
            double value = Convert.ToDouble(Console.ReadLine());

            LengthUnit unit = GetUnit(unitChoice);
            return new QuantityLength(value, unit);
        }

        /// <summary>
        /// Maps numeric choice to LengthUnit
        /// </summary>
        static LengthUnit GetUnit(int choice)
        {
            return choice switch
            {
                1 => LengthUnit.FEET,
                2 => LengthUnit.INCHES,
                3 => LengthUnit.YARDS,
                4 => LengthUnit.CENTIMETERS,
                _ => throw new ArgumentException("Invalid unit selection.")
            };
        }
    }
}