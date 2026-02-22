using QuantityMeasurementApp.Model;

namespace QuantityMeasurementApp.Service
{
    // This class contains business logic 
   
    public class FeetService
    {
        // Method to check whether two Feet objects are equal.
       
        public bool AreEqual(Feet feet1, Feet feet2)
        {
            // If either object is null, return false.
            if (feet1 == null || feet2 == null)
                return false;

            // call model's equality logic.
            return feet1.Equals(feet2);
        }
    }
}