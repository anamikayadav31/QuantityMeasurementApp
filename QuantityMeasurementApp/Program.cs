using System;
using QuantityMeasurementApp.Models;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main()
        {
            try
            {
                // ================= LENGTH OPERATIONS =================
                // Read two length quantities from user
                var q1 = ReadLength("First");
                var q2 = ReadLength("Second");

                // Show first value converted into all length units
                Console.WriteLine("\n--- First Value in All Units ---");
                ShowLengthConversions(q1);

                // ---------- Addition ----------
                Console.WriteLine("\n--- Addition ---");

                // Add two length quantities
                var sum = q1.Add(
                    q2,
                    (u, v) => u.ConvertToBaseUnit(v),     // convert unit to base unit
                    (u, v) => u.ConvertFromBaseUnit(v)   // convert base unit back to original unit
                );

                Console.WriteLine("Sum: " + sum);

                // ---------- Subtraction ----------
                Console.WriteLine("\n--- Subtraction ---");

                // Subtract second length from first length
                var difference = q1.Subtract(
                    q2,
                    (u, v) => u.ConvertToBaseUnit(v),
                    (u, v) => u.ConvertFromBaseUnit(v)
                );

                Console.WriteLine("Difference: " + difference);

                // ---------- Division ----------
                Console.WriteLine("\n--- Division ---");

                // Divide first length by second length
                double ratio = q1.Divide(
                    q2,
                    (u, v) => u.ConvertToBaseUnit(v)
                );

                Console.WriteLine("Division Result: " + ratio);


                // ================= WEIGHT OPERATIONS =================
                Console.WriteLine("\n------ WEIGHT OPERATIONS ------");

                // Read two weight values
                var w1 = ReadWeight("First");
                var w2 = ReadWeight("Second");

                // Show conversions for first weight
                Console.WriteLine("\n--- First Weight in All Units ---");
                ShowWeightConversions(w1);

                // ---------- Addition ----------
                Console.WriteLine("\n--- Addition ---");

                var weightSum = w1.Add(
                    w2,
                    (u, v) => u.ConvertToBaseUnit(v),
                    (u, v) => u.ConvertFromBaseUnit(v)
                );

                Console.WriteLine("Sum: " + weightSum);

                // ---------- Subtraction ----------
                Console.WriteLine("\n--- Subtraction ---");

                var weightDifference = w1.Subtract(
                    w2,
                    (u, v) => u.ConvertToBaseUnit(v),
                    (u, v) => u.ConvertFromBaseUnit(v)
                );

                Console.WriteLine("Difference: " + weightDifference);

                // ---------- Division ----------
                Console.WriteLine("\n--- Division ---");

                double weightRatio = w1.Divide(
                    w2,
                    (u, v) => u.ConvertToBaseUnit(v)
                );

                Console.WriteLine("Division Result: " + weightRatio);


                // ================= VOLUME OPERATIONS =================
                Console.WriteLine("\n------ VOLUME OPERATIONS ------");

                // Read two volume values
                var v1 = ReadVolume("First");
                var v2 = ReadVolume("Second");

                // Show conversions for first volume
                Console.WriteLine("\n--- First Volume in All Units ---");
                ShowVolumeConversions(v1);

                // ---------- Addition ----------
                Console.WriteLine("\n--- Addition ---");

                var volumeSum = v1.Add(
                    v2,
                    (u, val) => u.ConvertToBaseUnit(val),
                    (u, val) => u.ConvertFromBaseUnit(val)
                );

                Console.WriteLine("Sum: " + volumeSum);

                // ---------- Subtraction ----------
                Console.WriteLine("\n--- Subtraction ---");

                var volumeDifference = v1.Subtract(
                    v2,
                    (u, val) => u.ConvertToBaseUnit(val),
                    (u, val) => u.ConvertFromBaseUnit(val)
                );

                Console.WriteLine("Difference: " + volumeDifference);

                // ---------- Division ----------
                Console.WriteLine("\n--- Division ---");

                double volumeRatio = v1.Divide(
                    v2,
                    (u, val) => u.ConvertToBaseUnit(val)
                );

                Console.WriteLine("Division Result: " + volumeRatio);
            }
            catch (Exception ex)
            {
                // If any error occurs, print the message
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // ================= LENGTH METHODS =================

        // Reads a length value and unit from the user
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

            // Determine which unit user selected
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

            // Create and return quantity object
            return new Quantity<LengthUnit>(value, unit);
        }

        // Shows conversions of a length into all supported units
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

        // ================= WEIGHT METHODS =================

        // Reads weight value and unit from user
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

        // Shows conversions for weight
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

        // ================= VOLUME METHODS =================

        // Reads volume value and unit from user
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

        // Shows conversions for volume
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