using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp
{
    /// <summary>
    /// Main class of the application.
    /// This program reads Length, Weight and Volume values from the user,
    /// performs unit conversions, and adds two quantities using generic Quantity<T>.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Application entry point.
        /// Handles all operations: Length, Weight and Volume.
        /// </summary>
        static void Main()
        {
            try
            {
                // ---------------- LENGTH OPERATIONS ----------------

                // Read two length values from the user
                var q1 = ReadLength("First");
                var q2 = ReadLength("Second");

                // Show the first length in all possible units
                Console.WriteLine("\n--- First Value in All Units ---");
                ShowLengthConversions(q1);

                // Add the two length quantities
                Console.WriteLine("\n--- Addition ---");

                var sum = q1.Add(
                    q2,
                    (u, v) => u.ConvertToBaseUnit(v),     // Convert both values to base unit
                    (u, v) => u.ConvertFromBaseUnit(v)    // Convert result back to original unit
                );

                Console.WriteLine("Sum: " + sum);

                // ---------------- WEIGHT OPERATIONS ----------------

                Console.WriteLine("\n------WEIGHT OPERATIONS------");

                // Read two weight values
                var w1 = ReadWeight("First");
                var w2 = ReadWeight("Second");

                // Show weight conversions
                Console.WriteLine("\n--- First Weight in All Units ---");
                ShowWeightConversions(w1);

                // Add the weights
                Console.WriteLine("\n--- Addition ---");

                var weightSum = w1.Add(
                    w2,
                    (u, v) => u.ConvertToBaseUnit(v),
                    (u, v) => u.ConvertFromBaseUnit(v)
                );

                Console.WriteLine("Sum: " + weightSum);

                // ---------------- VOLUME OPERATIONS ----------------

                Console.WriteLine("\n------VOLUME OPERATIONS------");

                // Read two volume values
                var v1 = ReadVolume("First");
                var v2 = ReadVolume("Second");

                // Show conversions
                Console.WriteLine("\n--- First Volume in All Units ---");
                ShowVolumeConversions(v1);

                // Add volume quantities
                Console.WriteLine("\n--- Addition ---");

                var volumeSum = v1.Add(
                    v2,
                    (u, v) => u.ConvertToBaseUnit(v),
                    (u, v) => u.ConvertFromBaseUnit(v)
                );

                Console.WriteLine("Sum: " + volumeSum);
            }
            catch (Exception ex)
            {
                // Catch and display any runtime errors
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // ======================================================
        // LENGTH METHODS
        // ======================================================

        /// <summary>
        /// Reads a length value and unit from the user.
        /// </summary>
        static Quantity<LengthUnit> ReadLength(string name)
        {
            Console.WriteLine($"\nEnter {name} Length Value:");

            // Read numeric value
            Console.Write("Value: ");
            double value = Convert.ToDouble(Console.ReadLine());

            // Ask user to select unit
            Console.WriteLine("Select Unit:");
            Console.WriteLine("1 - Feet");
            Console.WriteLine("2 - Inches");
            Console.WriteLine("3 - Yards");
            Console.WriteLine("4 - Centimeters");

            Console.Write("Choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            LengthUnit unit;

            // Map user choice to unit enum
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

            // Return quantity object
            return new Quantity<LengthUnit>(value, unit);
        }

        /// <summary>
        /// Displays the given length in all supported units.
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

        // ======================================================
        // WEIGHT METHODS
        // ======================================================

        /// <summary>
        /// Reads a weight value and unit from the user.
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

            if (choice == 1)
                unit = WeightUnit.KILOGRAM;
            else if (choice == 2)
                unit = WeightUnit.GRAM;
            else if (choice == 3)
                unit = WeightUnit.POUND;
            else
                throw new ArgumentException("Invalid unit");

            return new Quantity<WeightUnit>(value, unit);
        }

        /// <summary>
        /// Displays weight in all supported units.
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

        // ======================================================
        // VOLUME METHODS
        // ======================================================

        /// <summary>
        /// Reads a volume value and unit from the user.
        /// </summary>
        static Quantity<VolumeUnit> ReadVolume(string name)
        {
            Console.WriteLine($"\nEnter {name} Volume Value:");

            Console.Write("Value: ");
            double value = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Select Unit:");
            Console.WriteLine("1 - Litre");
            Console.WriteLine("2 - Millilitre");
            Console.WriteLine("3 - Gallon");

            Console.Write("Choice: ");
            int choice = Convert.ToInt32(Console.ReadLine());

            VolumeUnit unit;

            if (choice == 1)
                unit = VolumeUnit.LITRE;
            else if (choice == 2)
                unit = VolumeUnit.MILLILITRE;
            else if (choice == 3)
                unit = VolumeUnit.GALLON;
            else
                throw new ArgumentException("Invalid unit");

            return new Quantity<VolumeUnit>(value, unit);
        }

        /// <summary>
        /// Displays volume in all supported units.
        /// </summary>
        static void ShowVolumeConversions(Quantity<VolumeUnit> v)
        {
            Console.WriteLine("Litre: " +
                v.ConvertTo((u, val) => u.ConvertFromBaseUnit(val),
                            (u, val) => u.ConvertToBaseUnit(val),
                            VolumeUnit.LITRE));

            Console.WriteLine("Millilitre: " +
                v.ConvertTo((u, val) => u.ConvertFromBaseUnit(val),
                            (u, val) => u.ConvertToBaseUnit(val),
                            VolumeUnit.MILLILITRE));

            Console.WriteLine("Gallon: " +
                v.ConvertTo((u, val) => u.ConvertFromBaseUnit(val),
                            (u, val) => u.ConvertToBaseUnit(val),
                            VolumeUnit.GALLON));
        }
    }
}