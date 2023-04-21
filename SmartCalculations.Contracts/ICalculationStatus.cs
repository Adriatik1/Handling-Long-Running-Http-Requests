using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalculations.MessageContracts
{
    public interface ICalculationStatus
    {
        Guid CalculationId { get; }
        string State { get; }
    }
}
