
using CostPricingEngine.BackgroundTask;
using CostPricingEngine.Data;
using Microsoft.EntityFrameworkCore;


namespace CostPricingEngine.Data
{
    
    public static class DatabaseInitializer
{
    public static async Task ReadAsync(IServiceProvider services){

using var scope = services.CreateScope();

 var db = scope.ServiceProvider.GetRequiredService<CostDbContext>();
 var refresh = scope.ServiceProvider.GetRequiredService<AzurePriceRefreshService>();

  db.Database.Migrate();
  await refresh.EnsureDataIsFreshAsync(); 
}

    }
}
