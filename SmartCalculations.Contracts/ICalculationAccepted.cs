using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalculations.Contracts
{
    public interface ICalculationAccepted
    {
        /// <summary>
        /// Calculation Id.
        /// </summary>
        Guid CalculationId { get; }

        /// <summary>
        /// Coffee type.
        /// </summary>
        string CalculationType { get; }
    }
}
