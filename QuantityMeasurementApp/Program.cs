using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main()
        {
            try
            {
                var service = new QuantityMeasurementService();

                // Read two quantities
                QuantityLength q1 = ReadQuantity("First");
                QuantityLength q2 = ReadQuantity("Second");

                // Convert first value to all units
                Console.WriteLine("\n--- Conversions of First Value ---");
                Console.WriteLine($"Feet: {QuantityLength.Convert(q1.Value, q1.Unit, LengthUnit.FEET)}");
                Console.WriteLine($"Inches: {QuantityLength.Convert(q1.Value, q1.Unit, LengthUnit.INCHES)}");
                Console.WriteLine($"Yards: {QuantityLength.Convert(q1.Value, q1.Unit, LengthUnit.YARDS)}");
                Console.WriteLine($"Centimeters: {QuantityLength.Convert(q1.Value, q1.Unit, LengthUnit.CENTIMETERS)}");

                // Equality check
                Console.WriteLine("\n--- Equality Check ---");
                Console.WriteLine($"Are Equal? {service.AreEqual(q1, q2)}");

                // Addition
                Console.WriteLine("\n--- Addition ---");
                QuantityLength sum = service.Add(q1, q2);
                Console.WriteLine($"{q1} + {q2} = {sum}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static QuantityLength ReadQuantity(string label)
        {
            Console.WriteLine($"\nEnter {label} Value:");

            Console.Write("Value: ");
            double value = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Choose Unit:");
            Console.WriteLine("1 - Feet");
            Console.WriteLine("2 - Inches");
            Console.WriteLine("3 - Yards");
            Console.WriteLine("4 - Centimeters");
            Console.Write("Choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            LengthUnit unit = choice switch
            {
                1 => LengthUnit.FEET,
                2 => LengthUnit.INCHES,
                3 => LengthUnit.YARDS,
                4 => LengthUnit.CENTIMETERS,
                _ => throw new ArgumentException("Invalid unit selection")
            };

            return new QuantityLength(value, unit);
        }
    }
}