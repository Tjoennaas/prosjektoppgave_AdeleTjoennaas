
        using CostPricingEngine.BackgroundTask;
        using Microsoft.EntityFrameworkCore;

// Denne koden brukes til å initalerese databasen når applikasjonen kjører.
//Oppdatere databasen når migrasjon kjører og at azure pris data finnes og kjøres.
        namespace CostPricingEngine.Data {
            
            public static class DatabaseInitializer {
            public static async Task ReadAsync(IServiceProvider services){

        using var scope = services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<CostDbContext>();
            var refresh = scope.ServiceProvider.GetRequiredService<AzurePriceRefreshService>();

            db.Database.Migrate();
            await refresh.EnsureDataIsFreshAsync();  }}}
