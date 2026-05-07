


// Denne klassen sørger for at en liste med flere Azure Retail prices API rader blir returnert tilbake.

    namespace CostPricingEngine.Models.AzureApi {
    public class  AzureResponse {

        public required List<AzureApiPricesDto>Items { get; set; } = new();
        public string? NextPageLink { get; set; }
        public int Count { get; set; }
        
    }}