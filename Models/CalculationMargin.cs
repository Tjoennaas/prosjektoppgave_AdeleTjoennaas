



namespace ProsjektOppgave_AdeleTjoennaas.Models {


    public class CalculationMargin {

        public int Id { get; set; }

        public int AzureCostCalculationId { get; set; }
        public Guid CalculationGroupId { get; set; }
        public decimal Margin { get; set; }
        public decimal MarginPercent { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  };}