using System;

namespace QuantityMeasurementApp.ModelLayer.Entities
{
    /// <summary>
    /// Persistence record for a quantity measurement operation.
    /// Stored in the repository after every operation.
    /// </summary>
    [Serializable]
    public class QuantityMeasurementEntity
    {
        /// <summary>Unique identifier for this record.</summary>
        public Guid Id { get; set; }

        /// <summary>Name of the operation.</summary>
        public string Operation { get; set; } = string.Empty;

        /// <summary>When the operation was performed.</summary>
        public DateTime Timestamp { get; set; }

        /// <summary>First operand.</summary>
        public string Operand1 { get; set; } = string.Empty;

        /// <summary>Second operand (null for single operand operations).</summary>
        public string? Operand2 { get; set; }

        /// <summary>Target unit (used only for convert operations).</summary>
        public string? TargetUnit { get; set; }

        /// <summary>Result of operation.</summary>
        public string? Result { get; set; }

        /// <summary>True if operation failed.</summary>
        public bool HasError { get; set; }

        /// <summary>Error message when HasError = true.</summary>
        public string? ErrorMessage { get; set; }

        // Required by serialization
        public QuantityMeasurementEntity() { }

        // Binary operation constructor (Add, Subtract, Compare, Divide)
        public QuantityMeasurementEntity(string operation, string operand1, string operand2, string result)
        {
            Id = Guid.NewGuid();
            Operation = operation;
            Operand1 = operand1;
            Operand2 = operand2;
            Result = result;
            HasError = false;
            Timestamp = DateTime.Now;
        }

        // Error constructor
        public QuantityMeasurementEntity(string operation, string operand1, string? operand2, string errorMessage, bool hasError)
        {
            Id = Guid.NewGuid();
            Operation = operation;
            Operand1 = operand1;
            Operand2 = operand2;
            ErrorMessage = errorMessage;
            HasError = hasError;
            Timestamp = DateTime.Now;
        }

        /// <summary>Human readable log.</summary>
        public override string ToString()
        {
            string ops = Operand2 != null
                ? $"{Operand1} & {Operand2}"
                : Operand1;

            return HasError
                ? $"[{Timestamp:HH:mm:ss}] {Operation} | {ops} → ERROR: {ErrorMessage}"
                : $"[{Timestamp:HH:mm:ss}] {Operation} | {ops} → {Result}";
        }
    }
}