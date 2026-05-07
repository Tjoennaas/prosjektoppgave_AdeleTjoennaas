


       
        using CostPricingEngine.Models.CostCalculation;

        namespace CostPricingEngine.Models.CostMargin {


            public class CalculationMargin {

                public int Id { get; set; }
                public int PeriodNumber { get; set; }
                public int AzureCostCalculationId { get; set; }
                public AzureCostCalculation AzureCostCalculation { get; set; } = null!;
                public Guid CalculationGroupId { get; set; }
                public decimal Margin { get; set; }
                public decimal MarginPercent { get; set; }
                public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  }}






   