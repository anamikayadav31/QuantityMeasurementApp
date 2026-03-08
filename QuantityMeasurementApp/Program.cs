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

                // -------- LENGTH OPERATIONS --------

                QuantityLength q1 = ReadQuantity("First");
                QuantityLength q2 = ReadQuantity("Second");

                Console.WriteLine("\n--- First Value in All Units ---");
                ShowConversions(q1);

                Console.WriteLine("\n--- Equality Check ---");
                bool equal = service.AreEqual(q1, q2);
                Console.WriteLine("Are Equal? " + equal);

                Console.WriteLine("\n--- Addition ---");
                QuantityLength sum = service.Add(q1, q2);
                Console.WriteLine("Sum: " + sum);

                Console.WriteLine("\n--- Subtraction ---");
                QuantityLength diff = service.Subtract(q1, q2);
                Console.WriteLine("Difference: " + diff);

                Console.WriteLine("\n--- Multiplication ---");
                Console.Write("Enter number to multiply: ");
                double number = Convert.ToDouble(Console.ReadLine());

                QuantityLength result = service.Multiply(q1, number);
                Console.WriteLine("Result: " + result);

                // -------- WEIGHT OPERATIONS (UC9) --------

               
                Console.WriteLine("------WEIGHT OPERATIONS------");
               

                QuantityWeight w1 = ReadWeight("First");
                QuantityWeight w2 = ReadWeight("Second");

                Console.WriteLine("\n--- First Weight in All Units ---");
                ShowWeightConversions(w1);

                Console.WriteLine("\n--- Equality Check ---");
                Console.WriteLine("Are Equal? " + w1.Equals(w2));

                Console.WriteLine("\n--- Addition ---");
                QuantityWeight weightSum = w1.Add(w2);
                Console.WriteLine("Sum: " + weightSum);

                Console.WriteLine("\n--- Conversion Example ---");
                Console.WriteLine("First weight in Grams: " + w1.ConvertTo(WeightUnit.GRAM));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Method to read length quantity from user
        /// </summary>
        static QuantityLength ReadQuantity(string name)
        {
            Console.WriteLine($"\nEnter {name} Length Value:");

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
        /// Show length value in all units
        /// </summary>
        static void ShowConversions(QuantityLength q)
        {
            Console.WriteLine("Feet: " + q.ConvertTo(LengthUnit.FEET));
            Console.WriteLine("Inches: " + q.ConvertTo(LengthUnit.INCHES));
            Console.WriteLine("Yards: " + q.ConvertTo(LengthUnit.YARDS));
            Console.WriteLine("Centimeters: " + q.ConvertTo(LengthUnit.CENTIMETERS));
        }

        // ---------------- WEIGHT METHODS ----------------

        /// <summary>
        /// Method to read weight from user
        /// </summary>
        static QuantityWeight ReadWeight(string name)
        {
            Console.WriteLine($"\nEnter {name} Weight Value:");

            Console.Write("Value: ");
            double value = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Select Unit:");
            Console.WriteLine("1 - Kilogram");
            Console.WriteLine("2 - Gram");
            Console.WriteLine("3 - Pound");

            Console.Write("Choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            WeightUnit unit;

            if (choice == 1)
                unit = WeightUnit.KILOGRAM;
            else if (choice == 2)
                unit = WeightUnit.GRAM;
            else if (choice == 3)
                unit = WeightUnit.POUND;
            else
                throw new ArgumentException("Invalid unit");

            return new QuantityWeight(value, unit);
        }

        /// <summary>
        /// Show weight value in all units
        /// </summary>
        static void ShowWeightConversions(QuantityWeight w)
        {
            Console.WriteLine("Kilogram: " + w.ConvertTo(WeightUnit.KILOGRAM));
            Console.WriteLine("Gram: " + w.ConvertTo(WeightUnit.GRAM));
            Console.WriteLine("Pound: " + w.ConvertTo(WeightUnit.POUND));
        }
    }
}