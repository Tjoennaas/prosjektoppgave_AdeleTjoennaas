



        using Microsoft.EntityFrameworkCore;
        using CostPricingEngine.Data;
        using CostPricingEngine.Models.CostCalculation;
            
           

     namespace CostPricingEngine.Services.AzureCostCalculator {
             
            
             public class AzurePricingService {

            private readonly CostDbContext _db;
     
             public AzurePricingService (CostDbContext db) {
                _db = db; }

 
    public async Task<AzureCostResult> GetAzureCostPricesAsync(string currency = "USD") {       
    var staticIpAddress = await GetPriceAsync(

        productName: "IP Addresses",
        meterName: "Basic IPv4 Static Public IP",
        unitOfMeasure: "1 Hour",
        currency: currency );


    var privatEndpointPerGibWritten = await GetPriceAsync(

        productName: "Virtual Network Private Link",
        meterName: "Standard Data Processed - Ingress",
        unitOfMeasure: "1 GB",
        currency: currency,
        armRegionName: "Global" );

    
    var privatEndpointPricePerHour = await GetPriceAsync(

        productName: "Virtual Network Private Link",
        serviceName: "Virtual Network",
        meterName: "Standard Private Endpoint",
        unitOfMeasure: "1 Hour",
        currency: currency,
        armRegionName: "Global" );


    var cpuPrice = await GetPriceAsync(

        serviceName: "Azure Container Apps",
        meterName: "Standard vCPU Active Usage",
        unitOfMeasure: "1 Second",
        currency: currency);


    var gibPrice = await GetPriceAsync(

        serviceName: "Azure Container Apps",
        meterName: "Standard Memory Active Usage",
        unitOfMeasure: "1 GiB Second",
        currency: currency );


    var cosmosDbHourPrice = await GetPriceAsync(

        serviceName: "Azure Cosmos DB",
        meterName: "100 RU/s",
        unitOfMeasure: "1/Hour",
        currency: currency );


    var natGatewayCostPerHour = await GetPriceAsync(

        productName: "NAT Gateway",
        serviceName: "NAT Gateway",
        meterName: "Standard Gateway",
        unitOfMeasure: "1 Hour",
        currency: currency,
        armRegionName: "Global" );


    var natGatewayCostPerGiB = await GetPriceAsync(

        productName: "NAT Gateway",
        serviceName: "NAT Gateway",
        meterName: "Standard Data Processed",
        unitOfMeasure: "1 GB",
        currency: currency,
        armRegionName: "Global" );

    
    var storagePerBlobWriteForTXs = await GetPriceAsync(

        productName: "General Block Blob v2",
        meterName: "Hot ZRS Write Operations",
        skuName: "Hot ZRS",
        unitOfMeasure: "10K",
        currency: currency );


    var storagePerBlobWriteForAttachments = await GetPriceAsync(

        productName: "General Block Blob v2",
        meterName: "Cold ZRS Write Operations",
        skuName: "Cold ZRS",
        unitOfMeasure: "10K",
        currency: currency );

    var storagPerGibBlobStorageForTxs = await GetPriceAsync(

        productName: "General Block Blob v2",
        meterName: "Hot ZRS Data Stored",
        skuName: "Hot ZRS",
        unitOfMeasure: "1 GB/Month",
        currency: currency );

    var storagePerGibBlobStorageForAttachments = await GetPriceAsync(

        productName: "General Block Blob v2",
        meterName: "Cold ZRS Data Stored",
        skuName: "Cold ZRS",
        unitOfMeasure: "1 GB/Month",
        currency: currency );

    var storagePerTableWrite = await GetPriceAsync(

        productName: "Tables",
        meterName: "Batch Write Operations",
        skuName: "Standard ZRS",
        unitOfMeasure: "10K",
        currency: currency );

    var storagePerGiBTableStorage = await GetPriceAsync(

        productName: "Tables",
        meterName: "ZRS Data Stored",
        skuName: "Standard ZRS",
        unitOfMeasure: "1 GB/Month",
        currency: currency );

        return new  AzureCostResult {
       

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
        StoragPerGibTableStorag = storagePerGiBTableStorage, };}
        
       
        private async Task<decimal> GetPriceAsync (
                string? productName = null, 
                string? serviceName = null, 
                string? meterName = null,
                string? skuName = null,
                string? unitOfMeasure = null, 
                string? currency = "USD",
                string? armRegionName = null) {  

                var price = await _db.AzureApiPricesDto
                .Where(x => string.IsNullOrEmpty(productName) || x.ProductName == productName)
                .Where(x => string.IsNullOrEmpty(serviceName) || x.ServiceName == serviceName)
                .Where(x => string.IsNullOrEmpty(meterName) || x.MeterName == meterName)
                .Where(x => string.IsNullOrEmpty(skuName) || x.SkuName == skuName)
                .Where(x => string.IsNullOrEmpty(unitOfMeasure) || x.UnitOfMeasure == unitOfMeasure)
                .Where(x => string.IsNullOrEmpty(currency) || x.CurrencyCode == currency)
                .Where(x => string.IsNullOrEmpty(armRegionName) || x.ArmRegionName == armRegionName)
                .Select(x => x.RetailPrice)
                .FirstAsync();

                return price;  }};}
    
         
