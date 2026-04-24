


using Serilog;
using ProsjektOppgave_AdeleTjoennaas.Middleware;
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

builder.Services.AddDbContext<PriceDbContext>(options =>
options.UseSqlite(connectionString));   



Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(Path.Combine(dataDirectory, "log.txt"))
     .CreateLogger();


builder.Host.UseSerilog();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddScoped<CustomerCalculator>();
builder.Services.AddSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<AzurePriceService>();
builder.Services.AddScoped<AzurePriceRefreshService>();


var app = builder.Build();

await DatabaseInitializer.ReadAsync(app.Services);

app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();


app.Run();


