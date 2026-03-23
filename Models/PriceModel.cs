

using System.Diagnostics.CodeAnalysis;

namespace ProsjektOppgave_AdeleTjoennaas.Models {



public class AzurePrice
{

    [NotNull]
    public string? ArmRegionName { get; set; }
    [NotNull]
    public string? ProductName { get; set; }
    public decimal RetailPrice { get; set; }
    [NotNull]
    public string? CurrencyCode {get; set;}
    }
}