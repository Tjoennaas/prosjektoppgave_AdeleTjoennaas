

using System.Net;
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

        public async Task<List<AzurePrice>> GetPricesAsync(
            string region,
            string currency,
            string? productName = null,
            string? serviceName = null,
            string? meterName = null,
            string? skuName = null,
            string? armSkuName = null,
            string? unitOfMeasure = null)
        {
            var allPrices = new List<AzurePrice>();
            var filterParts = new List<string>
            {
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

            try
            {
                while (!string.IsNullOrEmpty(url))
                {
                    var result = await _httpClient.GetFromJsonAsync<AzureResponse>(url);

                    if (result?.Items != null)
                    {
                        allPrices.AddRange(result.Items);
                    }
                    else
                    {
                        _logger.LogInformation("No items returned");
                    }

                    url = result?.NextPageLink;
                }
            }

            catch (Exception exception)
            {
                _logger.LogError(exception, "Failed to fetch prices");
                throw;
            }

            return allPrices;
        }

        private static void AddFilter(List<string> filterParts, string fieldName, string? fieldValue)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                return;
            }

            var escapedValue = fieldValue.Replace("'", "''");
            filterParts.Add($"{fieldName} eq '{escapedValue}'");
        }
    }
}

