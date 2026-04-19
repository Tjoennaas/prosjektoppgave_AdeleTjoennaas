
using System.ComponentModel.DataAnnotations;


namespace ProsjektOppgave_AdeleTjoennaas.Models {
public class AzurePrice
{
        [Key]
        public int Id { get; set; }
        public string? CurrencyCode { get; set; }
        public decimal TierMinimumUnits { get; set; }
        public decimal RetailPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public string? ArmRegionName { get; set; }
        public string? Location { get; set; } 
        public DateTime? EffectiveStartDate { get; set; }
        public string? MeterId { get; set; }
        public string? MeterName { get; set; }
        public string? ProductId { get; set; }
        public string? SkuId { get; set; }
        public string? ProductName { get; set; }
        public string? SkuName { get; set; }
        public string? ServiceName { get; set; }
        public string? ServiceId { get; set; }
        public string? ServiceFamily { get; set; }
        public string? Type { get; set; }
        public bool? IsPrimaryMeterRegion { get; set; }
        public string? ArmSkuName { get; set; }
        public DateTime LastUpdatedUtc { get; internal set; }
}}
