

using ProsjektOppgave_AdeleTjoennaas.Models;
using System.Net.Http.Json;


namespace ProsjektOppgave_AdeleTjoennaas.Services{



   public class AzurePriceService
    {
        private readonly HttpClient _httpClient;

        public AzurePriceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }



       /* internal static async Task<IEnumerable<AzurePrice>> GetPricesAsync(string region, string currency)
        {
            throw new NotImplementedException(); 
        }*/

        public async Task<List<AzurePrice>>GetPricesAsync(string region, string currency)
    {
         
         var url = $"https://prices.azure.com/api/retail/prices?currencyCode='{currency}'&$filter=armRegionName eq '{region}'";

         var result = await _httpClient.GetFromJsonAsync<AzureResponse>(url);

    
            return result?.Items ?? new List<AzurePrice>();

    }}}

      


