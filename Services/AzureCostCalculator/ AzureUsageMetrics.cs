

   
    using CostPricingEngine.Models.Config;
    using CostPricingEngine.Models.CostCalculation;
 

namespace CostPricingEngine.Services.AzureCostCalculator {
                   
public class AzureCostCalculator { 
    private readonly ConfigApp _configApp; public AzureCostCalculator( ConfigApp configApp) { 
        
        _configApp = configApp; } 
    
    private IntermediateCostVariables CalculateIntermediateVariables() { 

         
    //totalRetainedEventMonths: for (i = 1; i < retentionMonthsCount; i++) { totalRetainedEventMonths += eventsLoggedPerMonthCount * i }
            var totalRetainedEventMonths = 0m;

                for (int i = 1; i <_configApp.MiscSeting.AverageMonthsOfRetention; i++) {
                     totalRetainedEventMonths += 
                   _configApp.MiscSeting.EventsLoggedPerMonthCount * i; }
         
        //oneWayDataTransferGbKafka: (avgSizeOfEventInKafka * eventsLoggedPerMonthCount) / 1024 / 1024 / 1024
           var oneWayDataTransferGbKafka =
                    (_configApp.MiscSeting.AvgSizeOfEventInKafka *
                    _configApp.MiscSeting.EventsLoggedPerMonthCount)
                     / 1024m / 1024m / 1024m;         

        // blobTxsLoggedGb: (avgSizeOfEventInTxStorage * eventsLoggedPerMonthCount) / 1024 / 1024 / 1024
            var blobTxsLoggedGb = 
                    (_configApp.MiscSeting.AvgSizeOfEventdecimaTxStorage *
                    _configApp.MiscSeting.EventsLoggedPerMonthCount)
                      / 1024 / 1024 / 1024;

        // blobAttachmentsLoggedGb: (avgSizeOfStoredAttachment * eventsLoggedPerMonthCount * avgAttachmentsPerEventCount) / 1024 / 1024 / 1024
            var blobAttachmentsLoggedGb = 
                    (_configApp.MiscSeting.AvgSizeOfStoredAttachment *
                   _configApp.MiscSeting.EventsLoggedPerMonthCount *
                   _configApp.MiscSeting.AvgAttachmentsPerEventCount)
                    / 1024m / 1024m / 1024m;

        //tablesLoggedGb: (avgSizeOfIndexedEventInTxStorage * eventsLoggedPerMonthCount) / 1024 / 1024 / 1024
            var tablesLoggedGb =
                    (_configApp.MiscSeting.AvgSizeOfIndexedEventdecimaTxStorage*
                    _configApp.MiscSeting.EventsLoggedPerMonthCount)
                     / 1024 / 1024 / 1024;

            return new IntermediateCostVariables {
                    TotalRetainedEventMonths = totalRetainedEventMonths,
                    OneWayDataTransferGbKafka = oneWayDataTransferGbKafka,
                    BlobTxsLoggedGb = blobTxsLoggedGb,
                    BlobAttachmentsLoggedGb = blobAttachmentsLoggedGb,
                    TablesLoggedGb = tablesLoggedGb
                };} 


 public VariableCostResult CalculateVariableCosts( AzureCostResult  azureCostResult) {
 

       var intermediate = CalculateIntermediateVariables();


     //Blob TXs logged: (eventsLoggedPerMonthCount / avgEventsPerBatchIngestedCount) * blobTxsPerWrite  
           var blobTxsLogged = 
                    (_configApp.MiscSeting.EventsLoggedPerMonthCount /
                   _configApp.MiscSeting.AvgEventsPerBatchIngestedCount) *
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
                    (_configApp.MiscSeting.EventsLoggedPerMonthCount /
                        _configApp.MiscSeting.AvgEventsPerBatchIngestedCount) * 
                        _configApp.MiscSeting.AvgAttachmentsPerEventCount *
                         azureCostResult.StoragPerBlobWriteForAttachment;

     //Blob attachments stored: blobAttachmentsLoggedGb * blobAttachmentsPerGbCost
            var blobAttachmentsStored = 
                        intermediate.BlobAttachmentsLoggedGb *
                         azureCostResult.StoragPerGibBlobStoragForAttacments;

     //Blob attachments retained: blobAttachmentsLoggedGb * blobAttachmentsPerGbCost
            var blobAttachmentsRetained = 
                        intermediate.BlobAttachmentsLoggedGb * 
                         azureCostResult.StoragPerGibBlobStoragForAttacments;

     //Tables logged: ((eventsLoggedPerMonthCount * tablesToWriteToWhenIndexingCount) / (100 * tableBatchFillFactor)) * tablesPerWrite
            var tablesLogged =
                        ((_configApp.MiscSeting.EventsLoggedPerMonthCount *
                       _configApp.MiscSeting.TablesToWriteToWhenIndexingCount)
                        / (100m *_configApp.MiscSeting.TableBatchFillFactor))
                        *  azureCostResult.StoragPerTableWrite;   

        //Tables stored: tablesLoggedGb * tablesPerGbCost
            var tablesStored = 
                        intermediate.TablesLoggedGb * 
                         azureCostResult.StoragPerGibTableStorag;  

        //Tables retained: tablesLoggedGb * tablesPerGbCost 
            var tableRetained =
                        intermediate.TablesLoggedGb *
                         azureCostResult.StoragPerGibTableStorag;

        //Kafka logged: oneWayDataTransferGbKafka * kafkaPerGbTransferCost * 2 // Once in + once out
            var kafkaLogged =
                        intermediate.OneWayDataTransferGbKafka *
                       _configApp.Kafka.PerGiBThroughKafka *
                        2;
            
        //Kafka stored: oneWayDataTransferGbKafka * kafkaPerGbStorageCost
            var kafkaStored =
                        intermediate.OneWayDataTransferGbKafka *
                       _configApp.Kafka.PerGibInKafaStorag;
                            
        
        //Kafka retained: oneWayDataTransferGbKafka * kafkaPerGbStorageCost?
            var kafkaRetained =
                        intermediate.OneWayDataTransferGbKafka *
                       _configApp.Kafka.PerGibInKafaStorag;


        //Private endpoints logged: (blobTxsLoggedGb + blobAttachmentsLoggedGb + tablesLoggedGb) * privateEndpointCostPerGb
        //NAT gateway logged: (blobTxsLoggedGb + blobAttachmentsLoggedGb + tablesLoggedGb) * natGatewayCostPerGb
            var totalStorageTrafficGb =
                      intermediate.BlobTxsLoggedGb +
                      intermediate.BlobAttachmentsLoggedGb +
                      intermediate.TablesLoggedGb;
     
            var privateEndpointsLogged =
                    totalStorageTrafficGb *  azureCostResult.PrivatEndpointPerGibWritten;

            var natGatewayLogged =
                    totalStorageTrafficGb *  azureCostResult.NatGatewayCostPerGibprocessed;

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

               
       //Per million events retained: (blobTxsRetained + blobAttachmentsRetained + tablesRetained + kafkaStored) / 1000000
            var retainedCost =
                        blobTxRetained +
                        blobAttachmentsRetained +
                        tableRetained +
                        kafkaRetained;
    
                    var perMillionEventsRetained =
                        retainedCost / (_configApp.MiscSeting.EventsLoggedPerMonthCount / 1_000_000m);

                 

       //Per million events received: (blobTxsLogged + blobTxsStored + blobAttachmentsLogged + blobAttachmentsStored + tablesLogged + tablesStored + kafkaLogged + kafkaStored + privateEndpointsLogged + natGatewayLogged) / 1000000
           var perMillionEventsReceived =   
                        receivedCost / (_configApp.MiscSeting.EventsLoggedPerMonthCount / 1_000_000m);

                    var totalVariableCosts =
                        blobTxsLogged +
                        blobTxStored +
                        blobTxRetained +
                        blobAttachmentsLogged +
                        blobAttachmentsStored +
                        blobAttachmentsRetained +
                        tablesLogged +
                        tablesStored +
                        tableRetained +
                        kafkaLogged +
                        kafkaStored +
                        privateEndpointsLogged +
                        natGatewayLogged;

                    return new VariableCostResult
                    {
                        TotalVariableCosts = totalVariableCosts,
                        PerMillionEventsReceived = perMillionEventsReceived,
                        PerMillionEventsRetained = perMillionEventsRetained };}}}
