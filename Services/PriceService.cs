
      
using ProsjektOppgave_AdeleTjoennaas.Models;


namespace ProsjektOppgave_AdeleTjoennaas.Services
{
    public class AzurePriceService
    {
        private readonly HttpClient _httpClient;
      private readonly ILogger<AzurePriceService> _logger;

        public AzurePriceService(HttpClient httpClient, ILogger<AzurePriceService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<AzurePrice>> GetPricesAsync(string product, string region, string currency)
        {
            List<AzurePrice> allPrices = new List<AzurePrice>();

            var url = $"https://prices.azure.com/api/retail/prices?currencyCode='{currency}'" +
                      $"&$filter=armRegionName eq '{region}'" +
                      $" and productName eq '{product}'";

            _logger.LogInformation("Fetching data from Azure");

            try {

            while (!string.IsNullOrEmpty(url))
            {
                var result = await _httpClient.GetFromJsonAsync<AzureResponse>(url);

                if (result?.Items != null)
                {
                    allPrices.AddRange(result.Items);
                }
            else
                {
                        
            _logger.LogInformation("No items returne"); 
            }
                url = result?.NextPageLink;
            }}

             catch (Exception exception) {
            
        _logger.LogError(exception, "Failed to fetch produkts");
        throw; 
     }

    return allPrices;
        
    }}}

