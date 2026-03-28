

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ProsjektOppgave_AdeleTjoennaas.Models {



public class AzurePrice
{
    [Key]
    public int Id { get; set; }

    [NotNull]
    public string? ArmRegionName { get; set; }
    [NotNull]
    public string? ProductName { get; set; }
    public decimal RetailPrice { get; set; }
    [NotNull]
    public string? CurrencyCode {get; set;}
    public DateTime LastUpdatedUtc { get; internal set; }
}}