

using Serilog;
using ProsjektOppgave_AdeleTjoennaas.Services;
using ProsjektOppgave_AdeleTjoennaas.BackgroundTask;
using ProsjektOppgave_AdeleTjoennaas.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


 builder.Services.AddDbContext<PriceAzureContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));   

//Ref serielog: https://www.nuget.org/packages/Serilog.AspNetCore
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt")
     .CreateLogger();

builder.Host.UseSerilog();

//builder.Services.AddSerilog();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<AzurePriceService>();
builder.Services.AddHostedService<AzureBackgroundService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PriceAzureContext>();
    db.Database.EnsureCreated();
}


app.Run();
