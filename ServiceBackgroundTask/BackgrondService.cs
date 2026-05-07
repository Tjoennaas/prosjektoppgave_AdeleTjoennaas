



// ChatGPT ble brukt i denne delen av koden,
// fordi problemstillingen rundt dynamisk filtrering gjorde det vanskelig å finne en god løsning alene.
// Det gjelder spesielt funsjonen HasMissingRequestedDataAsync()



        using CostPricingEngine.Services;
        using CostPricingEngine.Data;
        using Microsoft.EntityFrameworkCore;
        using CostPricingEngine.Models.AzureApi;
             
         namespace CostPricingEngine.BackgroundTask{

    public class AzurePriceRefreshService {
        private static readonly string[] SupportedCurrencies = { "USD", "EUR" };
        private static readonly string[] SupportedRegions = {"northeurope" };


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
            
            "Network Security Group",
            "Private DNS Zone",
            "Managed Identity",
            "Front Door"
        };

        private readonly CostDbContext _db;
        private readonly AzureRetailPriceApiService _azurePriceService;
        private readonly ILogger<AzurePriceRefreshService> _logger;

        public AzurePriceRefreshService(
            CostDbContext db,
            AzureRetailPriceApiService azurePriceService,
            ILogger<AzurePriceRefreshService> logger)
        {
            _db = db;
            _azurePriceService = azurePriceService;
            _logger = logger;
        }
         

//---------------------------------------------------------------------------//  

//  Her sjekkes det om data i databasen er eldre enn 24 timer.
//  Databasen har pris data.
//  Sjekker om noen rader mangler data.
//  Dersom alt er OK stopper metoden, vis ikke går den over til neste metode og må hente data. 
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

        //Dersom mangler data i databasen: 
        // Lager en tom liste allPrices, med filtrering og derretter gjør et kall på 
        // AzureRetailPriceApiService som deretter henter Azure Retaile Price API. 
        

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

            //Fjerner duplicater, slik at samme prisrad ikke lagres flere ganger.
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

               //Sletter gammle prisrader fra databasen og lagrer ny
                _db.AzureApiPricesDto.RemoveRange(_db.AzureApiPricesDto);
                await _db.SaveChangesAsync();

                await _db.AzureApiPricesDto.AddRangeAsync(allPrices);
                await _db.SaveChangesAsync();  }




    //Metoden sjekker at databasen inneholder alle nødvendige prisrader,
    //ikke bare at den inneholder noe data.

    private async Task<bool> HasMissingRequestedDataAsync() {
        foreach (var region in SupportedRegions) {
        foreach (var currency in SupportedCurrencies) {
        foreach (var query in RequestedPriceQueries) {
            var existingPrices = _db.AzureApiPricesDto.Where(price =>
                price.ArmRegionName == region &&
                price.CurrencyCode == currency);

  //Deretter legger den på filtre, men bare for verdier som finnes:
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

        //Hjelpe funksjon som brukes av HasMissingRequestedDataAsync,
        //den legger til et filter i databasesøket, men bare hvis det finnes en verdi.
        //Ikke alle prisradene fra Azure API har verdi i alle felter.

    private static IQueryable<AzureApiPricesDto> ApplyFilter(
        IQueryable<AzureApiPricesDto> query,
        System.Linq.Expressions.Expression<Func<AzureApiPricesDto, string?>> selector,
        string? value) {
        if (string.IsNullOrWhiteSpace(value)) {
            return query; }

        return query.Where(BuildEqualsExpression(selector, value));  }

    //---------------------//

   // Lager en sammenligning slik at databasen kan filtrere riktige prisrader.
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
































