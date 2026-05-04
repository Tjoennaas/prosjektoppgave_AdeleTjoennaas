
using CostPricingEngine.Data;
using CostPricingEngine.Models.AzureApi;
using CostPricingEngine.Models.Config;
using CostPricingEngine.Models.CostCalculation;
using CostPricingEngine.Models.CostMargin;

using Microsoft.EntityFrameworkCore;
using CostPricingEngine.Models;


namespace CostPricingEngine.Data {

public class CostDbContext : DbContext {  

public CostDbContext(DbContextOptions<CostDbContext> options)
              : base(options) { }
     
     public DbSet<AzureApiPricesDto> AzureApiPricesDto { get; set; }
     public DbSet<CustomerCalculationResult> CustomerCalculations { get; set; }
     public DbSet<AzureCostCalculation> AzureCostCalculations { get; set; }
     public DbSet<CalculationMargin> CalculationMargins { get; set; }

}}
    

