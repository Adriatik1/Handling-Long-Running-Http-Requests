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
        private readonly IRequestClient<ICheckCalculation> _checkCalculationRequestClient;

        public CalculationsController(
            IPublishEndpoint publishEndpoint,
            IRequestClient<ICheckCalculation> checkCalculationRequestClient)
        {
            _publishEndpoint = publishEndpoint 
                ?? throw new ArgumentNullException(nameof(publishEndpoint));
            _checkCalculationRequestClient = checkCalculationRequestClient
                ?? throw new ArgumentNullException(nameof(_checkCalculationRequestClient));
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
            var (status, notFound) = await _checkCalculationRequestClient
                .GetResponse<ICalculationStatus, ICalculationNotFound>(new
                {
                    CalculationId = calculationId
                });
            
            if (status.IsCompletedSuccessfully)
            {
                var response = await status;
                return Ok(response);
            }

            var responseNotFound = await notFound;

            return Ok(responseNotFound);
        }
    }
}
