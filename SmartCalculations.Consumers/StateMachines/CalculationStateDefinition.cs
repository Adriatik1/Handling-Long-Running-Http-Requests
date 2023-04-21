using MassTransit;
using SmartCalculations.MessageComponents.SagaComponents;

namespace SmartCalculations.MessageComponents.StateMachines
{
    public class CalculationStateDefinition : SagaDefinition<CalculationState>
    {
        public CalculationStateDefinition()
        {
            ConcurrentMessageLimit = 8;
        }

        protected override void ConfigureSaga(
            IReceiveEndpointConfigurator endpointConfigurator,
            ISagaConfigurator<CalculationState> sagaConfigurator)
        {
            // Any failed message will be locked inside the broker and is not available to other consumers.
            // Any failed message will be kept in memory and block any processing message slots, e.g. pre-fetch slots.
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 5000, 10000));

            // Prevent any messages or events published from this state machine from going out. E.g., this state machine will stop publishing events.
            sagaConfigurator.UseInMemoryOutbox();
        }
    }
}
