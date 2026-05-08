

            using Microsoft.EntityFrameworkCore;
            using Serilog;
            using ExceptionHandler.Middleware;
            using CostPricingEngine.Services;
            using CostPricingEngine.Services.AzureCostCalculator;
            using CostPricingEngine.BackgroundTask;
            using CostPricingEngine.Data;
            using CostPricingEngine.Models.Config;


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


        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(dataDirectory, "log.txt"))
            .CreateLogger();


        builder.Services.AddDbContext<CostDbContext>(options =>
        options.UseSqlite(connectionString));



        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddControllers();
        builder.Services.AddSerilog();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddHttpClient<AzureRetailPriceApiService>();

        builder.Services.AddScoped<AzurePricingService>();
        builder.Services.AddScoped<AzurePriceRefreshService>();
        builder.Services.AddScoped<CustomerCalculator>();

        builder.Services.AddScoped<MarginCalculation>();
        builder.Services.AddScoped<AzureCostCalculationService>();
        builder.Services.AddScoped<CalculatorFixdCosts>();
        builder.Services.AddScoped<AzureCostCalculator>();

        builder.Host.UseSerilog();
        builder.Services.AddSingleton(
        builder.Configuration.GetSection("ConfigApp").Get<ConfigApp>()! );

        var app = builder.Build();

        await DatabaseInitializer.ReadAsync(app.Services);

        app.UseExceptionHandler();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.MapControllers();
        app.Run();


