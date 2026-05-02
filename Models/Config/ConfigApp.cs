
using System.Collections.Generic;

namespace ProsjektOppgave_AdeleTjoennaas.Models {


   public class ConfigApp {
     public Kafka Kafka {get; set;}= new();
     public MiscCost MiscCost {get; set;}= new();
     public MiscSeting MiscSeting {get; set;}= new();
     public List<ContainerApp> ContainerApps {get; set;}= new();
}}