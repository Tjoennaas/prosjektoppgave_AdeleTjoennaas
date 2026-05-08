

/*Beregner de faste kostnadene i løsningen.
 Klassen bruker prisdata fra AzurePricingData 
 sammen med konfigurasjonsverdier fra appsettings.json. 
 Til slutt summeres delkostnadene og returnerer total fast kostand */

// Se vedlegg.3 "kafka" og "FIXED COSTS FORMULAS"

    using CostPricingEngine.Models.Config;
    using CostPricingEngine.Models.CostCalculation;
    using CostPricingEngine.Data;

          
     namespace CostPricingEngine.Services.AzureCostCalculator {
             
            
    public class  CalculatorFixdCosts {

    private readonly CostDbContext _db;
    private readonly ConfigApp _configApp;
    public  CalculatorFixdCosts(CostDbContext db, ConfigApp configApp) {
        _db = db;
        _configApp = configApp; }


    //Each Container App: (vCoresCount / 0,25) * containerAppsPerQuarterVCorePrice + (ramAmount / 0,5) * 
    //containerAppsPerHalfGbPrice
    public decimal CalculatorFixdCost( AzurePricingData  azureCostResult){
    decimal containerAppsCost = 0m;

            foreach (var app in _configApp.ContainerApps) {
            containerAppsCost +=
            (app.VCoresCount / 0.25m) *  azureCostResult.ContainerAppsPriceOfVcpuSeconds
            +
            (app.RamAmount / 0.5m) *  azureCostResult.ContainerAppsPriceOfGibSeconds; }


    //NAT Gateway: natGatewayFixedCost * 1 :  feks sum 38,8 * 1 
            var natGateway =  azureCostResult.NatGatewayCostPerHour * 1;


    //Static IP Address: staticIpFixedCost * 1
            var staticIpAdress =  azureCostResult.StaticIpAddressPricePerHour * 1;  
            
                                    
    //Private Endpoints: privateEndpointFixedCost * 4 
           var privateEndpoints =  azureCostResult.PrivatEndpointPricePerHour * 4;
        
    
    //Employees: employeeAvgMonthlyCost / employeesNeededPerCustomer
           var employees = _configApp.MiscCost.EmployeeAvgMonthlyCost / _configApp.MiscSeting.EmployeeNumNeededPerCustomer;
  
                            
    //CosmosDB: costPerHundreRus * (cosmosDbMaxRus / 100) * cosmosDbAvgBilledFactor
          var cosmosDb =  azureCostResult.CosmosDbPricedByHundredRusApiRespons * 
            (_configApp.MiscSeting.CosmosDbMaxRus / 100m) * 
            _configApp.MiscSeting.CosmosDbAverageBilledRuFacto;

        
    //Auth: workOsSsoCostPerMonth * 1 
         var auth = _configApp.MiscSeting.WorkOsSsoCostPerMonth * 1;

    // Total fixed costs: sum(foreach containerAppCost in containerApps) + natGateway + staticIpAddress + privateEndpoints + employees + cosmosDb + auth  
         var totalFixedCosts =
                containerAppsCost +
                natGateway +
                staticIpAdress +
                privateEndpoints +
                employees +
                cosmosDb +
                auth;

            return totalFixedCosts; }}}