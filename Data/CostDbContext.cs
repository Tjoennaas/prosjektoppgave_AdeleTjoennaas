


using Microsoft.EntityFrameworkCore;
using ProsjektOppgave_AdeleTjoennaas.Models;


namespace CostPrices.Data {

public class CostDbContext : DbContext {  

public CostDbContext(DbContextOptions<CostDbContext> options)
              : base(options) { }

     public DbSet<AzurePrice> AzurePrices { get; set; }
     public DbSet<CustomerCalculationResult> CustomerCalculations { get; set; }
     public DbSet<AzureCostCalculation> AzureCostCalculations { get; set; }
     public DbSet<CalculationMargin> CalculationMargins { get; set; }

}}
    

