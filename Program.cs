
using ProsjektOppgave_AdeleTjoennaas.Services;
using ProsjektOppgave_AdeleTjoennaas.BackgroundTask;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<AzurePriceService>();
builder.Services.AddHostedService<AzureBackgroundService >();


var app = builder.Build();



app.Run();

