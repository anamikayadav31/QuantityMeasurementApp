namespace QuantityMeasurementApp.Model
{
    // Represents measurement in Inches
    public class Inches
    {
        private readonly double value;

        public Inches(double value)
        {
            // Validate numeric input
            if (double.IsNaN(value))
                throw new ArgumentException("Invalid numeric value");

            this.value = value;
        }

        public double Value => value;

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj is null || obj.GetType() != typeof(Inches))
                return false;

            Inches other = (Inches)obj;

            return this.value.CompareTo(other.value) == 0;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}