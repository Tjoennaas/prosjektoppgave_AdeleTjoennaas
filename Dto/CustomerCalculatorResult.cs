


namespace ProsjektOppgave_AdeleTjoennaas.Dto {


public record CustomerCalculation
(

   int ActiveUsers,

   int EventsPerPeriod,

   int RetentionPeriods, 

   int? Collctor,

   double BasePrice, 

   double EventCost, 

   double UserCost, 

   double RetentionCost,

   int CollectorCost,

   double TotalPrice

);}




