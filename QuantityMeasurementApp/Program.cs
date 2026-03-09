using System;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp
{
    // Main class of the application
    class Program
    {
        // Entry point of the console application
        static void Main()
        {
            // Creating object of service class which performs operations
            var service = new QuantityMeasurementService();

            try
            {
                Console.WriteLine("==== Welcome to Quantity Measurement App ====\n");

                // ================= LENGTH =================
                // Section to perform length related operations
                Console.WriteLine(">>> LENGTH OPERATIONS <<<");

                // Reading two length values from the user
                var q1 = ReadLength("First");
                var q2 = ReadLength("Second");

                // Showing first length value in all available units
                Console.WriteLine("\n-- First Length in All Units --");
                ShowLengthConversions(q1);

                // Performing arithmetic operations
                var sumLength = service.Add(q1, q2, LengthUnit.FEET);
                var diffLength = service.Subtract(q1, q2, LengthUnit.FEET);
                double divLength = service.Divide(q1, q2);

                // Displaying results
                Console.WriteLine("\n-- Length Results --");
                Console.WriteLine($"Sum: {sumLength.Value:F3} {sumLength.Unit}");
                Console.WriteLine($"Difference: {diffLength.Value:F3} {diffLength.Unit}");
                Console.WriteLine($"Division Result: {divLength:F3}");

                Console.WriteLine("\n--------------------------------------------");

                // ================= WEIGHT =================
                // Section to perform weight related operations
                Console.WriteLine(">>> WEIGHT OPERATIONS <<<");

                // Reading two weight values from the user
                var w1 = ReadWeight("First");
                var w2 = ReadWeight("Second");

                // Showing first weight value in all units
                Console.WriteLine("\n-- First Weight in All Units --");
                ShowWeightConversions(w1);

                // Performing arithmetic operations
                var sumWeight = service.Add(w1, w2, WeightUnit.KILOGRAM);
                var diffWeight = service.Subtract(w1, w2, WeightUnit.KILOGRAM);
                double divWeight = service.Divide(w1, w2);

                // Displaying results
                Console.WriteLine("\n-- Weight Results --");
                Console.WriteLine($"Sum: {sumWeight.Value:F3} {sumWeight.Unit}");
                Console.WriteLine($"Difference: {diffWeight.Value:F3} {diffWeight.Unit}");
                Console.WriteLine($"Division Result: {divWeight:F3}");

                Console.WriteLine("\n--------------------------------------------");

                // ================= VOLUME =================
                // Section to perform volume related operations
                Console.WriteLine(">>> VOLUME OPERATIONS <<<");

                // Reading two volume values from the user
                var v1 = ReadVolume("First");
                var v2 = ReadVolume("Second");

                // Showing first volume value in all units
                Console.WriteLine("\n-- First Volume in All Units --");
                ShowVolumeConversions(v1);

                // Performing arithmetic operations
                var sumVolume = service.Add(v1, v2, VolumeUnit.LITRE);
                var diffVolume = service.Subtract(v1, v2, VolumeUnit.LITRE);
                double divVolume = service.Divide(v1, v2);

                // Displaying results
                Console.WriteLine("\n-- Volume Results --");
                Console.WriteLine($"Sum: {sumVolume.Value:F3} {sumVolume.Unit}");
                Console.WriteLine($"Difference: {diffVolume.Value:F3} {diffVolume.Unit}");
                Console.WriteLine($"Division Result: {divVolume:F3}");

                Console.WriteLine("\n==== Thank You for Using Quantity Measurement App! ====");
            }

            // Catch block handles runtime errors
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
        }

        // ---------------- LENGTH ----------------

        // Method to read length value and unit from the user
        static Quantity<LengthUnit> ReadLength(string name)
        {
            Console.WriteLine($"\nEnter {name} Length:");

            // Read numeric value
            double value = ReadDouble("Value");

            // Ask user to choose unit
            LengthUnit unit = ReadChoice<LengthUnit>(
                new string[] { "Feet", "Inches", "Yards", "Centimeters" });

            // Create and return Quantity object
            return new Quantity<LengthUnit>(value, unit);
        }

        // Method to display length value converted into all units
        static void ShowLengthConversions(Quantity<LengthUnit> q)
        {
            // Loop through all possible length units
            foreach (LengthUnit u in Enum.GetValues(typeof(LengthUnit)))
            {
                Console.WriteLine($"{u}: {q.ConvertTo((unit, val) =>
                unit.ConvertFromBaseUnit(val), (unit, val) =>
                unit.ConvertToBaseUnit(val), u).Value:F3}");
            }
        }

        // ---------------- WEIGHT ----------------

        // Method to read weight value and unit
        static Quantity<WeightUnit> ReadWeight(string name)
        {
            Console.WriteLine($"\nEnter {name} Weight:");

            double value = ReadDouble("Value");

            // User chooses weight unit
            WeightUnit unit = ReadChoice<WeightUnit>(
                new string[] { "Kilogram", "Gram", "Pound" });

            return new Quantity<WeightUnit>(value, unit);
        }

        // Convert weight into all units
        static void ShowWeightConversions(Quantity<WeightUnit> q)
        {
            foreach (WeightUnit u in Enum.GetValues(typeof(WeightUnit)))
            {
                Console.WriteLine($"{u}: {q.ConvertTo((unit, val) =>
                unit.ConvertFromBaseUnit(val), (unit, val) =>
                unit.ConvertToBaseUnit(val), u).Value:F3}");
            }
        }

        // ---------------- VOLUME ----------------

        // Method to read volume value and unit
        static Quantity<VolumeUnit> ReadVolume(string name)
        {
            Console.WriteLine($"\nEnter {name} Volume:");

            double value = ReadDouble("Value");

            // User chooses volume unit
            VolumeUnit unit = ReadChoice<VolumeUnit>(
                new string[] { "Litre", "Millilitre", "Gallon" });

            return new Quantity<VolumeUnit>(value, unit);
        }

        // Convert volume into all units
        static void ShowVolumeConversions(Quantity<VolumeUnit> q)
        {
            foreach (VolumeUnit u in Enum.GetValues(typeof(VolumeUnit)))
            {
                Console.WriteLine($"{u}: {q.ConvertTo((unit, val) =>
                unit.ConvertFromBaseUnit(val), (unit, val) =>
                unit.ConvertToBaseUnit(val), u).Value:F3}");
            }
        }

        // ---------------- HELPER METHODS ----------------

        // Method to safely read a double number from the user
        static double ReadDouble(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");

                // Try to convert user input into double
                if (double.TryParse(Console.ReadLine(), out double value))
                    return value;

                // If invalid input
                Console.WriteLine("Invalid input! Please enter a number.");
            }
        }

        // Generic method to allow user to choose from options
        static T ReadChoice<T>(string[] options) where T : Enum
        {
            // Display all options
            for (int i = 0; i < options.Length; i++)
                Console.WriteLine($"{i + 1} - {options[i]}");

            while (true)
            {
                Console.Write("Choice: ");

                // Validate user choice
                if (int.TryParse(Console.ReadLine(), out int choice) &&
                    choice >= 1 && choice <= options.Length)

                    return (T)Enum.GetValues(typeof(T)).GetValue(choice - 1);

                Console.WriteLine("Invalid choice! Try again.");
            }
        }
    }
}