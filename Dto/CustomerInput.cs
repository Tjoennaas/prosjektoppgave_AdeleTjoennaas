
using System.ComponentModel.DataAnnotations;

namespace ProsjektOppgave_AdeleTjoennaas.Dto {

public record CustomerInput {
    public int ActiveUsers { get; set; }
    public int EventsPerPeriod { get; set; }
    public int RetentionPeriods { get; set; }
    public int? Collector { get; set; }

}};
