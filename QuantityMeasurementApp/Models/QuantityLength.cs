using System;

namespace QuantityMeasurementApp.Models
{
   

    public class QuantityLength
    {
        // Immutable numeric value of the length
        public double Value { get; }

        // Immutable unit of the length
        public LengthUnit Unit { get; }

        // Constructor
        public QuantityLength(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            Value = value;
            Unit = unit;
        }

        // Convert this instance to target unit and return new QuantityLength
        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            double baseValue = ToFeet();  // Convert current value to base unit (Feet)

            double convertedValue = targetUnit switch
            {
                LengthUnit.FEET => baseValue,
                LengthUnit.INCHES => baseValue * 12,
                LengthUnit.YARDS => baseValue / 3,
                LengthUnit.CENTIMETERS => baseValue * 30.48,
                _ => throw new ArgumentException("Unsupported target unit")
            };

            return new QuantityLength(convertedValue, targetUnit);
        }

        // Private helper: Convert current value to Feet
        private double ToFeet()
        {
            return Unit switch
            {
                LengthUnit.FEET => Value,
                LengthUnit.INCHES => Value / 12,
                LengthUnit.YARDS => Value * 3,
                LengthUnit.CENTIMETERS => Value / 30.48,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }

        // Static method: Convert a raw value between two units
        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid numeric value");

            var temp = new QuantityLength(value, source);
            return temp.ConvertTo(target).Value;
        }

        // Override Equals to compare two QuantityLength objects in base unit
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is not QuantityLength other)
                return false;

            return Math.Abs(this.ToFeet() - other.ToFeet()) < 0.0001; // Small tolerance
        }

        public override int GetHashCode()
        {
            return ToFeet().GetHashCode();
        }

        public override string ToString()
        {
            return $"{Value:F2} {Unit}";
        }

        // ---------- Demonstration Methods ----------

        // Overloaded method: Convert raw numeric value between units
        public static void DemonstrateLengthConversion(double value, LengthUnit fromUnit, LengthUnit toUnit)
        {
            double converted = Convert(value, fromUnit, toUnit);
            Console.WriteLine($"{value} {fromUnit} = {converted:F2} {toUnit}");
        }

        // Overloaded method: Convert an existing QuantityLength object to target unit
        public static void DemonstrateLengthConversion(QuantityLength length, LengthUnit toUnit)
        {
            QuantityLength converted = length.ConvertTo(toUnit);
            Console.WriteLine($"{length.Value} {length.Unit} = {converted.Value:F2} {converted.Unit}");
        }

        // Demonstrate equality between two QuantityLength objects
        public static void DemonstrateLengthEquality(QuantityLength length1, QuantityLength length2)
        {
            Console.WriteLine($"{length1} and {length2} are " + (length1.Equals(length2) ? "equal" : "not equal"));
        }

        // Demonstrate comparison (less than, greater than)
        public static void DemonstrateLengthComparison(QuantityLength length1, QuantityLength length2)
        {
            double val1 = length1.ConvertTo(LengthUnit.FEET).Value;
            double val2 = length2.ConvertTo(LengthUnit.FEET).Value;

            if (val1 < val2)
                Console.WriteLine($"{length1} is less than {length2}");
            else if (val1 > val2)
                Console.WriteLine($"{length1} is greater than {length2}");
            else
                Console.WriteLine($"{length1} is equal to {length2}");
        }
    }
}