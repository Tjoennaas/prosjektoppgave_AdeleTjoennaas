
/*
using Microsoft.EntityFrameworkCore;
using ProsjektOppgave_AdeleTjoennaas.Data;
using ProsjektOppgave_AdeleTjoennaas.Models;

namespace ProsjektOppgave_AdeleTjoennaas.Services
{
    public class AzurePriceStorageService
    {
        private readonly PriceDbContext _db;

        public AzurePriceStorageService(PriceDbContext db)
        {
            _db = db;
        }

        public List<AzurePrice> RemoveDuplicates(List<AzurePrice> prices)
        {
            return prices
                .DistinctBy(price => new
                {
                    price.CurrencyCode,
                    price.ArmRegionName,
                    price.ProductName,
                    price.ProductId,
                    price.SkuId,
                    price.SkuName,
                    price.MeterId,
                    price.MeterName,
                    price.UnitOfMeasure,
                    price.Type,
                    price.RetailPrice,
                    price.UnitPrice
                })
                .ToList();
        }

        public async Task ReplacePricesAsync(List<AzurePrice> prices)
        {
            _db.AzurePrices.RemoveRange(_db.AzurePrices);
            await _db.SaveChangesAsync();

            await _db.AzurePrices.AddRangeAsync(prices);
            await _db.SaveChangesAsync();
        }
    }
}*/