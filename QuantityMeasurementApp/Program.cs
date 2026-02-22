using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    // Entry point of the Quantity Measurement Application
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
                Console.Write("Enter choice (1 or 2): ");
                int unitChoice1 = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter first value: ");
                double input1 = Convert.ToDouble(Console.ReadLine());

                // ----- SECOND VALUE -----
                Console.WriteLine("\nSelect Unit for Second Value:");
                Console.WriteLine("1. Feet");
                Console.WriteLine("2. Inches");
                Console.Write("Enter choice (1 or 2): ");
                int unitChoice2 = Convert.ToInt32(Console.ReadLine());

                Console.Write("Enter second value: ");
                double input2 = Convert.ToDouble(Console.ReadLine());

                // Convert user choice to LengthUnit enum
                LengthUnit unit1 = unitChoice1 == 1 ? LengthUnit.FEET :
                                   unitChoice1 == 2 ? LengthUnit.INCH :
                                   throw new ArgumentException("Invalid unit selection for first value.");

                LengthUnit unit2 = unitChoice2 == 1 ? LengthUnit.FEET :
                                   unitChoice2 == 2 ? LengthUnit.INCH :
                                   throw new ArgumentException("Invalid unit selection for second value.");

                // Create Quantity objects
                QuantityLength quantity1 = new QuantityLength(input1, unit1);
                QuantityLength quantity2 = new QuantityLength(input2, unit2);

                // Compare using single service method
                bool result = service.AreEqual(quantity1, quantity2);

                Console.WriteLine("\nComparing...");
                Console.WriteLine($"First:  {quantity1}");
                Console.WriteLine($"Second: {quantity2}");
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
    }
}