
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

    public async Task<List<AzurePrice>> GetPricesAsync()
    {
         
         var url = "https://prices.azure.com/api/retail/prices?$filter=armRegionName eq 'westeurope'";

         var resulte = await _httpClient.GetFromJsonAsync<AzurePrice>(url);

      if (resulte != null && resulte.Items != null)
        {
            return resulte.Items;}
            else{
                
                return new List<AzurePrice>();
            }
        }}}

      


