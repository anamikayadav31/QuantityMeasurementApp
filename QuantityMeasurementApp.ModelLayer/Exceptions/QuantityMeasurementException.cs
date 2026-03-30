using System;

namespace QuantityMeasurementApp.ModelLayer.Exceptions
{
    /// <summary>
    /// Custom exception for Quantity Measurement operations.
    /// Used in test cases and service layer for error handling.
    /// </summary>
    public class QuantityMeasurementException : Exception
    {
        /// <summary>
        /// Creates a new QuantityMeasurementException with a message.
        /// </summary>
        /// <param name="message">The exception message.</param>
        public QuantityMeasurementException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new QuantityMeasurementException with a message and inner exception.
        /// </summary>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public QuantityMeasurementException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}