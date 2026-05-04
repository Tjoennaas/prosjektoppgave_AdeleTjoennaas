



  
using Microsoft.EntityFrameworkCore;

namespace CostPricingEngine.Models.CostCalculation
{
    public class AzureCostCalculation
    {          
        public int AzureCostCalculationId { get; set; }

        public decimal FixedCosts { get; set; }
        public decimal VariableCosts { get; set; }
        public decimal TotalAzureCost { get; set; }

        public decimal PerMillionEventsReceived { get; set; }
        public decimal PerMillionEventsRetained { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  }}      