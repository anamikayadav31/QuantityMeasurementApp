using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Main program to test Quantity Measurement operations.
    /// </summary>
    class Program
    {
        static void Main()
        {
            try
            {
                var service = new QuantityMeasurementService();

                // Read two quantities from user
                QuantityLength q1 = ReadQuantity("First");
                QuantityLength q2 = ReadQuantity("Second");

                // Show conversions
                Console.WriteLine("\n--- First Value in All Units ---");
                ShowConversions(q1);

                // Equality
                Console.WriteLine("\n--- Equality Check ---");
                bool equal = service.AreEqual(q1, q2);
                Console.WriteLine("Are Equal? " + equal);

                // Addition
                Console.WriteLine("\n--- Addition ---");
                QuantityLength sum = service.Add(q1, q2);
                Console.WriteLine("Sum: " + sum);

                // Subtraction
                Console.WriteLine("\n--- Subtraction ---");
                QuantityLength diff = service.Subtract(q1, q2);
                Console.WriteLine("Difference: " + diff);

                // Multiplication
                Console.WriteLine("\n--- Multiplication ---");
                Console.Write("Enter number to multiply: ");
                double number = Convert.ToDouble(Console.ReadLine());

                QuantityLength result = service.Multiply(q1, number);
                Console.WriteLine("Result: " + result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Method to read quantity from user
        /// </summary>
        static QuantityLength ReadQuantity(string name)
        {
            Console.WriteLine($"\nEnter {name} Value:");

            Console.Write("Value: ");
            double value = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Select Unit:");
            Console.WriteLine("1 - Feet");
            Console.WriteLine("2 - Inches");
            Console.WriteLine("3 - Yards");
            Console.WriteLine("4 - Centimeters");

            Console.Write("Choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            LengthUnit unit;

            if (choice == 1)
                unit = LengthUnit.FEET;
            else if (choice == 2)
                unit = LengthUnit.INCHES;
            else if (choice == 3)
                unit = LengthUnit.YARDS;
            else if (choice == 4)
                unit = LengthUnit.CENTIMETERS;
            else
                throw new ArgumentException("Invalid unit");

            return new QuantityLength(value, unit);
        }

        /// <summary>
        /// Show value in all units
        /// </summary>
        static void ShowConversions(QuantityLength q)
        {
            Console.WriteLine("Feet: " + q.ConvertTo(LengthUnit.FEET));
            Console.WriteLine("Inches: " + q.ConvertTo(LengthUnit.INCHES));
            Console.WriteLine("Yards: " + q.ConvertTo(LengthUnit.YARDS));
            Console.WriteLine("Centimeters: " + q.ConvertTo(LengthUnit.CENTIMETERS));
        }
    }
}