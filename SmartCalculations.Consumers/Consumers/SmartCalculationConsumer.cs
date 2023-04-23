using MassTransit;
using Microsoft.Extensions.Logging;
using SmartCalculations.Contracts;
using SmartCalculations.MessageContracts;

namespace SmartCalculations.MessageComponents.Consumers
{
    public class SmartCalculationConsumer : IConsumer<ICalculationAccepted>
    {
        private readonly ILogger<SmartCalculationConsumer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public SmartCalculationConsumer(ILogger<SmartCalculationConsumer> logger,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _publishEndpoint = publishEndpoint ??
                throw new ArgumentNullException(nameof(publishEndpoint));
        }

        public async Task Consume(ConsumeContext<ICalculationAccepted> context)
        {
            _logger.LogInformation($"{nameof(SmartCalculationConsumer)}: " +
                $"Starting calculation ({context.Message.CalculationId}) " +
                $"for calculation id = {context.Message.CalculationId}. UTC: {DateTime.UtcNow}.");

            // Long-running task that takes time
            await Task.Delay(8000);


            // Publish it as completed calculation
            await _publishEndpoint.Publish<ICalculationCompleted>(new
            {
                CalculationId = context.Message.CalculationId,
                TimeStamp = DateTime.UtcNow
            });

            _logger.LogInformation($"{nameof(SmartCalculationConsumer)}:" +
                $"Completed calculation ({context.Message.CalculationType}) " +
                $"for calculation id = {context.Message.CalculationId}. UTC: {DateTime.UtcNow}.");
        }
    }
}
