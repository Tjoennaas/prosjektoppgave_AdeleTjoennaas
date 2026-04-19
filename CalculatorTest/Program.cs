

using ProsjektOppgave_AdeleTjoennaas.Models;

using ProsjektOppgave_AdeleTjoennaas.Services;




var input = new CustomerInput

{
    EventsPerPeriod = 25000000,
    ActiveUsers = 130,
    RetentionPeriods = 9
};

Calculator calculator = new Calculator();

Console.WriteLine("=== TEST AV CALCULATOR ===");

for (int i = 1; i <= 12; i++)
{
    double eventCost = calculator.CalculateEventsPerPeriod(input);
    double userCost = calculator.CalculateActiveUser(input);
    double retentionCost = calculator.CalculateRetentionCost(input, i);
    double totalCost = calculator.CalculateTotalForPeriod(input, i);

    Console.WriteLine($"Periode {i}");
    Console.WriteLine($"Events: {eventCost}");
    Console.WriteLine($"Brukere: {userCost}");
    Console.WriteLine($"Retention: {retentionCost}");
    Console.WriteLine($"Total: {totalCost}");
    Console.WriteLine("----------------------");
}

return;