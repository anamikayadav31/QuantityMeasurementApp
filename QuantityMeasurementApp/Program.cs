using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    /// <summary>
/// Console application to demonstrate operations on <see cref="QuantityLength"/> objects.
/// 
/// Features:
/// 1. Reads two length quantities from user input, including value and unit.
/// 2. Converts the first quantity to all supported units (Feet, Inches, Yards, Centimeters).
/// 3. Checks equality between the two quantities, accounting for unit conversions.
/// 4. Adds the quantities:
///    - Using the first quantity’s unit (default).
///    - Using a user-selected target unit.
/// 5. Displays the sum in all units for clarity.
/// 6. Handles invalid input and exceptions gracefully.
/// </summary>
    class Program
    {
        static void Main()
        {
            try
            {
                var service = new QuantityMeasurementService();

                // Read two quantities from the user
                QuantityLength q1 = ReadQuantity("First");
                QuantityLength q2 = ReadQuantity("Second");

                // ----- Conversion of first value to all units -----
                Console.WriteLine("\n--- Conversions of First Value ---");
                Console.WriteLine($"Feet: {q1.ConvertTo(LengthUnit.FEET)}");
                Console.WriteLine($"Inches: {q1.ConvertTo(LengthUnit.INCHES)}");
                Console.WriteLine($"Yards: {q1.ConvertTo(LengthUnit.YARDS)}");
                Console.WriteLine($"Centimeters: {q1.ConvertTo(LengthUnit.CENTIMETERS)}");

                // ----- Equality Check -----
                Console.WriteLine("\n--- Equality Check ---");
                bool areEqual = service.AreEqual(q1, q2);
                Console.WriteLine($"Are Equal? {areEqual}");

                // ----- Addition in default unit (first quantity's unit) -----
                Console.WriteLine("\n--- Addition (Default Unit) ---");
                QuantityLength sumDefault = service.Add(q1, q2); // returns in q1's unit
                Console.WriteLine($"{q1} + {q2} = {sumDefault}");

                // ----- Addition in a specific target unit -----
                Console.WriteLine("\nChoose Target Unit for Sum:");
                Console.WriteLine("1 - Feet");
                Console.WriteLine("2 - Inches");
                Console.WriteLine("3 - Yards");
                Console.WriteLine("4 - Centimeters");
                Console.Write("Choice: ");
                int targetChoice = Convert.ToInt32(Console.ReadLine());

                LengthUnit targetUnit = targetChoice switch
                {
                    1 => LengthUnit.FEET,
                    2 => LengthUnit.INCHES,
                    3 => LengthUnit.YARDS,
                    4 => LengthUnit.CENTIMETERS,
                    _ => throw new ArgumentException("Invalid target unit")
                };

                // Call Add as an instance method on q1
                QuantityLength sumInTarget = q1.Add(q2, targetUnit);
                Console.WriteLine($"\nSum in {targetUnit}: {sumInTarget}");

                // ----- Show sum in all units for clarity -----
                Console.WriteLine("\n--- Sum in All Units ---");
                Console.WriteLine($"Feet: {sumInTarget.ConvertTo(LengthUnit.FEET)}");
                Console.WriteLine($"Inches: {sumInTarget.ConvertTo(LengthUnit.INCHES)}");
                Console.WriteLine($"Yards: {sumInTarget.ConvertTo(LengthUnit.YARDS)}");
                Console.WriteLine($"Centimeters: {sumInTarget.ConvertTo(LengthUnit.CENTIMETERS)}");
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

        // Reads a quantity from the user
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