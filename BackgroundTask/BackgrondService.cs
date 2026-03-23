
using ProsjektOppgave_AdeleTjoennaas.Services;
using ProsjektOppgave_AdeleTjoennaas.Models;

namespace ProsjektOppgave_AdeleTjoennaas.BackgroundTask {


public class AzureBackgroundService : BackgroundService {
       private readonly ILogger<AzureBackgroundService> _logger;

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
            var price = 
                scope.ServiceProvider
                    .GetRequiredService<AzurePriceService>();         
        
/*
 using var scope = Services.CreateScope();
 var price = scope.ServiceProvider.GetRequiredService<AzurePriceService>();   */

 var currencies = new []{"USD", "EUR", "GBP"};
 var allPrices = new List<AzurePrice>();

foreach(var currency in currencies)
{  var prices = await price.GetPricesAsync(currency);
   allPrices.AddRange(prices);
  
}}}

catch (Exception ex)
            {
                _logger.LogError(ex, "Feil ved henting av Azure-priser.");
            
            }}}}


        

       














