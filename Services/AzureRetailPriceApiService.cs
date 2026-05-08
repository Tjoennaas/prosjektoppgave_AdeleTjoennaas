


// Klassen henter rå prisdata fra Azure Retail Prices API
// ved hjelp av HTTP-forespørsler og dynamiske filtre.
// Brukt referanse fra https://learn.microsoft.com/en-us/rest/api/cost-management/retail-prices/azure-retail-prices#api-response-pagination
// Brukte ChatGPT for å få hjelp med filtrering 
    using System.Net;
    using CostPricingEngine.Models.AzureApi;
            
  namespace CostPricingEngine.Services {


    public class AzureRetailPriceApiService {
    private readonly HttpClient _httpClient;
            private readonly ILogger<AzureRetailPriceApiService> _logger;

    public AzureRetailPriceApiService(HttpClient httpClient, ILogger<AzureRetailPriceApiService> logger) {
        _httpClient = httpClient;
        _logger = logger; 
    }

        public async Task<List<AzureApiPricesDto>> GetPricesAsync(
            string region,
            string currency,
            string? productName = null,
            string? serviceName = null,
            string? meterName = null,
            string? skuName = null,
            string? armSkuName = null,
            string? unitOfMeasure = null) {

             if(string.IsNullOrEmpty(region)) {
                throw new ArgumentException("Region must be provided");
            }

            if (string.IsNullOrEmpty(currency)) {
                throw new ArgumentException("Currensy must be provided");
            }

            var allPrices = new List<AzureApiPricesDto>();
       
            var filterParts = new List<string> {
                $"armRegionName eq '{region}'"
            };

            AddFilter(filterParts, "productName", productName);
            AddFilter(filterParts, "serviceName", serviceName);
            AddFilter(filterParts, "meterName", meterName);
            AddFilter(filterParts, "skuName", skuName);
            AddFilter(filterParts, "armSkuName", armSkuName);
            AddFilter(filterParts, "unitOfMeasure", unitOfMeasure);

            var filter = string.Join(" and ", filterParts);
              
              
             var url = $"https://prices.azure.com/api/retail/prices?currencyCode={currency}" +
          $"&$filter={WebUtility.UrlEncode(filter)}";
           

            _logger.LogInformation("Fetching data from Azure");


             while (!string.IsNullOrEmpty(url)) {
                        var response = await _httpClient.GetAsync(url);
                    
                    if (!response.IsSuccessStatusCode){  
                        throw new HttpRequestException($"{response.StatusCode}"); }
                   
             var result = await response.Content.ReadFromJsonAsync<AzureResponse>();
                     
                   if (result?.Items == null) {
                        throw new KeyNotFoundException("Ingen data funnet.");  }
                        allPrices.AddRange(result.Items);
                        url = result?.NextPageLink;  }

                         return allPrices;   }



        private static void AddFilter(List<string> filterParts, string fieldName, string? fieldValue) {
                    if (string.IsNullOrWhiteSpace(fieldValue)) {
                        return; } 
                    
                    var escapedValue = fieldValue.Replace("'", "''");
                    filterParts.Add($"{fieldName} eq '{escapedValue}'");
                }}}

