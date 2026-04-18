

namespace ProsjektOppgave_AdeleTjoennaas.Models {
public class AzureResponse
{
    public required List<AzurePrice>Items { get; set; } = new();
    public string? NextPageLink { get; set; }
    public int Count { get; set; }
    
}}