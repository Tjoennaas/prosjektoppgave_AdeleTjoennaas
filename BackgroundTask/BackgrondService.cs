




using ProsjektOppgave_AdeleTjoennaas.Services;
using ProsjektOppgave_AdeleTjoennaas.Models;
using ProsjektOppgave_AdeleTjoennaas.Data;
using Microsoft.EntityFrameworkCore;

namespace ProsjektOppgave_AdeleTjoennaas.BackgroundTask {


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
                _logger.LogInformation("Data finnes allerede og er gyldige.");
                return;
            }

            _logger.LogInformation("Data eldre enn 24 timer. Henter nye data fra Azure.");

            var currencies = new[] { "USD", "EUR", "GBP" };
            var regions = new[] { "westeurope", "northeurope" };

            var allPrices = new List<AzurePrice>();

            foreach (var region in regions)
            {
                foreach (var currency in currencies)
                {
                    var prices = await _azurePriceService.GetPricesAsync(region, currency);

                    foreach (var price in prices)
                    {
                        price.LastUpdatedUtc = DateTime.UtcNow;
                    }

                    allPrices.AddRange(prices);
                }
            }

            _db.AzurePrices.RemoveRange(_db.AzurePrices);
            await _db.SaveChangesAsync();

            await _db.AzurePrices.AddRangeAsync(allPrices);
            await _db.SaveChangesAsync();}}};





       














