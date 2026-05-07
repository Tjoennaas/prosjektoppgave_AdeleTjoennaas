

// Representerer pris per GIB datatrafikk gjennom kafka og pris per GIB lagring i kafka
// Lagt in default verdier i appsetings.json
namespace CostPricingEngine.Models.Config {
    
     public class Kafka {   
         public decimal PerGiBThroughKafka {get; set;} = 0.05m;
      
         public decimal PerGibInKafaStorag {get; set;}= 0.08m;

     }}

     