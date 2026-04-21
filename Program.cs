


using Serilog;
using ProsjektOppgave_AdeleTjoennaas.Services;
using ProsjektOppgave_AdeleTjoennaas.BackgroundTask;
using ProsjektOppgave_AdeleTjoennaas.Data;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


var isRunningInContainer = string.Equals(
    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"),
    "true",
    StringComparison.OrdinalIgnoreCase);

var dataDirectory = isRunningInContainer
    ? "/app/data"
    : Path.Combine(builder.Environment.ContentRootPath, "sqldata");

Directory.CreateDirectory(dataDirectory);

var connectionString = isRunningInContainer
    ? builder.Configuration.GetConnectionString("DefaultConnection")
        ?? $"Data Source={Path.Combine(dataDirectory, "data.db")}"
    : $"Data Source={Path.Combine(dataDirectory, "data.db")}";

builder.Services.AddDbContext<PriceAzureContext>(options =>
options.UseSqlite(connectionString));   

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(dataDirectory, "log.txt"))
     .CreateLogger();

builder.Host.UseSerilog();


builder.Services.AddControllers();
builder.Services.AddScoped<Calculator>();
builder.Services.AddSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<AzurePriceService>();
builder.Services.AddScoped<AzurePriceRefreshService>();


var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


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


