
using ProsjektOppgave_AdeleTjoennaas.BackgroundTask;
using Microsoft.EntityFrameworkCore;

namespace ProsjektOppgave_AdeleTjoennaas.Data
{
    public static class DatabaseInitializer
    {
        public static async Task ReadAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<PriceDbContext>();
            var refresh = scope.ServiceProvider.GetRequiredService<AzurePriceRefreshService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<PriceDbContext>>();

            await db.Database.MigrateAsync();

            try
            {
                await refresh.EnsureDataIsFreshAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Feil ved henting av Azure-priser under oppstart.");
                throw;
            }
        }
    }
}