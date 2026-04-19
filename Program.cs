

using Serilog;
using ProsjektOppgave_AdeleTjoennaas.Services;
using ProsjektOppgave_AdeleTjoennaas.BackgroundTask;
using ProsjektOppgave_AdeleTjoennaas.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<PriceAzureContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));   

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("/app/data/log.txt")
     .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<AzurePriceService>();
builder.Services.AddScoped<AzurePriceRefreshService>();


var app = builder.Build();


using (var scope = app.Services.CreateScope()) {

 var db = scope.ServiceProvider.GetRequiredService<PriceAzureContext>();
 var refresh = scope.ServiceProvider.GetRequiredService<AzurePriceRefreshService>();

  db.Database.Migrate();
  await refresh.EnsureDataIsFreshAsync(); 
}


app.MapGet("/objekts", async (PriceAzureContext db) =>
{
    return await db.AzurePrices.ToListAsync();
});

app.MapControllers();
app.Run();

