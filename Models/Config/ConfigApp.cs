
using System.Collections.Generic;
namespace CostPricingEngine.Models.Config {

//KLassen brukes til å samle inn all informasjon om kostnader  på et sted. 
   public class ConfigApp {
     public Kafka Kafka {get; set;}= new();
     public MiscCost MiscCost {get; set;}= new();
     public MiscSeting MiscSeting {get; set;}= new();
     public List<ContainerApp> ContainerApps {get; set;}= new();
}}