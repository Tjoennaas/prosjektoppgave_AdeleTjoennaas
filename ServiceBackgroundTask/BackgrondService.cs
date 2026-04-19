

using ProsjektOppgave_AdeleTjoennaas.Services;
using ProsjektOppgave_AdeleTjoennaas.Models;
using ProsjektOppgave_AdeleTjoennaas.Data;
using Microsoft.EntityFrameworkCore;

namespace ProsjektOppgave_AdeleTjoennaas.BackgroundTask
{
    public class AzurePriceRefreshService
    {
        private readonly PriceAzureContext _db;
        private readonly AzurePriceService _azurePriceService;
        private readonly ILogger<AzurePriceRefreshService> _logger;

        public AzurePriceRefreshService(
            PriceAzureContext db,
            AzurePriceService azurePriceService,
            ILogger<AzurePriceRefreshService> logger)
        {
            _db = db;
            _azurePriceService = azurePriceService;
            _logger = logger;
        }

        public async Task EnsureDataIsFreshAsync()
        {
            var hasData = await _db.AzurePrices.AnyAsync();
            var isStale = false;

            if (hasData)
            {
                var latestUpdate = await _db.AzurePrices.MaxAsync(x => x.LastUpdatedUtc);
                isStale = latestUpdate < DateTime.UtcNow.AddHours(-24);
            }

            if (hasData && !isStale)
            {
                _logger.LogInformation("Data already exists.");
                return;
            }

            _logger.LogInformation("Data is older than 24 hours. Fetching new data from Azure.");

            var currencies = new[] { "USD", "EUR" };
            var regions = new[] { "northeurope" };
            var productNames = new[]
            {
                "Azure Table Storage",
                "Azure Cosmos DB",
                "Container Apps",
                "Container Registry",
                "Azure Storage Queue"
            };

            List<AzurePrice> allPrices = new List<AzurePrice>();

            foreach (var region in regions)
            {
                foreach (var currency in currencies)
                {
                    foreach (var product in productNames)
                    {
                        var prices = await _azurePriceService.GetPricesAsync(product, region, currency);

                        foreach (var price in prices)
                        {
                            price.LastUpdatedUtc = DateTime.UtcNow;
                        }

                        allPrices.AddRange(prices);
                    }
                }
            }

            _db.AzurePrices.RemoveRange(_db.AzurePrices);
            await _db.SaveChangesAsync();

            await _db.AzurePrices.AddRangeAsync(allPrices);
            await _db.SaveChangesAsync();
        }
    }
}



       














