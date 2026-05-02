

namespace ProsjektOppgave_AdeleTjoennaas.Models {
    
       public class AzureCostResult
       {
              public decimal ContainerAppsPriceOfVcpuSeconds{get; set;}
              
              public decimal ContainerAppsPriceOfGibSeconds{get; set;}

              public decimal CosmosDbPricedByHundredRusApiRespons{get; set;}
 
              public decimal NatGatewayCostPerHour{get; set;}

              public decimal NatGatewayCostPerGibprocessed{get; set;}
              
              public decimal StaticIpAddressPricePerHour{get; set;}

              public decimal PrivatEndpointPricePerHour{get; set;}

              public decimal PrivatEndpointPerGibWritten{get; set;}
              
              public decimal StoragPerBlobWriteForTxs{get; set;}

              public decimal StoragPerBlobWriteForAttachment{get; set;}

              public decimal StoragPerGibBlobStoragForTxs{get; set;}

              public decimal StoragPerGibBlobStoragForAttacments{get; set;}

              public decimal StoragPerTableWrite{get; set;}

              public decimal StoragPerGibTableStorag{get; set;}
       }}