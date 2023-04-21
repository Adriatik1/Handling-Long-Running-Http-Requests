using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SmartCalculations.Contracts;
using SmartCalculations.MessageContracts;
using SmartSolution.API.Models;

namespace SmartSolution.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculationsController : Controller
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<CalculationsController> _logger;

        public CalculationsController(
            IPublishEndpoint publishEndpoint,
            ILogger<CalculationsController> logger)
        {
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost("Simple")]
        public async Task<IActionResult> GenerateSimpleCalculationAsync([FromBody] GenerateCalculationCommand command)
        {
            await Task.Delay(5000);

            return StatusCode(StatusCodes.Status408RequestTimeout);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateSmartCalculationAsync([FromBody] GenerateCalculationCommand command)
        {
            GenerateCalculationResult result = new()
            {
                CalculationId = Guid.NewGuid()
            };

            await _publishEndpoint.Publish<ICalculationAccepted>(new
            {
                result.CalculationId,
                CalculationType = "EmissionCalculation" 
            });

            return Ok(result);
        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> CheckCalculationStatusAsync(Guid calculationId)
        {
            GenerateCalculationResult result = new()
            {
                CalculationId = Guid.NewGuid()
            };

            await _publishEndpoint.Publish<ICheckCalculation>(new
            {
                result.CalculationId
            });

            return Ok(result);
        }
    }
}
