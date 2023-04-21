using MassTransit;
using MassTransit.Saga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalculations.MessageComponents.SagaComponents
{
    public class CalculationState :
        SagaStateMachineInstance,
        ISagaVersion
    {
        public Guid CorrelationId { get; set; }
        public int Version { get; set; }
        public string CurrentState { get; set; } = default!;
        public DateTime Updated { get; set; }
    }
}
