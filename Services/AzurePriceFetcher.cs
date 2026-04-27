

/*

using ProsjektOppgave_AdeleTjoennaas.Models;

namespace ProsjektOppgave_AdeleTjoennaas.Services
{
    public class AzurePriceFetcher
    {
        private readonly AzurePriceService _azurePriceService;

        public AzurePriceFetcher(AzurePriceService azurePriceService)
        {
            _azurePriceService = azurePriceService;
        }

        public async Task<List<AzurePrice>> FetchAllPricesAsync(
            string[] regions,
            string[] currencies,
            PriceQuerySpec[] queries)
        {
            var allPrices = new List<AzurePrice>();

            foreach (var region in regions)
            {
                foreach (var currency in currencies)
                {
                    foreach (var query in queries)
                    {
                        var prices = await _azurePriceService.GetPricesAsync(
                            region,
                            currency,
                            productName: query.ProductName,
                            serviceName: query.ServiceName,
                            meterName: query.MeterName,
                            skuName: query.SkuName,
                            armSkuName: query.ArmSkuName,
                            unitOfMeasure: query.UnitOfMeasure);

                        foreach (var price in prices)
                        {
                            price.LastUpdatedUtc = DateTime.UtcNow;
                        }

                        allPrices.AddRange(prices);
                    }
                }
            }

            return allPrices;
        }
    }
}*/