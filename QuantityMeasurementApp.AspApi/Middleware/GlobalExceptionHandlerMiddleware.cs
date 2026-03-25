using System.Text.Json;
using QuantityMeasurementApp.ModelLayer.Exceptions;
using QuantityMeasurementApp.AspApi.DTO;

namespace QuantityMeasurementApp.AspApi.Middleware
{
    // ── UC17: Global Exception Handling ──────────────────────────────────
    //

    //
    // This middleware wraps the ENTIRE HTTP pipeline.
    // If any unhandled exception escapes a controller, this catches it and
    // returns a clean, structured JSON error response to the client.
    //
    // Without this, ASP.NET would return a raw stack trace or a generic
    // 500 page — neither of which is useful in a REST API.
    //
    // How it is registered (in Program.cs):
    //   app.UseGlobalExceptionHandler();
    //
    // HTTP status mapping:
    //   QuantityMeasurementException  → 400 Bad Request
    //   ArgumentException             → 400 Bad Request
    //   InvalidOperationException     → 400 Bad Request
    //   DivideByZeroException         → 500 Internal Server Error
    //   DatabaseException             → 500 Internal Server Error
    //   All other exceptions          → 500 Internal Server Error

    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate                           _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        // Use camelCase JSON to match standard REST API conventions
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented        = false
        };

        public GlobalExceptionHandlerMiddleware(
            RequestDelegate                           next,
            ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next   = next;
            _logger = logger;
        }

        // Called automatically for every HTTP request by ASP.NET Core
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Let the request continue through the pipeline (controllers, etc.)
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the full exception so developers can see it in the console
                _logger.LogError(ex, "Unhandled exception on {Path}: {Message}",
                    context.Request.Path, ex.Message);

                // Send a friendly JSON response to the client
                await WriteErrorResponse(context, ex);
            }
        }

        private static async Task WriteErrorResponse(HttpContext context, Exception ex)
        {
            // Choose HTTP status code and label based on exception type
            (int statusCode, string errorLabel) = ex switch
            {
                QuantityMeasurementException => (400, "Quantity Measurement Error"),
                ArgumentException            => (400, "Quantity Measurement Error"),
                InvalidOperationException    => (400, "Quantity Measurement Error"),
                DivideByZeroException        => (500, "Internal Server Error"),
                DatabaseException            => (500, "Database Error"),
                _                            => (500, "Internal Server Error")
            };

            var errorBody = new ErrorResponseDTO
            {
                Timestamp = DateTime.UtcNow.ToString("o"),
                Status    = statusCode,
                Error     = errorLabel,
                Message   = ex.Message,
                Path      = context.Request.Path
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode  = statusCode;

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(errorBody, _jsonOptions));
        }
    }

    // Extension method so Program.cs can register the middleware cleanly:
    //   app.UseGlobalExceptionHandler();
    public static class GlobalExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(
            this IApplicationBuilder app)
            => app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}