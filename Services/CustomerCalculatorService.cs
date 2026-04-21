


using ProsjektOppgave_AdeleTjoennaas.Dto;

namespace ProsjektOppgave_AdeleTjoennaas.Services
{
    public class CustomerCalculator
    {
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

            return (input.ActiveUsers - 10) * 10;
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

        public CustomerCalculation CalculateForPeriod(CustomerInput input, int periodNumber)
        {
            double basePrice = 50_000;
            double eventCost = CalculateEventsPerPeriod(input);
            double userCost = CalculateActiveUser(input);
            int collectorCost = CalculateCollectorCost(input);
            double retentionCost = CalculateRetentionCost(input, periodNumber);

            double totalPrice =
                basePrice + eventCost + userCost + collectorCost + retentionCost;

            return new CustomerCalculation(
                input.ActiveUsers,
                input.EventsPerPeriod,
                input.RetentionPeriods,
                input.Collector,
                basePrice,
                eventCost,
                userCost,
                retentionCost,
                collectorCost,
                totalPrice
            );
        }
    }
}                 



     










