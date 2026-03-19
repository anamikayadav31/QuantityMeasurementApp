namespace QuantityMeasurementApp.ModelLayer.Exceptions
{
    /// <summary>
    /// Thrown when a database operation fails.
    /// Wraps the original ADO.NET exception with a friendlier message.
    /// </summary>
    public class DatabaseException : Exception
    {
        public DatabaseException(string message) : base(message) { }

        public DatabaseException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}