


// Kilde/Ref: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-10.0&tabs=visual-studio#asynchronous-timed-background-task



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
                _logger.LogInformation("Data finnes allerede og er fortsatt gyldige.");
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






/*

public class AzureBackgroundService : BackgroundService {
       private readonly ILogger<AzureBackgroundService> _logger;
       // private object dbContext;

        public AzureBackgroundService(IServiceProvider services, 
        ILogger<AzureBackgroundService> logger) {  

        Services = services;
        _logger = logger;
 }
    public IServiceProvider Services { get; }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Service starter.");

        await DoWork(stoppingToken);

        using PeriodicTimer timer = new(TimeSpan.FromHours(24));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await DoWork(stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Service stoppet.");
        }}
        

   //----------------------------------------------------------------// 


    private async Task DoWork(CancellationToken stoppingToken){
    try {
         _logger.LogInformation("Starter henting av informasjon fra Azure");

    using (var scope = Services.CreateScope())
        {
            var price = scope.ServiceProvider.GetRequiredService<AzurePriceService>();         
            var dbContext = scope.ServiceProvider.GetRequiredService<PriceAzureContext>();



 var currencies = new []{"USD", "EUR", "GBP"};
var regions = new[] { "westeurope", "northeurope" };
 var allPrices = new List<AzurePrice>();


   foreach (var region in regions)
        {
            foreach (var currency in currencies)
            {
                var prices = await price.GetPricesAsync( region,currency);
                allPrices.AddRange(prices);
            }
        }

    
        dbContext.AzurePrices.RemoveRange(dbContext.AzurePrices);
        await dbContext.SaveChangesAsync(stoppingToken);

        await dbContext.AzurePrices.AddRangeAsync(allPrices, stoppingToken);
        await dbContext.SaveChangesAsync(stoppingToken);}}

catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av Azure-priser.");
            
            }}}};

  */


        

       














