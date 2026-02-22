using System;
using QuantityMeasurementApp.Service;

namespace QuantityMeasurementApp
{
    // Entry point of the Quantity Measurement Application
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Display unit options to the user
                Console.WriteLine("Select Unit Type:");
                Console.WriteLine("1. Feet");
                Console.WriteLine("2. Inches");
                Console.Write("Enter choice (1 or 2): ");

                // Read user choice and convert to integer
                int choice = Convert.ToInt32(Console.ReadLine());

                // Ask user to enter first value
                Console.WriteLine("Enter first value:");
                double input1 = Convert.ToDouble(Console.ReadLine());

                // Ask user to enter second value
                Console.WriteLine("Enter second value:");
                double input2 = Convert.ToDouble(Console.ReadLine());

                // Variable to store comparison result
                bool result = false;

                // Call appropriate static service method based on user choice
                if (choice == 1)
                {
                    // Compare values in Feet
                    result = QuantityMeasurementService.AreEqual(input1, input2);
                }
                else if (choice == 2)
                {
                    // Compare values in Inches
                    result = QuantityMeasurementService.AreInchesEqual(input1, input2);
                }
                else
                {
                    // Handle invalid unit selection
                    Console.WriteLine("Invalid choice.");
                    return; // Exit the program
                }

                // Display final comparison result
                Console.WriteLine("Equal: " + result);
            }
            catch (FormatException)
            {
                // Handle invalid numeric input
                Console.WriteLine("Invalid input. Please enter numeric values only.");
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}