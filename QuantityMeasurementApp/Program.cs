using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Main class of the application.
    /// This program takes length and weight input from user
    /// and performs conversion and addition using generic Quantity<T>.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point of the program
        /// </summary>
        static void Main()
        {
            try
            {
                // -------- LENGTH OPERATIONS --------

                // Read two length quantities from user
                var q1 = ReadLength("First");
                var q2 = ReadLength("Second");

                Console.WriteLine("\n--- First Value in All Units ---");
                ShowLengthConversions(q1);

                Console.WriteLine("\n--- Addition ---");

                // Add two length quantities
                var sum = q1.Add(
                    q2,
                    (u, v) => u.ConvertToBaseUnit(v),
                    (u, v) => u.ConvertFromBaseUnit(v)
                );

                Console.WriteLine("Sum: " + sum);

                // -------- WEIGHT OPERATIONS --------

                Console.WriteLine("\n------WEIGHT OPERATIONS------");

                // Read two weight quantities
                var w1 = ReadWeight("First");
                var w2 = ReadWeight("Second");

                Console.WriteLine("\n--- First Weight in All Units ---");
                ShowWeightConversions(w1);

                Console.WriteLine("\n--- Addition ---");

                // Add two weight quantities
                var weightSum = w1.Add(
                    w2,
                    (u, v) => u.ConvertToBaseUnit(v),
                    (u, v) => u.ConvertFromBaseUnit(v)
                );

                Console.WriteLine("Sum: " + weightSum);
            }
            catch (Exception ex)
            {
                // Show error if invalid input occurs
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Method to read length value and unit from user
        /// </summary>
        static Quantity<LengthUnit> ReadLength(string name)
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

            // Using if-else instead of ternary operator
            if (choice == 1)
            {
                unit = LengthUnit.FEET;
            }
            else if (choice == 2)
            {
                unit = LengthUnit.INCHES;
            }
            else if (choice == 3)
            {
                unit = LengthUnit.YARDS;
            }
            else if (choice == 4)
            {
                unit = LengthUnit.CENTIMETERS;
            }
            else
            {
                throw new ArgumentException("Invalid unit");
            }

            return new Quantity<LengthUnit>(value, unit);
        }

        /// <summary>
        /// Displays the length value in all units
        /// </summary>
        static void ShowLengthConversions(Quantity<LengthUnit> q)
        {
            Console.WriteLine("Feet: " +
                q.ConvertTo((u, v) => u.ConvertFromBaseUnit(v),
                            (u, v) => u.ConvertToBaseUnit(v),
                            LengthUnit.FEET));

            Console.WriteLine("Inches: " +
                q.ConvertTo((u, v) => u.ConvertFromBaseUnit(v),
                            (u, v) => u.ConvertToBaseUnit(v),
                            LengthUnit.INCHES));

            Console.WriteLine("Yards: " +
                q.ConvertTo((u, v) => u.ConvertFromBaseUnit(v),
                            (u, v) => u.ConvertToBaseUnit(v),
                            LengthUnit.YARDS));

            Console.WriteLine("Centimeters: " +
                q.ConvertTo((u, v) => u.ConvertFromBaseUnit(v),
                            (u, v) => u.ConvertToBaseUnit(v),
                            LengthUnit.CENTIMETERS));
        }

        /// <summary>
        /// Method to read weight value and unit from user
        /// </summary>
        static Quantity<WeightUnit> ReadWeight(string name)
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

            // Using if-else for selecting unit
            if (choice == 1)
            {
                unit = WeightUnit.KILOGRAM;
            }
            else if (choice == 2)
            {
                unit = WeightUnit.GRAM;
            }
            else if (choice == 3)
            {
                unit = WeightUnit.POUND;
            }
            else
            {
                throw new ArgumentException("Invalid unit");
            }

            return new Quantity<WeightUnit>(value, unit);
        }

        /// <summary>
        /// Displays the weight value in all units
        /// </summary>
        static void ShowWeightConversions(Quantity<WeightUnit> w)
        {
            Console.WriteLine("Kilogram: " +
                w.ConvertTo((u, v) => u.ConvertFromBaseUnit(v),
                            (u, v) => u.ConvertToBaseUnit(v),
                            WeightUnit.KILOGRAM));

            Console.WriteLine("Gram: " +
                w.ConvertTo((u, v) => u.ConvertFromBaseUnit(v),
                            (u, v) => u.ConvertToBaseUnit(v),
                            WeightUnit.GRAM));

            Console.WriteLine("Pound: " +
                w.ConvertTo((u, v) => u.ConvertFromBaseUnit(v),
                            (u, v) => u.ConvertToBaseUnit(v),
                            WeightUnit.POUND));
        }
    }
}