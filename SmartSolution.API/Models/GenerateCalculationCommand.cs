namespace SmartSolution.API.Models
{
    public class GenerateCalculationCommand
    {
        public string CalculationType { get; set; } = default!;
    }


    public class GenerateCalculationResult
    {
        public Guid CalculationId { get; set; }
    }
}
