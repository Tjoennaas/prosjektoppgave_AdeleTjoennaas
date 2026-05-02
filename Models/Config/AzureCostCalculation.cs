
namespace ProsjektOppgave_AdeleTjoennaas.Models
{
    public class AzureCostCalculation
    {
        public int Id { get; set; }

        public decimal FixedCosts { get; set; }
        public decimal VariableCosts { get; set; }
        public decimal TotalAzureCost { get; set; }

        public decimal PerMillionEventsReceived { get; set; }
        public decimal PerMillionEventsRetained { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}