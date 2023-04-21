using MassTransit;
using SmartCalculations.Contracts;
using SmartCalculations.MessageComponents.SagaComponents;
using SmartCalculations.MessageContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalculations.MessageComponents.StateMachines
{
    public class CalculationStateMachine : MassTransitStateMachine<CalculationState>
    {
        public CalculationStateMachine()
        {
            Event(() => CalculationAccepted,
                x => x.CorrelateById(m => m.Message.CalculationId));
            Event(() => CalculationCompleted,
                x => x.CorrelateById(m => m.Message.CalculationId));

            Event(() => CheckCalculationStatusRequested, x =>
            {
                x.CorrelateById(m => m.Message.CalculationId);

                x.OnMissingInstance(m => m.ExecuteAsync(async context =>
                {
                    if (context.RequestId.HasValue)
                    {
                        await context.RespondAsync<ICalculationNotFound>(new
                        {
                            RequestId = context.RequestId.Value
                        });
                    }
                }));
            });

            InstanceState(x => x.CurrentState);

            Initially(
                When(CalculationAccepted)
                    .Then(context =>
                    {
                        context.Saga.Updated = DateTime.UtcNow;
                    })
                    .TransitionTo(Accepted)
                );

            DuringAny(
                When(CheckCalculationStatusRequested)
                    .RespondAsync(x => x.Init<ICalculationStatus>(new
                    {
                        CalculationId = x.Saga.CorrelationId,
                        State = x.Saga.CurrentState
                    }))
            );

            DuringAny(
                When(CalculationCompleted)
                    .TransitionTo(Completed)
            );
        }

        /// <summary>
        ///     Accepted state.
        /// </summary>
        public State Accepted { get; private set; } = default!;

        /// <summary>
        ///  Completed state
        /// </summary>
        public State Completed { get; private set; } = default!;


        /// <summary>
        ///     On calculation accepted event.
        /// </summary>
        public Event<ICalculationAccepted> CalculationAccepted { get; private set; } = default!;

        /// <summary>
        ///     On calculation completed event.
        /// </summary>
        public Event<ICalculationCompleted> CalculationCompleted { get; private set; } = default!;

        /// <summary>
        ///     On check calculation status event.
        /// </summary>
        public Event<ICheckCalculation> CheckCalculationStatusRequested { get; private set; } = default!;
    }
}
