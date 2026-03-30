using System.ComponentModel.DataAnnotations;
using QuantityMeasurementApp.ModelLayer.Enums;

namespace QuantityMeasurementApp.AspApi.DTO
{
    // ════════════════════════════════════════════════════════════════════
    //  UC17: REST API DTOs  (Data Transfer Objects)
    //
    //  These classes define the JSON shape that clients send and receive.
    //  They are intentionally SEPARATE from the internal QuantityDTO so
    //  that the API contract can evolve without touching business logic.
    //
    //  Flow:
    //    Client JSON  →  QuantityInputDTO  →  QuantityDTO (internal)
    //    QuantityDTO (internal)  →  QuantityMeasurementResponseDTO  →  Client JSON
    // ════════════════════════════════════════════════════════════════════

    // ── REQUEST body ──────────────────────────────────────────────────────

    /// <summary>
    /// The JSON body the client sends for every operation.
    ///
    /// Binary ops (compare/add/subtract/divide):
    ///   { "thisQuantityDTO": {...}, "thatQuantityDTO": {...} }
    ///
    /// Convert:
    ///   { "thisQuantityDTO": {...}, "thatQuantityDTO": { "unit":"INCHES", ... } }
    ///   (thatQuantityDTO.unit = target unit; its value field is ignored)
    /// </summary>
    public class QuantityInputDTO
    {
        [Required(ErrorMessage = "thisQuantityDTO is required.")]
        public QuantityRequestItem ThisQuantityDTO { get; set; } = null!;

        // Null is allowed for unary ops; controllers validate it where needed
        public QuantityRequestItem? ThatQuantityDTO { get; set; }
    }

    /// <summary>One quantity as sent by the client.</summary>
    public class QuantityRequestItem
    {
        [Required]
        public double Value { get; set; }

        [Required(ErrorMessage = "unit is required.")]
        [RegularExpression(
            @"^(FEET|INCHES|YARDS|CENTIMETERS|KILOGRAM|GRAM|POUND|LITRE|MILLILITRE|GALLON|CELSIUS|FAHRENHEIT|KELVIN)$",
            ErrorMessage = "unit must be one of: FEET, INCHES, YARDS, CENTIMETERS, " +
                           "KILOGRAM, GRAM, POUND, LITRE, MILLILITRE, GALLON, " +
                           "CELSIUS, FAHRENHEIT, KELVIN.")]
        public string Unit { get; set; } = string.Empty;

        [Required(ErrorMessage = "measurementType is required.")]
        [RegularExpression(
            @"^(LengthUnit|WeightUnit|VolumeUnit|TemperatureUnit)$",
            ErrorMessage = "measurementType must be one of: LengthUnit, WeightUnit, VolumeUnit, TemperatureUnit.")]
        public string MeasurementType { get; set; } = string.Empty;
    }

    // ── RESPONSE body ─────────────────────────────────────────────────────

    /// <summary>
    /// The JSON body the API returns for every operation.
    ///
    /// Compare:                resultString = "true" / "false"
    /// Convert/Add/Subtract:   resultValue + resultUnit
    /// Divide:                 resultValue (dimensionless ratio)
    /// Error:                  isError = true, errorMessage = reason
    ///
    /// Sample success (compare):
    /// {
    ///   "thisValue":1.0,  "thisUnit":"FEET",   "thisMeasurementType":"LengthUnit",
    ///   "thatValue":12.0, "thatUnit":"INCHES",  "thatMeasurementType":"LengthUnit",
    ///   "operation":"compare",
    ///   "resultString":"true", "resultValue":0.0, "resultUnit":null,
    ///   "errorMessage":null, "isError":false
    /// }
    /// </summary>
    public class QuantityMeasurementResponseDTO
    {
        // ── Echo of the input (helps the client debug) ────────────────────
        public double  ThisValue             { get; set; }
        public string? ThisUnit              { get; set; }
        public string? ThisMeasurementType   { get; set; }

        public double  ThatValue             { get; set; }
        public string? ThatUnit              { get; set; }
        public string? ThatMeasurementType   { get; set; }

        public string? Operation             { get; set; }

        // ── Result fields (only one group is populated per operation) ─────
        public string? ResultString          { get; set; }  // compare
        public double  ResultValue           { get; set; }  // convert / add / subtract / divide
        public string? ResultUnit            { get; set; }  // add / subtract
        public string? ResultMeasurementType { get; set; }  // add / subtract

        // ── Error fields ──────────────────────────────────────────────────
        public string? ErrorMessage { get; set; }
        public bool    IsError      { get; set; }

        // ── Static factory methods keep the controller clean ──────────────

        public static QuantityMeasurementResponseDTO Success(
            QuantityRequestItem  q1,
            QuantityRequestItem? q2,
            string  operation,
            string? resultString = null,
            double  resultValue  = 0,
            string? resultUnit   = null,
            string? resultMType  = null)
        {
            return new QuantityMeasurementResponseDTO
            {
                ThisValue             = q1.Value,
                ThisUnit              = q1.Unit,
                ThisMeasurementType   = q1.MeasurementType,
                ThatValue             = q2?.Value ?? 0,
                ThatUnit              = q2?.Unit,
                ThatMeasurementType   = q2?.MeasurementType,
                Operation             = operation,
                ResultString          = resultString,
                ResultValue           = resultValue,
                ResultUnit            = resultUnit,
                ResultMeasurementType = resultMType,
                IsError               = false
            };
        }

        public static QuantityMeasurementResponseDTO Error(
            QuantityRequestItem  q1,
            QuantityRequestItem? q2,
            string operation,
            string errorMessage)
        {
            return new QuantityMeasurementResponseDTO
            {
                ThisValue           = q1.Value,
                ThisUnit            = q1.Unit,
                ThisMeasurementType = q1.MeasurementType,
                ThatValue           = q2?.Value ?? 0,
                ThatUnit            = q2?.Unit,
                ThatMeasurementType = q2?.MeasurementType,
                Operation           = operation,
                ErrorMessage        = errorMessage,
                IsError             = true
            };
        }
    }

    /// <summary>
    /// Standard error envelope returned by GlobalExceptionHandlerMiddleware
    /// when an unhandled exception escapes a controller action.
    ///
    /// Equivalent to Java Spring's @ControllerAdvice error response.
    ///
    /// Sample:
    /// {
    ///   "timestamp":"2026-03-19T12:00:00Z",
    ///   "status":400,
    ///   "error":"Quantity Measurement Error",
    ///   "message":"Temperature does not support arithmetic...",
    ///   "path":"/api/v1/quantities/add"
    /// }
    /// </summary>
    public class ErrorResponseDTO
    {
        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("o");
        public int    Status    { get; set; }
        public string Error     { get; set; } = string.Empty;
        public string Message   { get; set; } = string.Empty;
        public string Path      { get; set; } = string.Empty;
    }

    // ── Mapper: "LengthUnit" string  →  MeasurementType.LENGTH enum ──────

    public static class MeasurementTypeMapper
    {
        public static MeasurementType Map(string s) => s.Trim() switch
        {
            "LengthUnit"      => MeasurementType.LENGTH,
            "WeightUnit"      => MeasurementType.WEIGHT,
            "VolumeUnit"      => MeasurementType.VOLUME,
            "TemperatureUnit" => MeasurementType.TEMPERATURE,
            _ => throw new ArgumentException($"Unknown measurementType: '{s}'")
        };
    }
}