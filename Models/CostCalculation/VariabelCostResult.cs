
//reprenseterer summen av variablene

namespace CostPricingEngine.Models.CostCalculation
{
    public class VariableCostResult
    {
        public decimal TotalVariableCosts { get; set; }
        public decimal PerMillionEventsReceived { get; set; }
        public decimal PerMillionEventsRetained { get; set; }
    }}