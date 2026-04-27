


/*

using ProsjektOppgave_AdeleTjoennaas.Services;
using ProsjektOppgave_AdeleTjoennaas.Models;
using ProsjektOppgave_AdeleTjoennaas.Data;
using Microsoft.EntityFrameworkCore;



namespace ProsjektOppgave_AdeleTjoennaas.Services {


public class AzurePriceRefreshService {


        private readonly PriceDbContext _db;
        private readonly AzurePriceService _azurePriceService;
  

        public AzurePriceRefreshService(
            PriceDbContext db,
            AzurePriceService azurePriceService
          )
        {
            _db = db;
            _azurePriceService = azurePriceService;
       
        }

private async Task<bool> MissingRequestedDataAsync() {


            foreach (var region in SupportedRegions)
            {
                foreach (var currency in SupportedCurrencies)
                {
                    foreach (var query in RequestedPriceQueries)
                    {

                        var exsistingData = _db.AzurePrices.Where(
                            price =>
                            price.ArmRegionName == region &&
                            price.CurrencyCode == currency);



                    if (!string.IsNullOrWhiteSpace(query.ProductName))
                    {
                     existingPrices = existingPrices
                     .Where(price => price.ProductName == query.ProductName);
                        
                        }   

                    if (!string.IsNullOrWhiteSpace(query.ServiceName))
                        {
                            existingPrices = existingPrices
                        .Where(price => price.ServiceName == query.ServiceName);
                        }

                    if (!string.IsNullOrWhiteSpace(query.MeterName))
                        {
                         existingPrices = existingPrices
                        .Where(price => price.MeterName == query.MeterName);
                        }

                    if (!string.IsNullOrWhiteSpace(query.SkuName))
                        {
                         existingPrices = existingPrices
                        .Where(price => price.SkuName == query.SkuName);
                        }
                    
                    if (!string.IsNullOrWhiteSpace(query.ArmSkuName))
                        {
                         existingPrices = existingPrices
                        .Where(price => price.ArmSkuName == query.ArmSkuName);
                        }
                        
                    if (!string.IsNullOrWhiteSpace(query.UnitOfMeasure))
                        {
                        existingPrices = existingPrices
                        .Where(price => price.UnitOfMeasure == query.UnitOfMeasure);
                        }
                    if (!await exsistingPrice.AnyAcync()){

                        return true; // Mangler data
                            
                        }

}}}} 
                        return false; //Mangler ikke data
                     
}}
*/