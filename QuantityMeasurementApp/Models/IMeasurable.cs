using System;

namespace QuantityMeasurementApp.Models
{
    /// <summary>
    /// Functional interface to indicate whether arithmetic is supported
    /// </summary>
    public delegate bool SupportsArithmetic();

    /// <summary>
    /// Interface for measurable units
    /// </summary>
    public interface IMeasurable
    {
        // Convert value to base unit
        double ConvertToBaseUnit(double value);

        // Convert value from base unit
        double ConvertFromBaseUnit(double value);

        // Default lambda → arithmetic supported
        SupportsArithmetic supportsArithmetic => () => true;

        /// <summary>
        /// Check if arithmetic is supported
        /// </summary>
        bool SupportsArithmeticOperation()
        {
            return supportsArithmetic();
        }

        /// <summary>
        /// Validate operation support
        /// Default: allow all
        /// </summary>
        void ValidateOperationSupport(string operation)
        {
            // default does nothing
        }
    }
}