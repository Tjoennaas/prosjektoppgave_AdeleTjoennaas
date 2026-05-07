


namespace CostPricingEngine.Dto {

//Kunde input som har parametere som brukes i bergning av kundepris
//Dette gjøres for å unngå at interne databaseobjekter eksponeres direkte gjennom API-et.
public record CustomerInput {
    public int ActiveUsers { get; set; }
    public int EventsPerPeriod { get; set; }
    public int RetentionPeriods { get; set; }
    public int? Collector { get; set; }

}};
