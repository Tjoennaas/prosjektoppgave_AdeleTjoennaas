

                using ProsjektOppgave_AdeleTjoennaas.Dto;
                using ProsjektOppgave_AdeleTjoennaas.Models;
                using CostPrices.Data;
namespace ProsjektOppgave_AdeleTjoennaas.Services
{
    public class  CustomerCalculator
    { private readonly CostDbContext _db;
   

     public  CustomerCalculator (CostDbContext db)
{
    _db = db;

    
}
   

        public double CalculateEventsPerPeriod(CustomerInput input)
        {

            if (input.EventsPerPeriod <= 10_000_000)
            {
                return 0;
            }

            return (input.EventsPerPeriod - 10_000_000) / 1_000_000.0 * 100;
        }

        public double CalculateActiveUser(CustomerInput input)
        {
            if (input.ActiveUsers <= 10)
            {
                return 0;
            }

            return (input.ActiveUsers - 10) * 80;
        }

        public int CalculateCollectorCost(CustomerInput input)
        {
            int collectorCount = input.Collector ?? 0;

            if (collectorCount < 0)
            {
                return 0;
            }

            return collectorCount * 2000;
         }


        public double CalculateRetentionCost(CustomerInput input, int periodNumber)
        {
            if (input.RetentionPeriods <= 1)
            {
                return 0;
            }

            double billableMillions =
                (input.EventsPerPeriod - Math.Min(input.EventsPerPeriod, 10_000_000)) / 1_000_000.0;

            int storedPeriods =
                Math.Min(periodNumber - 1, input.RetentionPeriods - 1);

            return billableMillions * storedPeriods * 100;
        }

        public CustomerCalculationResult CalculateForPeriod(CustomerInput input, int periodNumber = 1)
        {
            double basePrice = 50_000;
            double eventCost = CalculateEventsPerPeriod(input);
            double userCost = CalculateActiveUser(input);
            int collectorCost = CalculateCollectorCost(input);
            double retentionCost = CalculateRetentionCost(input, periodNumber);

            decimal totalVariablePrice =
    (decimal)eventCost + (decimal)userCost + collectorCost + (decimal)retentionCost;

decimal totalPrice =
    (decimal)basePrice + totalVariablePrice; 

          
            
            return new CustomerCalculationResult{

                        ActiveUsers = input.ActiveUsers,
                        EventsPerPeriod = input.EventsPerPeriod,
                        RetentionPeriods = input.RetentionPeriods,
                        Collector = input.Collector,

                        BasePrice = (decimal)basePrice,
                        EventCost = (decimal)eventCost,
                        UserCost = (decimal)userCost,
                        RetentionCost = (decimal)retentionCost,
                        CollectorCost = collectorCost,
                        TotalPrice = (decimal)totalPrice  };}


public async Task<List<CustomerCalculationResult>> CalculateAndSaveAllAsync(CustomerInput input)
{
    var results = new List<CustomerCalculationResult>();
    var groupId = Guid.NewGuid();
  


    for (int periodNumber = 1; periodNumber <= input.RetentionPeriods; periodNumber++)

    {
        var result = CalculateForPeriod(input, periodNumber);

        result.CalculationGroupId = groupId;
        result.PeriodNumber = periodNumber;

  

        _db.CustomerCalculations.Add(result);
        results.Add(result);
    }

    await _db.SaveChangesAsync();

    return results;
}}}
                 










