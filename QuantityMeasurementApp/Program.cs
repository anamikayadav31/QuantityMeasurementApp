using System;
using QuantityMeasurementApp.Model;
using QuantityMeasurementApp.Service;

namespace QuantityMeasurementApp
{
    // entry point of the application.
   
    class Program
    {
        static void Main(string[] args)
        {
            // Ask user to enter first value.
            Console.WriteLine("Enter first value in feet:");
            
            // Read input from console
            double input1 = Convert.ToDouble(Console.ReadLine());

            // Ask user to enter second value.
            Console.WriteLine("Enter second value in feet:");
            
            // Read second input 
            double input2 = Convert.ToDouble(Console.ReadLine());

            // Create Feet objects
            Feet feet1 = new Feet(input1);
            Feet feet2 = new Feet(input2);

            // Create object of Service class.
            FeetService service = new FeetService();

            // Call service method to check equality.
            bool result = service.AreEqual(feet1, feet2);

            // Display result to the user.
            Console.WriteLine("Equal: " + result);
        }
    }
}