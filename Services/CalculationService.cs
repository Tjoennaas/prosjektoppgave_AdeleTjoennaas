

                    using Microsoft.EntityFrameworkCore;

                    using ProsjektOppgave_AdeleTjoennaas.Data;
                    using ProsjektOppgave_AdeleTjoennaas.Dto;
                    using ProsjektOppgave_AdeleTjoennaas.Models;



                    namespace ProsjektOppgave_AdeleTjoennaas.Services
                    {
                        public class CalculationServices
                        {
                            private readonly PriceCalculator _priceCalculator;
                            private readonly CustomerCalculator _customerCalculator;
                            private readonly PriceDbContext _db;

                            public CalculationServices(
                                PriceCalculator priceCalculator,
                                CustomerCalculator customerCalculator,
                                PriceDbContext db)
                            {
                                _priceCalculator = priceCalculator;
                                _customerCalculator = customerCalculator;
                                _db = db;
                            }

    public async Task<List<CalculationMargin>> CalculateAndSaveAll(CustomerInput input)
{
    var results = new List<CalculationMargin>();

    var azure = await _priceCalculator.CalculateAndSaveAzureCostAsync();
    var customers = await _customerCalculator.CalculateAndSaveAllAsync(input);

    var groupId = customers.First().CalculationGroupId;

    foreach (var customer in customers)
    {
        var margin = customer.TotalPrice - azure.TotalAzureCost;

        var marginPercent =
            customer.TotalPrice == 0
                ? 0
                : (margin / customer.TotalPrice) * 100;

        var marginEntity = new CalculationMargin
        {
            AzureCostCalculationId = azure.Id,
            CalculationGroupId = customer.CalculationGroupId,
            // CustomerCalculationResultId = customer.Id,
            Margin = Math.Round(margin, 2),
            MarginPercent = Math.Round(marginPercent, 2)
        };

        _db.CalculationMargins.Add(marginEntity);
        results.Add(marginEntity);
    }

    await _db.SaveChangesAsync();

    return results;
}}};

     













