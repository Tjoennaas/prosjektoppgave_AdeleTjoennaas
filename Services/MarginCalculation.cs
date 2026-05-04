

    
    using CostPricingEngine.Data;
    using CostPricingEngine.Dto;
    using CostPricingEngine.Models.Config;
    using CostPricingEngine.Models.CostMargin;
    using CostPricingEngine.Services.AzureCostCalculator;
        
    namespace CostPricingEngine.Services {
    public class CalculationServices {
        private readonly AzureCostCalculationService _azureCostCalculationService;
        private readonly CustomerCalculator _customerCalculator;
        private readonly CostDbContext _db;
        private readonly ConfigApp _config;

        
    public CalculationServices(
        
        CustomerCalculator customerCalculator,
        CostDbContext db,
        ConfigApp config, 
        AzureCostCalculationService azureCostCalculationService) {

        
        _azureCostCalculationService = azureCostCalculationService;
        _customerCalculator = customerCalculator;
        _db = db;
        _config = config; }

    public async Task<List<CalculationMargin>> CalculateAndSaveAll(CustomerInput input) {
        var results = new List<CalculationMargin>();

        _config.MiscSeting.EventsLoggedPerMonthCount = input.EventsPerPeriod;
        _config.MiscSeting.AverageMonthsOfRetention = input.RetentionPeriods;
        
        var azure = await _azureCostCalculationService.CalculateAndSaveAzureCostAsync();
        var customers = await _customerCalculator.CalculateAndSaveAllAsync(input);
        var groupId = customers.First().CalculationGroupId;

        decimal usdToNok = 9.6m;

    foreach (var customer in customers) {

        decimal customerUsd = customer.TotalPrice / usdToNok;

        var margin = customerUsd - azure.TotalAzureCost;

        var marginPercent =
            customerUsd == 0
                ? 0
                : (margin / customerUsd) * 100;



    var marginEntity = new CalculationMargin {

            AzureCostCalculationId = azure.Id,
            CalculationGroupId = customer.CalculationGroupId,
            PeriodNumber = customer.PeriodNumber,
            Margin = Math.Round(margin, 2),
            MarginPercent = Math.Round(marginPercent, 2) };


            _db.CalculationMargins.Add(marginEntity);
            results.Add(marginEntity);  }

                await _db.SaveChangesAsync();

                return results;  }}};

                













