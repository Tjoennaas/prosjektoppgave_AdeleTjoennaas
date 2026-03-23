

using Serilog;
using ProsjektOppgave_AdeleTjoennaas.Services;
using ProsjektOppgave_AdeleTjoennaas.BackgroundTask;

var builder = WebApplication.CreateBuilder(args);


//Ref serielog: https://www.nuget.org/packages/Serilog.AspNetCore
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("log.txt")
     .CreateLogger();


     builder.Services.AddSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<AzurePriceService>();
builder.Services.AddHostedService<AzureBackgroundService>();


var app = builder.Build();

app.Run();
