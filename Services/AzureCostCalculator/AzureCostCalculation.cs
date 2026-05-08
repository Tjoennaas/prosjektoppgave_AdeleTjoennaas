

// Klassen beregner totale Azure-kostnader og lagrer resultatet i databasen.
// Se vedlegg.3 "TOTAL COSTS FORMULA"

using CostPricingEngine.Data;
using CostPricingEngine.Models.Config;
using CostPricingEngine.Models.CostCalculation;

namespace CostPricingEngine.Services.AzureCostCalculator;

public class AzureCostCalculationService {
    private readonly CostDbContext _db;
    private readonly ConfigApp _configApp;
    private readonly AzurePricingService _azurePricingService;
    private readonly CalculatorFixdCosts _fixedCostCalculator;
    private readonly AzureCostCalculator _variableCostCalculator;

    public AzureCostCalculationService(
        CostDbContext db,
        ConfigApp configApp,
        AzurePricingService azurePricingService,
        CalculatorFixdCosts fixedCostCalculator,
        AzureCostCalculator variableCostCalculator) {
        _db = db;
        _configApp = configApp;
        _azurePricingService = azurePricingService;
        _fixedCostCalculator = fixedCostCalculator;
        _variableCostCalculator = variableCostCalculator;
    }



//(fixedCosts + variableCosts) * simplificationHedgeFactor * currencyDevaluationHedgeFactor * sellerCommisionFactor

    public async Task<AzureCostCalculation> CalculateAndSaveAzureCostAsync(string currency = "USD") {
        var azureCostResult = await _azurePricingService.GetAzureCostPricesAsync(currency);

        var fixedCosts = _fixedCostCalculator.CalculatorFixdCost(azureCostResult);

        var variableResult = _variableCostCalculator.CalculateVariableCosts(azureCostResult);

        var totalAzureCost =
            (fixedCosts + variableResult.TotalVariableCosts)
            * _configApp.MiscSeting.SimplificationHedgeFactor
            * _configApp.MiscSeting.CurrencyDevaluationHedge
            * _configApp.MiscSeting.SellerCommissionFactor;

        var calculation = new AzureCostCalculation
        {
            FixedCosts = fixedCosts,
            VariableCosts = variableResult.TotalVariableCosts,
            TotalAzureCost = totalAzureCost,
            PerMillionEventsReceived = variableResult.PerMillionEventsReceived,
            PerMillionEventsRetained = variableResult.PerMillionEventsRetained
        };

       _db.AzureCostCalculations.Add(calculation);
      
        await _db.SaveChangesAsync();

        return  calculation ;
    }
} 




