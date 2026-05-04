

        using CostPricingEngine.Services;
        using CostPricingEngine.Data;
        using Microsoft.EntityFrameworkCore;
        using CostPricingEngine.Models.AzureApi;
             
         namespace CostPricingEngine.BackgroundTask{

    public class AzurePriceRefreshService {
        private static readonly string[] SupportedCurrencies = { "USD", "EUR" };
        private static readonly string[] SupportedRegions = {"northeurope" };



        // productName er ikke unikt nok til å identifisere riktig pris rad fra Azure prising API,
        // må derfor bruke mer presis navn OG filtrere for å hente riktig pris informasjon.
        private static readonly PriceQuerySpec[] RequestedPriceQueries = {  
            new("Private Endpoint", ProductName: "Virtual Network Private Link", ServiceName: "Virtual Network", RegionOverride: "Global"),
            new("NAT Gateway", ProductName: "NAT Gateway", RegionOverride: "Global"),
            new("Azure Storage", ServiceName: "Storage"),
            new("Virtual Network", ServiceName: "Virtual Network"),
            new("Public IP Address", ProductName: "IP Addresses"),
            new("Private Endpoint", MeterName: "Private Link Unit"),
            new("Container App Environment", ServiceName: "Azure Container Apps"),
            new("Key Vault", ProductName: "Key Vault"),
            new("Azure Table Storage", ProductName: "Tables"),
            new("Azure Cosmos DB", ProductName: "Azure Cosmos DB"),
            new("Container Apps", ProductName: "Azure Container Apps"),
            new("Container Registry", ProductName: "Container Registry"),
            new("Azure Storage Queue", ProductName: "Queues v2")
        };       

    //Tydliggjøt hvilke tjenester som ikke har prisrader
     private static readonly string[] UnsupportedRetailLabels =
        {
            //"NAT Gateway",
            "Network Security Group",
            "Private DNS Zone",
            "Managed Identity",
            "Front Door"
        };

        private readonly CostDbContext _db;
        private readonly AzurePriceService _azurePriceService;
        private readonly ILogger<AzurePriceRefreshService> _logger;

        public AzurePriceRefreshService(
            CostDbContext db,
            AzurePriceService azurePriceService,
            ILogger<AzurePriceRefreshService> logger)
        {
            _db = db;
            _azurePriceService = azurePriceService;
            _logger = logger;
        }
         
//sjekker om det finnes data, eldre enn 24 timer, om rader eller data mangler
//---------------------------------------------------------------------------//  

        public async Task EnsureDataIsFreshAsync() {

            var hasData = await _db.AzureApiPricesDto.AnyAsync();
            var isStale = false;
            var missingUnitOfMeasure = false;
            var missingRequestedData = false;

            if (hasData)
            {
                var latestUpdate = await _db.AzureApiPricesDto.MaxAsync(x => x.LastUpdatedUtc);
                isStale = latestUpdate < DateTime.UtcNow.AddHours(-24);
                missingUnitOfMeasure = await _db.AzureApiPricesDto.AnyAsync(x => x.UnitOfMeasure == null);
                missingRequestedData = await HasMissingRequestedDataAsync();
            }

            if (hasData && !isStale && !missingUnitOfMeasure && !missingRequestedData)
            {
                _logger.LogInformation("Data already exists.");
                return;
            }

            _logger.LogInformation(
                "Refreshing Azure price data. IsStale: {IsStale}, MissingUnitOfMeasure: {MissingUnitOfMeasure}, MissingRequestedData: {MissingRequestedData}",
                isStale,
                missingUnitOfMeasure,
                missingRequestedData);

            _logger.LogInformation(
                "Skipping labels without direct retail price rows: {Labels}",
                string.Join(", ", UnsupportedRetailLabels));



     //---------------------------------------------------------//   


        List<AzureApiPricesDto> allPrices = new List<AzureApiPricesDto>();

        foreach (var region in SupportedRegions) {

        foreach (var currency in SupportedCurrencies) {

        foreach (var query in RequestedPriceQueries) {
            

        var regionToUse = query.RegionOverride ?? region;

        var prices = await _azurePriceService.GetPricesAsync(
            regionToUse,
            currency,
            productName: query.ProductName,
            serviceName: query.ServiceName,
            meterName: query.MeterName,
            skuName: query.SkuName,
            armSkuName: query.ArmSkuName,
            unitOfMeasure: query.UnitOfMeasure);


                foreach (var price in prices) {
                    price.LastUpdatedUtc = DateTime.UtcNow;
                }

                allPrices.AddRange(prices);  }}}

            //Fjerner duplicater før den lagres i databasen
                allPrices = allPrices
                    .DistinctBy(price => new {
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

                _db.AzureApiPricesDto.RemoveRange(_db.AzureApiPricesDto);
                await _db.SaveChangesAsync();

                await _db.AzureApiPricesDto.AddRangeAsync(allPrices);
                await _db.SaveChangesAsync();
            }


//Isteden for å sjekke om det finnes noe data i databasen, så sjekker denne metoden om databasen innholder alle prisene systemet forventer å ha,
//går igjennom alle regionser, valuta og prisoppslag
    private async Task<bool> HasMissingRequestedDataAsync() {
        foreach (var region in SupportedRegions) {
        foreach (var currency in SupportedCurrencies) {
        foreach (var query in RequestedPriceQueries) {
            var existingPrices = _db.AzureApiPricesDto.Where(price =>
                price.ArmRegionName == region &&
                price.CurrencyCode == currency);

            existingPrices = ApplyFilter(existingPrices, price => price.ProductName, query.ProductName);
            existingPrices = ApplyFilter(existingPrices, price => price.ServiceName, query.ServiceName);
            existingPrices = ApplyFilter(existingPrices, price => price.MeterName, query.MeterName);
            existingPrices = ApplyFilter(existingPrices, price => price.SkuName, query.SkuName);
            existingPrices = ApplyFilter(existingPrices, price => price.ArmSkuName, query.ArmSkuName);
            existingPrices = ApplyFilter(existingPrices, price => price.UnitOfMeasure, query.UnitOfMeasure);

            if (!await existingPrices.AnyAsync()) {
                return true;}}}}
                return false; }

    //---------------------//

    //Metoden leggger til et filter på database spørring, men barevis det finnes en verdi. 
    private static IQueryable<AzureApiPricesDto> ApplyFilter(
        IQueryable<AzureApiPricesDto> query,
        System.Linq.Expressions.Expression<Func<AzureApiPricesDto, string?>> selector,
        string? value) {
        if (string.IsNullOrWhiteSpace(value)) {
            return query; }

        return query.Where(BuildEqualsExpression(selector, value));  }

    //---------------------//


    //Lager selve filtret dynamisk, slik at Aplyfilter() kan bruke den
    private static System.Linq.Expressions.Expression<Func<AzureApiPricesDto, bool>> BuildEqualsExpression(
        System.Linq.Expressions.Expression<Func<AzureApiPricesDto, string?>> selector,
        string value) {
        var parameter = selector.Parameters[0];
        var body = System.Linq.Expressions.Expression.Equal(
            selector.Body,
            System.Linq.Expressions.Expression.Constant(value));

        return System.Linq.Expressions.Expression.Lambda<Func<AzureApiPricesDto, bool>>(body, parameter); }

    private sealed record PriceQuerySpec(
        string Label,
        string? ProductName = null,
        string? ServiceName = null,
        string? MeterName = null,
        string? SkuName = null,
        string? ArmSkuName = null,
        string? UnitOfMeasure = null,
        string? RegionOverride = null);
        }}
































