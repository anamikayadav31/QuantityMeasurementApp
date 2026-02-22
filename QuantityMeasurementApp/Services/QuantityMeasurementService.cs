using QuantityMeasurementApp.Model;

namespace QuantityMeasurementApp.Service
{
    // This class contains business logic for unit comparison
    public static class QuantityMeasurementService
    {
        // Static method to compare two Feet values
        public static bool AreEqual(double value1, double value2)
        {
            Feet feet1 = new Feet(value1);
            Feet feet2 = new Feet(value2);

            return feet1.Equals(feet2);
        }

        // Static method to compare two Inches values
        public static bool AreInchesEqual(double value1, double value2)
        {
            Inches inch1 = new Inches(value1);
            Inches inch2 = new Inches(value2);

            return inch1.Equals(inch2);
        }
    }
}