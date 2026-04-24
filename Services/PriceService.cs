

using System.Net;
using Microsoft.EntityFrameworkCore.Design;
using ProsjektOppgave_AdeleTjoennaas.Models;
using prosjektoppgave_AdeleTjoennaas.Middleware;


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

   // Parameter som skal brukes til å filtrere fra Azure API
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
             if(string.IsNullOrEmpty(region))
            {
                throw new ArgumentException("Region must be provided");
            }

            if (string.IsNullOrEmpty(currency))
            {
                throw new ArgumentException("Currensy must be provided");
            }

            var allPrices = new List<AzurePrice>();
            //Starter filtrering av region, den vil altid være tilstede mens de andre parameteren kan være null
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


              while (!string.IsNullOrEmpty(url))
                {
                    var response = await _httpClient.GetAsync(url);
                    
                    if (!response.IsSuccessStatusCode){  
                     throw new HttpRequestException($"{response.StatusCode}");   
                }
                    var result = await _httpClient.GetFromJsonAsync<AzureResponse>(url);
                     
                   if (result?.Items == null)
                    {
                        throw new KeyNotFoundException("Ingen data funnet.");
                    }
                        allPrices.AddRange(result.Items);
                        url = result?.NextPageLink;
                        }

                             return allPrices;  
                    }


//---------------------------//


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

