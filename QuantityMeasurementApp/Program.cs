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
                Console.WriteLine("Select Unit for First Value:");
                Console.WriteLine("1. Feet");
                Console.WriteLine("2. Inches");
                Console.WriteLine("3. Yards");
                Console.WriteLine("4. Centimeters");
                Console.Write("Enter choice (1-4): ");
                int unitChoice1 = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter first value: ");
                double input1 = Convert.ToDouble(Console.ReadLine());

                // ----- SECOND VALUE -----
                Console.WriteLine("\nSelect Unit for Second Value:");
                Console.WriteLine("1. Feet");
                Console.WriteLine("2. Inches");
                Console.WriteLine("3. Yards");
                Console.WriteLine("4. Centimeters");
                Console.Write("Enter choice (1-4): ");
                int unitChoice2 = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter second value: ");
                double input2 = Convert.ToDouble(Console.ReadLine());

                // Convert user choice to LengthUnit
                LengthUnit unit1 = GetUnit(unitChoice1);
                LengthUnit unit2 = GetUnit(unitChoice2);

                // Create Quantity objects
                QuantityLength quantity1 = new QuantityLength(input1, unit1);
                QuantityLength quantity2 = new QuantityLength(input2, unit2);

                // Compare
                bool result = service.AreEqual(quantity1, quantity2);

                Console.WriteLine("\nComparing...");
                Console.WriteLine($"First:  {input1} {unit1}");
                Console.WriteLine($"Second: {input2} {unit2}");
                Console.WriteLine("Equal: " + result);
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

        // Helper method to map user input to enum
        static LengthUnit GetUnit(int choice)
        {
            return choice switch
            {
                1 => LengthUnit.FEET,
                2 => LengthUnit.INCH,
                3 => LengthUnit.YARDS,
                4 => LengthUnit.CENTIMETERS,
                _ => throw new ArgumentException("Invalid unit selection.")
            };
        }
    }
}