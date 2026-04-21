
using ProsjektOppgave_AdeleTjoennaas.Middleware;
using ProsjektOppgave_AdeleTjoennaas.Services;
using ProsjektOppgave_AdeleTjoennaas.BackgroundTask;

using Microsoft.EntityFrameworkCore;


namespace ProsjektOppgave_AdeleTjoennaas.Data
{
    
    public static class DatabaseInitializer
{
    public static async Task ReadAsync(IServiceProvider services){

using var scope = services.CreateScope();

 var db = scope.ServiceProvider.GetRequiredService<PriceDbContext>();
 var refresh = scope.ServiceProvider.GetRequiredService<AzurePriceRefreshService>();

  db.Database.Migrate();
  await refresh.EnsureDataIsFreshAsync(); 
}

    }
}
