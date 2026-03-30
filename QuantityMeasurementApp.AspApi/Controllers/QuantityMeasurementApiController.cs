using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.BussinessLayer.Interfaces;
using QuantityMeasurementApp.ModelLayer.DTO;
using QuantityMeasurementApp.ModelLayer.Entities;
using QuantityMeasurementApp.RepoLayer.Interfaces;
using QuantityMeasurementApp.AspApi.DTO;

namespace QuantityMeasurementApp.AspApi.Controllers
{
    // ── UC17: Quantity Measurement Controller — EF Core async version ─────
    //
    // All repository calls are now awaited because EF Core uses async I/O.
    // Spring equivalent: @Async methods in a @Service class.

    [ApiController]
    [Route("api/v1/quantities")]
    [Produces("application/json")]
    public class QuantityMeasurementApiController : ControllerBase
    {
        private readonly IQuantityMeasurementService    _service;
        private readonly IQuantityMeasurementRepository _repository;
        private readonly ILogger<QuantityMeasurementApiController> _logger;

        public QuantityMeasurementApiController(
            IQuantityMeasurementService    service,
            IQuantityMeasurementRepository repository,
            ILogger<QuantityMeasurementApiController> logger)
        {
            _service    = service;
            _repository = repository;
            _logger     = logger;
        }

        // ── POST /compare ─────────────────────────────────────────────────
        [HttpPost("compare")]
        public async Task<IActionResult> CompareQuantities([FromBody] QuantityInputDTO input)
        {
            if (input.ThatQuantityDTO == null)
                return BadRequest(new { message = "thatQuantityDTO is required." });
            try
            {
                var q1 = ToDTO(input.ThisQuantityDTO);
                var q2 = ToDTO(input.ThatQuantityDTO);
                bool result = _service.Compare(q1, q2);

                await PersistSuccessAsync("COMPARE", q1, q2, result.ToString());

                return Ok(QuantityMeasurementResponseDTO.Success(
                    input.ThisQuantityDTO, input.ThatQuantityDTO,
                    "compare", resultString: result.ToString().ToLower()));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Compare failed");
                await PersistErrorAsync("COMPARE", input.ThisQuantityDTO, input.ThatQuantityDTO, ex.Message);
                return BadRequest(QuantityMeasurementResponseDTO.Error(
                    input.ThisQuantityDTO, input.ThatQuantityDTO, "compare", ex.Message));
            }
        }

        // ── POST /convert ─────────────────────────────────────────────────
        [HttpPost("convert")]
        public async Task<IActionResult> ConvertQuantity([FromBody] QuantityInputDTO input)
        {
            if (input.ThatQuantityDTO == null)
                return BadRequest(new { message = "thatQuantityDTO.unit is required." });
            try
            {
                var    q      = ToDTO(input.ThisQuantityDTO);
                string target = input.ThatQuantityDTO.Unit.Trim().ToUpperInvariant();
                var    result = _service.Convert(q, target);

                await PersistSuccessAsync("CONVERT", q, null, result.ToString());

                return Ok(QuantityMeasurementResponseDTO.Success(
                    input.ThisQuantityDTO, input.ThatQuantityDTO,
                    "convert", resultValue: result.Value, resultUnit: result.Unit));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Convert failed");
                await PersistErrorAsync("CONVERT", input.ThisQuantityDTO, input.ThatQuantityDTO, ex.Message);
                return BadRequest(QuantityMeasurementResponseDTO.Error(
                    input.ThisQuantityDTO, input.ThatQuantityDTO, "convert", ex.Message));
            }
        }

        // ── POST /add ─────────────────────────────────────────────────────
        [HttpPost("add")]
        public async Task<IActionResult> AddQuantities([FromBody] QuantityInputDTO input)
        {
            if (input.ThatQuantityDTO == null)
                return BadRequest(new { message = "thatQuantityDTO is required." });
            try
            {
                var q1     = ToDTO(input.ThisQuantityDTO);
                var q2     = ToDTO(input.ThatQuantityDTO);
                var result = _service.Add(q1, q2);

                await PersistSuccessAsync("ADD", q1, q2, result.ToString());

                return Ok(QuantityMeasurementResponseDTO.Success(
                    input.ThisQuantityDTO, input.ThatQuantityDTO,
                    "add", resultValue: result.Value, resultUnit: result.Unit,
                    resultMType: input.ThisQuantityDTO.MeasurementType));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Add failed");
                await PersistErrorAsync("ADD", input.ThisQuantityDTO, input.ThatQuantityDTO, ex.Message);
                return BadRequest(QuantityMeasurementResponseDTO.Error(
                    input.ThisQuantityDTO, input.ThatQuantityDTO, "add", ex.Message));
            }
        }

        // ── POST /subtract ────────────────────────────────────────────────
        [HttpPost("subtract")]
        public async Task<IActionResult> SubtractQuantities([FromBody] QuantityInputDTO input)
        {
            if (input.ThatQuantityDTO == null)
                return BadRequest(new { message = "thatQuantityDTO is required." });
            try
            {
                var q1     = ToDTO(input.ThisQuantityDTO);
                var q2     = ToDTO(input.ThatQuantityDTO);
                var result = _service.Subtract(q1, q2);

                await PersistSuccessAsync("SUBTRACT", q1, q2, result.ToString());

                return Ok(QuantityMeasurementResponseDTO.Success(
                    input.ThisQuantityDTO, input.ThatQuantityDTO,
                    "subtract", resultValue: result.Value, resultUnit: result.Unit,
                    resultMType: input.ThisQuantityDTO.MeasurementType));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Subtract failed");
                await PersistErrorAsync("SUBTRACT", input.ThisQuantityDTO, input.ThatQuantityDTO, ex.Message);
                return BadRequest(QuantityMeasurementResponseDTO.Error(
                    input.ThisQuantityDTO, input.ThatQuantityDTO, "subtract", ex.Message));
            }
        }

        // ── POST /divide ──────────────────────────────────────────────────
        [HttpPost("divide")]
        public async Task<IActionResult> DivideQuantities([FromBody] QuantityInputDTO input)
        {
            if (input.ThatQuantityDTO == null)
                return BadRequest(new { message = "thatQuantityDTO is required." });
            try
            {
                var    q1     = ToDTO(input.ThisQuantityDTO);
                var    q2     = ToDTO(input.ThatQuantityDTO);
                double result = _service.Divide(q1, q2);

                await PersistSuccessAsync("DIVIDE", q1, q2, result.ToString("F4"));

                return Ok(QuantityMeasurementResponseDTO.Success(
                    input.ThisQuantityDTO, input.ThatQuantityDTO,
                    "divide", resultValue: result));
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Divide failed");
                await PersistErrorAsync("DIVIDE", input.ThisQuantityDTO, input.ThatQuantityDTO, ex.Message);
                return StatusCode(500, QuantityMeasurementResponseDTO.Error(
                    input.ThisQuantityDTO, input.ThatQuantityDTO, "divide", ex.Message));
            }
        }

        // ── GET /history ──────────────────────────────────────────────────
        [HttpGet("history")]
        public async Task<IActionResult> GetAllHistory()
            => Ok(await _repository.GetAllAsync());

        // ── GET /history/operation/{operation} ────────────────────────────
        [HttpGet("history/operation/{operation}")]
        public async Task<IActionResult> GetOperationHistory([FromRoute] string operation)
            => Ok(await _repository.GetByOperationAsync(operation.ToUpperInvariant()));

        // ── GET /history/errored ──────────────────────────────────────────
        [HttpGet("history/errored")]
        public async Task<IActionResult> GetErrorHistory()
            => Ok(await _repository.GetErroredAsync());

        // ── GET /count/{operation} ────────────────────────────────────────
        [HttpGet("count/{operation}")]
        public async Task<IActionResult> GetOperationCount([FromRoute] string operation)
        {
            int count = await _repository.CountByOperationAsync(operation.ToUpperInvariant());
            return Ok(new { operation = operation.ToUpperInvariant(), count });
        }

        // ── Private helpers ───────────────────────────────────────────────

        private static QuantityDTO ToDTO(QuantityRequestItem item)
            => new QuantityDTO(item.Value, item.Unit,
                MeasurementTypeMapper.Map(item.MeasurementType));

        private async Task PersistSuccessAsync(
            string op, QuantityDTO q1, QuantityDTO? q2, string result)
        {
            try
            {
                await _repository.SaveAsync(new QuantityMeasurementEntity(
                    op, q1.ToString(), q2?.ToString() ?? string.Empty, result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist success record for {Op}", op);
            }
        }

        private async Task PersistErrorAsync(
            string op,
            QuantityRequestItem  q1,
            QuantityRequestItem? q2,
            string errorMessage)
        {
            try
            {
                await _repository.SaveAsync(new QuantityMeasurementEntity(
                    op,
                    $"{q1.Value} {q1.Unit} [{q1.MeasurementType}]",
                    q2 == null ? null : $"{q2.Value} {q2.Unit} [{q2.MeasurementType}]",
                    errorMessage,
                    true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to persist error record for {Op}", op);
            }
        }
    }
}