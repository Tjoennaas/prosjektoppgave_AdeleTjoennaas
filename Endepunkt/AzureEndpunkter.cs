
using ProsjektOppgave_AdeleTjoennaas.Data;
using ProsjektOppgave_AdeleTjoennaas.BackgroundTask;
using ProsjektOppgave_AdeleTjoennaas.Services;
using ProsjektOppgave_AdeleTjoennaas.Models;
using Microsoft.EntityFrameworkCore;

namespace ProsjektOppgave_AdeleTjoennaas.Endepunkt
{
    public static class AzureEndpoints
    {
 public static void MapAzureEndpoints(this WebApplication app)
{
 app.MapGet("/", () => "API kjører");

app.MapGet("/prices", async (
    AzurePriceRefreshService refreshService,
    PriceAzureContext db) =>
    {
    await refreshService.EnsureDataIsFreshAsync();

            var data = await db.AzurePrices
                    .Select(x => new
                    {
                        x.ArmRegionName,
                        x.ProductName,
                        x.RetailPrice,
                        x.CurrencyCode,
                        x.LastUpdatedUtc
                    })
                    .Take(10)
                    .ToListAsync();

                return Results.Ok(data);
            });
        }
    }
} 