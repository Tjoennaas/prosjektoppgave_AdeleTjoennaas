
using System.ComponentModel.DataAnnotations;

namespace ProsjektOppgave_AdeleTjoennaas.Dto {

public record CustomerInput
(
 
   [Range(1, int.MaxValue)]
   int ActiveUsers,
 
   [Range(1, int.MaxValue)]
   int EventsPerPeriod,
  
   [Range(1, int.MaxValue)]
   int RetentionPeriods, 

   int? Collector

  

  
);}
