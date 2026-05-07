
using CostPricingEngine.Dto;


//Modellen for kunde kalkulator

   namespace CostPricingEngine.Models.CostCalculation {

    public class CustomerCalculationResult {

 
    public int Id { get; set; }

    public Guid CalculationGroupId { get; set; }

    public int PeriodNumber { get; set; }

    public int ActiveUsers { get; set; }
    public int EventsPerPeriod { get; set; }
    public int RetentionPeriods { get; set; }
    public int? Collector { get; set; }

    public decimal BasePrice { get; set; }
    public decimal EventCost { get; set; }
    public decimal UserCost { get; set; }
    public decimal RetentionCost { get; set; }
    public decimal CollectorCost { get; set; }
    public decimal TotalVariablePrice { get; set; }
    public decimal TotalPrice { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}};



  




