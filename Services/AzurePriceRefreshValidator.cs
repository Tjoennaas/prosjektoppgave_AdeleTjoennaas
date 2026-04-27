

/*
using Microsoft.EntityFrameworkCore;
using ProsjektOppgave_AdeleTjoennaas.Data;
using ProsjektOppgave_AdeleTjoennaas.Helpers;
using ProsjektOppgave_AdeleTjoennaas.Models;

namespace ProsjektOppgave_AdeleTjoennaas.Services
{
    public class AzurePriceRefreshValidator
    {
        private readonly PriceDbContext _db;

        public AzurePriceRefreshValidator(PriceDbContext db)
        {
            _db = db;
        }

        public async Task<RefreshStatus> GetRefreshStatusAsync(
            string[] regions,
            string[] currencies,
            PriceQuerySpec[] queries)
        {
            var hasData = await _db.AzurePrices.AnyAsync();

            if (!hasData)
            {
                return new RefreshStatus(
                    HasData: false,
                    IsStale: false,
                    MissingUnitOfMeasure: false,
                    MissingRequestedData: false);
            }

            var latestUpdate = await _db.AzurePrices.MaxAsync(x => x.LastUpdatedUtc);

            var isStale = latestUpdate < DateTime.UtcNow.AddHours(-24);
            var missingUnitOfMeasure = await _db.AzurePrices.AnyAsync(x => x.UnitOfMeasure == null);
            var missingRequestedData = await HasMissingRequestedDataAsync(regions, currencies, queries);

            return new RefreshStatus(
                HasData: true,
                IsStale: isStale,
                MissingUnitOfMeasure: missingUnitOfMeasure,
                MissingRequestedData: missingRequestedData);
        }

        private async Task<bool> HasMissingRequestedDataAsync(
            string[] regions,
            string[] currencies,
            PriceQuerySpec[] queries)
        {
            foreach (var region in regions)
            {
                foreach (var currency in currencies)
                {
                    foreach (var query in queries)
                    {
                        var existingPrices = _db.AzurePrices.Where(price =>
                            price.ArmRegionName == region &&
                            price.CurrencyCode == currency);

                        existingPrices = AzurePriceQueryHelper.ApplyFilter(existingPrices, price => price.ProductName, query.ProductName);
                        existingPrices = AzurePriceQueryHelper.ApplyFilter(existingPrices, price => price.ServiceName, query.ServiceName);
                        existingPrices = AzurePriceQueryHelper.ApplyFilter(existingPrices, price => price.MeterName, query.MeterName);
                        existingPrices = AzurePriceQueryHelper.ApplyFilter(existingPrices, price => price.SkuName, query.SkuName);
                        existingPrices = AzurePriceQueryHelper.ApplyFilter(existingPrices, price => price.ArmSkuName, query.ArmSkuName);
                        existingPrices = AzurePriceQueryHelper.ApplyFilter(existingPrices, price => price.UnitOfMeasure, query.UnitOfMeasure);

                        if (!await existingPrices.AnyAsync())
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}*/