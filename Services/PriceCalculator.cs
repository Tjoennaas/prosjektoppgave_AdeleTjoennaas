

using ProsjektOppgave_AdeleTjoennaas.Data;
using ProsjektOppgave_AdeleTjoennaas.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using ProsjektOppgave_AdeleTjoennaas.Dto;

namespace ProsjektOppgave_AdeleTjoennaas.Services
{

    public class PriceCalculator {


        private readonly PriceDbContext _db;
        private readonly ConfigApp _config;

        public PriceCalculator(PriceDbContext db, ConfigApp config)
        {

            _db = db;
            _config = config;
        }

        //Her henter jeg priser fra databasen
        public async Task<AzureCostResult> CalculateAsync(string currency = "USD")
        {       
            var staticIpAddress = await GetPriceAsync(

                productName: "IP Addresses",
                meterName: "Basic IPv4 Static Public IP",
                unitOfMeasure: "1 Hour",
                currency: currency
            );

        var privatEndpointPerGibWritten = await GetPriceAsync(

            productName: "Virtual Network Private Link",
            meterName: "Standard Data Processed - Ingress",
            unitOfMeasure: "1 GB",
            currency: currency,
            armRegionName: "Global"
            );
          
        var privatEndpointPricePerHour = await GetPriceAsync(
            productName: "Virtual Network Private Link",
            serviceName: "Virtual Network",
            meterName: "Standard Private Endpoint",
            unitOfMeasure: "1 Hour",
            currency: currency,
            armRegionName: "Global"
);


            var cpuPrice = await GetPriceAsync(

                serviceName: "Azure Container Apps",
                meterName: "Standard vCPU Active Usage",
                unitOfMeasure: "1 Second",
                currency: currency);

            var gibPrice = await GetPriceAsync(

                serviceName: "Azure Container Apps",
                meterName: "Standard Memory Active Usage",
                unitOfMeasure: "1 GiB Second",
                currency: currency);

            var cosmosDbHourPrice = await GetPriceAsync(

                serviceName: "Azure Cosmos DB",
                meterName: "100 RU/s",
                unitOfMeasure: "1/Hour",
                currency: currency);

            var natGatewayCostPerHour = await GetPriceAsync(

                productName: "NAT Gateway",
                serviceName: "NAT Gateway",
                meterName: "Standard Gateway",
                unitOfMeasure: "1 Hour",
                currency: currency,
                armRegionName: "Global"
            );

            var natGatewayCostPerGiB = await GetPriceAsync(

                productName: "NAT Gateway",
                serviceName: "NAT Gateway",
                meterName: "Standard Data Processed",
                unitOfMeasure: "1 GB",
                currency: currency,
                armRegionName: "Global"
            );

         

            var storagePerBlobWriteForTXs = await GetPriceAsync(

                productName: "General Block Blob v2",
                meterName: "Hot ZRS Write Operations",
                skuName: "Hot ZRS",
                unitOfMeasure: "10K",
                currency: currency
                );


            var storagePerBlobWriteForAttachments = await GetPriceAsync(

                productName: "General Block Blob v2",
                meterName: "Cold ZRS Write Operations",
                skuName: "Cold ZRS",
                unitOfMeasure: "10K",
                currency: currency
                );

            var storagPerGibBlobStorageForTxs = await GetPriceAsync(

                productName: "General Block Blob v2",
                meterName: "Hot ZRS Data Stored",
                skuName: "Hot ZRS",
                unitOfMeasure: "1 GB/Month",
                currency: currency
                );

            var storagePerGibBlobStorageForAttachments = await GetPriceAsync(

                productName: "General Block Blob v2",
                meterName: "Cold ZRS Data Stored",
                skuName: "Cold ZRS",
                unitOfMeasure: "1 GB/Month",
                currency: currency
                );

            var storagePerTableWrite = await GetPriceAsync(

                productName: "Tables",
                meterName: "Batch Write Operations",
                skuName: "Standard ZRS",
                unitOfMeasure: "10K",
                currency: currency);

            var storagePerGiBTableStorage = await GetPriceAsync(

                productName: "Tables",
                meterName: "ZRS Data Stored",
                skuName: "Standard ZRS",
                unitOfMeasure: "1 GB/Month",
                currency: currency);


        return new AzureCostResult {

         
        
        //Networking - Static IP Address = [price per hour] * 730 = $2,628
              StaticIpAddressPricePerHour  = staticIpAddress * 730, 
    
        //Networking - Private Endpoint = [price per hour] * 730 = $7,3
              PrivatEndpointPricePerHour = privatEndpointPricePerHour * 730,

        //Networking - Private Endpoint per GiB written = [price for 0 to 1 PB inbound data processed] = $0,1
              PrivatEndpointPerGibWritten = privatEndpointPerGibWritten,

         //Networking - NAT Gateway = [cost per GiB processed] = $0.045
              NatGatewayCostPerGibprocessed = natGatewayCostPerGiB,    
           
        //Container Apps - Per 0.25 vCores = ([price of vCPU seconds] * 60 * 60 * 730) / 4 
              ContainerAppsPriceOfVcpuSeconds = (cpuPrice * 60 * 60 * 730) / 4,
        
        //Container Apps - Per 0.5 GiB RAM = ([price of GiB seconds] * 60 * 60 * 730) / 2 = $5,255   
              ContainerAppsPriceOfGibSeconds = (gibPrice * 60 * 60 * 730) / 2,

        //CosmosDB - Per 100 RUs = [probably priced by 100 RUs in API response, but if not, convert to per 100 RU/s] = $8,76
              CosmosDbPricedByHundredRusApiRespons = cosmosDbHourPrice * 730,

        //Networking - NAT Gateway = [cost per hour] * 730 = $32,85 
              NatGatewayCostPerHour = natGatewayCostPerHour * 730,

       //Storage - Per Blob Write for TXs = [ZRS Hot 10k transactions cost] / 10000 = $0,00000675
              StoragPerBlobWriteForTxs = storagePerBlobWriteForTXs / 10000,

       //Storage - Per Blob Write for attachments = [ZRS Cold 10k transactions cost] / 10000 = $0,00002
              StoragPerBlobWriteForAttachment = storagePerBlobWriteForAttachments / 10000,
          
       //Storage - Per GiB Blob Storage for TXs = [ZRS Hot First 50 TiB tier per GiB] = $0,024    (1 GB/Month)
              StoragPerGibBlobStoragForTxs = storagPerGibBlobStorageForTxs,

       //Storage - Per GiB Blob Storage for attachments = [ZRS Cold First 50 TiB tier per GiB] = $0,005   (1 GB/Month)
              StoragPerGibBlobStoragForAttacments = storagePerGibBlobStorageForAttachments,

       //Storage - Per Table Write = [10k Transactions cost] / 10000 = $0,000000036
              StoragPerTableWrite = storagePerTableWrite / 10000,

      //Storage - Per GiB Table Storage = [ZRS] = $0,0562     (1 GB/Month)
              StoragPerGibTableStorag = storagePerGiBTableStorage,

        };}
        
        //Filtrerer for å finne riktig rad
         
       private async Task<decimal> GetPriceAsync
       (
             string? productName = null, 
             string? serviceName = null, 
             string? meterName = null,
             string? skuName = null,
             string? unitOfMeasure = null, 
             string? currency = "USD",
             string? armRegionName = null)

       {  
        
        var price = await _db.AzurePrices
    .Where(x => string.IsNullOrEmpty(productName) || x.ProductName == productName)
    .Where(x => string.IsNullOrEmpty(serviceName) || x.ServiceName == serviceName)
    .Where(x => string.IsNullOrEmpty(meterName) || x.MeterName == meterName)
    .Where(x => string.IsNullOrEmpty(skuName) || x.SkuName == skuName)
    .Where(x => string.IsNullOrEmpty(unitOfMeasure) || x.UnitOfMeasure == unitOfMeasure)
    .Where(x => string.IsNullOrEmpty(currency) || x.CurrencyCode == currency)
    .Where(x => string.IsNullOrEmpty(armRegionName) || x.ArmRegionName == armRegionName)
    .Select(x => x.RetailPrice)
    .FirstAsync();

         return price;
        
        
        
        /*var price = await _db.AzurePrices
    
         .Where (x => string.IsNullOrEmpty(productName) || x.ProductName == productName)
         .Where (x => string.IsNullOrEmpty(serviceName) || x.ServiceName == serviceName)
         .Where (x => string.IsNullOrEmpty(meterName) || x.MeterName == meterName)
         .Where (x => string.IsNullOrEmpty(skuName) || x.SkuName == skuName)
         .Where (x => string.IsNullOrEmpty(unitOfMeasure) || x.UnitOfMeasure == unitOfMeasure)
         .Where (x => string.IsNullOrEmpty(currency) || x.CurrencyCode == currency)
         .Where (x => string.IsNullOrEmpty(armRegionName) || x.ArmRegionName == armRegionName)
         .OrderByDescending(x => x.RetailPrice)
         .Select(x => x.RetailPrice)
         .FirstAsync();
         
         return price;*/
       }
    
             public decimal CalculatorFixdCosts(AzureCostResult azureCostResult){
             
   
            decimal containerAppsCost = 0m;

                    foreach (var app in _config.ContainerApps) {
                    containerAppsCost +=
                        (app.VCoresCount / 0.25m) * azureCostResult.ContainerAppsPriceOfVcpuSeconds
                        +
                        (app.RamAmount / 0.5m) * azureCostResult.ContainerAppsPriceOfGibSeconds;
            }
   

            //NAT Gateway: natGatewayFixedCost * 1 :  feks sum 38,8 * 1 
             var natGateway = azureCostResult.NatGatewayCostPerHour * 1;


             //Static IP Address: staticIpFixedCost * 1
             var staticIpAdress = azureCostResult.StaticIpAddressPricePerHour * 1;  
                
                                       
            //Private Endpoints: privateEndpointFixedCost * 4 
            var privateEndpoints = azureCostResult.PrivatEndpointPricePerHour * 4;
          
        
            //Employees: employeeAvgMonthlyCost / employeesNeededPerCustomer
            var employees = _config.MiscCost.EmployeeAvgMonthlyCost / _config.MiscSeting.EmployeeNumNeededPerCustomer;

                              
            //CosmosDB: costPerHundreRus * (cosmosDbMaxRus / 100) * cosmosDbAvgBilledFactor
            var cosmosDb = azureCostResult.CosmosDbPricedByHundredRusApiRespons * 
                (_config.MiscSeting.CosmosDbMaxRus / 100m) * 
                _config.MiscSeting.CosmosDbAverageBilledRuFacto;
 
            
            //Auth: workOsSsoCostPerMonth * 1 : 125 * 1
            var auth = _config.MiscSeting.WorkOsSsoCostPerMonth * 1;

            var totalFixedCosts =
              containerAppsCost +
              natGateway +
              staticIpAdress +
              privateEndpoints +
              employees +
              cosmosDb +
              auth;

            return totalFixedCosts;
}


private IntermediateCostVariables CalculateIntermediateVariables() {



var totalRetainedEventMonths = 0m;

    for (int i = 1; i < _config.MiscSeting.AverageMonthsOfRetention; i++)
    {
        totalRetainedEventMonths += 
            _config.MiscSeting.EventsLoggedPerMonthCount * i;
    }



            var oneWayDataTransferGbKafka =
                    (_config.MiscSeting.AvgSizeOfEventInKafka *
                    _config.MiscSeting.EventsLoggedPerMonthCount)
                    / 1024m / 1024m / 1024m;         

            // blobTxsLoggedGb: (avgSizeOfEventInTxStorage * eventsLoggedPerMonthCount) / 1024 / 1024 / 1024
            var blobTxsLoggedGb = 
                    (_config.MiscSeting.AvgSizeOfEventdecimaTxStorage *
                     _config.MiscSeting.EventsLoggedPerMonthCount)
                      / 1024 / 1024 / 1024;

            // blobAttachmentsLoggedGb: (avgSizeOfStoredAttachment * eventsLoggedPerMonthCount * avgAttachmentsPerEventCount) / 1024 / 1024 / 1024
            var blobAttachmentsLoggedGb = 
                    (_config.MiscSeting.AvgSizeOfStoredAttachment *
                    _config.MiscSeting.EventsLoggedPerMonthCount *
                    _config.MiscSeting.AvgAttachmentsPerEventCount)
                    / 1024m / 1024m / 1024m;

            var tablesLoggedGb =
                    (_config.MiscSeting.AvgSizeOfIndexedEventdecimaTxStorage*
                     _config.MiscSeting.EventsLoggedPerMonthCount)
                     / 1024 / 1024 / 1024;

            return new IntermediateCostVariables {
                    TotalRetainedEventMonths = totalRetainedEventMonths,
                    OneWayDataTransferGbKafka = oneWayDataTransferGbKafka,
                    BlobTxsLoggedGb = blobTxsLoggedGb,
                    BlobAttachmentsLoggedGb = blobAttachmentsLoggedGb,
                    TablesLoggedGb = tablesLoggedGb
                };}

 private VariableCostResult CalculateVariableCosts(AzureCostResult azureCostResult) {
 
       var intermediate = CalculateIntermediateVariables();


//Blob TXs logged: (eventsLoggedPerMonthCount / avgEventsPerBatchIngestedCount) * blobTxsPerWrite  

           var blobTxsLogged = 
                (_config.MiscSeting.EventsLoggedPerMonthCount /
                 _config.MiscSeting.AvgEventsPerBatchIngestedCount) *
                 azureCostResult.StoragPerBlobWriteForTxs;

//Blob TXs stored: blobTxsLoggedGb * blobTxsPerGbCost
    
            var blobTxStored =  
                intermediate.BlobTxsLoggedGb * 
                azureCostResult.StoragPerGibBlobStoragForTxs;


//Blob TXs retained: blobTxsLoggedGb * blobTxsPerGbCost

            var blobTxRetained = 
                intermediate.BlobTxsLoggedGb *
                azureCostResult.StoragPerGibBlobStoragForTxs;
                
//Blob attachments logged: (eventsLoggedPerMonthCount / avgEventsPerBatchIngestedCount) * avgAttachmentsPerEventCount * blobAttachmentPerWrite
    var blobAttachmentsLogged = 
               ( _config.MiscSeting.EventsLoggedPerMonthCount /
                 _config.MiscSeting.AvgEventsPerBatchIngestedCount) * 
                 _config.MiscSeting.AvgAttachmentsPerEventCount *
                 azureCostResult.StoragPerBlobWriteForAttachment;

        var blobAttachmentsStored = 
                intermediate.BlobAttachmentsLoggedGb *
                azureCostResult.StoragPerGibBlobStoragForAttacments;
    
        var blobAttachmentsRetained = 
                intermediate.BlobAttachmentsLoggedGb * 
                azureCostResult.StoragPerGibBlobStoragForAttacments;

        var tablesLogged =
                ((_config.MiscSeting.EventsLoggedPerMonthCount *
                 _config.MiscSeting.TablesToWriteToWhenIndexingCount)
                 / (100m * _config.MiscSeting.TableBatchFillFactor))
                 * azureCostResult.StoragPerTableWrite;          

        var tablesStored = 
                intermediate.TablesLoggedGb * 
                azureCostResult.StoragPerGibTableStorag;  

        var tableRetained =
                intermediate.TablesLoggedGb *
                azureCostResult.StoragPerGibTableStorag;


        var kafkaLogged =
            intermediate.OneWayDataTransferGbKafka *
            _config.Kafka.PerGiBThroughKafka *
            2;

        var kafkaStored =
            intermediate.OneWayDataTransferGbKafka *
            _config.Kafka.PerGibInKafaStorag;

        var totalStorageTrafficGb =
            intermediate.BlobTxsLoggedGb
            + intermediate.BlobAttachmentsLoggedGb
            + intermediate.TablesLoggedGb;

        var privateEndpointsLogged =
            totalStorageTrafficGb * azureCostResult.PrivatEndpointPerGibWritten;

        var natGatewayLogged =
            totalStorageTrafficGb * azureCostResult.NatGatewayCostPerGibprocessed;

var receivedCost =
    blobTxsLogged +
    blobTxStored +
    blobAttachmentsLogged +
    blobAttachmentsStored +
    tablesLogged +
    tablesStored +
    kafkaLogged +
    kafkaStored +
    privateEndpointsLogged +
    natGatewayLogged;

var perMillionEventsReceived =
    receivedCost / (_config.MiscSeting.EventsLoggedPerMonthCount / 1_000_000m);

var retainedCost =
    blobTxRetained +
    blobAttachmentsRetained +
    tableRetained +
    kafkaStored;

var perMillionEventsRetained =
    retainedCost / (_config.MiscSeting.EventsLoggedPerMonthCount / 1_000_000m);


var totalVariableCosts = receivedCost + retainedCost;

return new VariableCostResult
{
    TotalVariableCosts = totalVariableCosts,
    PerMillionEventsReceived = perMillionEventsReceived,
    PerMillionEventsRetained = perMillionEventsRetained
};}


public async Task<AzureCostCalculation> CalculateAndSaveAzureCostAsync(string currency = "USD")
{
    var azureCostResult = await CalculateAsync(currency);

    var fixedCosts = CalculatorFixdCosts(azureCostResult);
    var variableResult = CalculateVariableCosts(azureCostResult);

    var totalAzureCost =
        (fixedCosts + variableResult.TotalVariableCosts)
        * _config.MiscSeting.SimplificationHedgeFactor
        * _config.MiscSeting.CurrencyDevaluationHedge
        * _config.MiscSeting.SellerCommissionFactor;

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

    return calculation;
}

        internal async Task CalculateAndSaveAll(CustomerInput input)
        {
            throw new NotImplementedException();
        }
    }
}


 





       
       



     

        
        

        