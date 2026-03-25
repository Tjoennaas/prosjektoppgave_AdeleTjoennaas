


// Kilde/Ref: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-10.0&tabs=visual-studio#asynchronous-timed-background-task



using ProsjektOppgave_AdeleTjoennaas.Services;
using ProsjektOppgave_AdeleTjoennaas.Models;
using ProsjektOppgave_AdeleTjoennaas.Data;
using Microsoft.EntityFrameworkCore;

namespace ProsjektOppgave_AdeleTjoennaas.BackgroundTask {


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
/*
 using var scope = Services.CreateScope();
 var price = scope.ServiceProvider.GetRequiredService<AzurePriceService>();   */


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

  


        

       














