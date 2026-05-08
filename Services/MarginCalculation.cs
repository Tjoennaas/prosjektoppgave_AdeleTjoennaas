

//Fikk hjelp av ChatGPT til å koble samme tabellene med groupID og hvordan å gjøre NOK til USD

using System.Security.Cryptography.X509Certificates;
using CostPricingEngine.Data;
    using CostPricingEngine.Dto;
    using CostPricingEngine.Models.Config;
    using CostPricingEngine.Models.CostMargin;
    using CostPricingEngine.Services.AzureCostCalculator;
        
    namespace CostPricingEngine.Services {
    public class MarginCalculation(

        CustomerCalculator customerCalculator,
        CostDbContext db,
        ConfigApp config,
        AzureCostCalculationService azureCostCalculationService)
    {
        private readonly AzureCostCalculationService _azureCostCalculationService = azureCostCalculationService;
        private readonly CustomerCalculator _customerCalculator = customerCalculator;
        private readonly CostDbContext _db = db;
        private readonly ConfigApp _config = config;

        public async Task<List<CalculationMargin>> CalculateAndSaveAll(CustomerInput input) {
        var results = new List<CalculationMargin>();

        // Her legges kundeinput inn i config-verdiene, 
        // slik at Azure-kostnadsberegningen bruker riktig antall events og retention-perioder.
        _config.MiscSeting.EventsLoggedPerMonthCount = input.EventsPerPeriod;
        _config.MiscSeting.AverageMonthsOfRetention = input.RetentionPeriods;
        
        //Her beregnes Azure-kostnaden og lagres i databasen.
        var azure = await _azureCostCalculationService.CalculateAndSaveAzureCostAsync();
        //Her beregnes kundeprisene og lagres i databasen.
        var customers = await _customerCalculator.CalculateAndSaveAllAsync(input);
      

  foreach (var customer in customers)
    {
        decimal customerUsd =
            customer.TotalPrice / _config.MiscSeting.CurrencyUsdToNokFactor;

        var margin = customerUsd - azure.TotalAzureCost;

        var marginPercent = (margin / customerUsd) * 100;

 
//Innholder resutat av margin bergningen
    var marginEntity = new CalculationMargin {

            AzureCostCalculationId = azure.AzureCostCalculationId,
            CalculationGroupId = customer.CalculationGroupId,
            PeriodNumber = customer.PeriodNumber,        
            Margin = Math.Round(margin, 2),
            MarginPercent = Math.Round(marginPercent, 2) };

             //Lagres i databasen
            _db.CalculationMargins.Add(marginEntity);
            results.Add(marginEntity);  }

                await _db.SaveChangesAsync();

                return results;  }}};







