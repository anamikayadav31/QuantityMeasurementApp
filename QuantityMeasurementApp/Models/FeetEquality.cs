namespace QuantityMeasurementApp.Model
{
   
    public class Feet
    {
       
        // readonly ensures immutability
        private readonly double value;

        // Constructor to initialize the feet value.
    
        public Feet(double value)
        {
            this.value = value;
        }

    
        // Only getter is provided to maintain immutability.
        public double Value
        {
            get { return value; }
        }

        
        public override bool Equals(object? obj)
        {
            // Check if both references point to the same object in memory.
            if (ReferenceEquals(this, obj))
                return true;

            // If object is null or not of type Feet, return false.
            if (obj is null || obj.GetType() != typeof(Feet))
                return false;

            // Type casting object to Feet.
            Feet other = (Feet)obj;
            // Returns 0 if values are equal.
            return this.value.CompareTo(other.value) == 0;
        }

        // Whenever Equals is overridden, GetHashCode must also be overridden.

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}