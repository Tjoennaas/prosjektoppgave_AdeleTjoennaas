using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prosjektoppgave_AdeleTjoennaas.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AzureApiPricesDto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurrencyCode = table.Column<string>(type: "TEXT", nullable: true),
                    TierMinimumUnits = table.Column<decimal>(type: "TEXT", nullable: false),
                    RetailPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    ArmRegionName = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    EffectiveStartDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MeterId = table.Column<string>(type: "TEXT", nullable: true),
                    MeterName = table.Column<string>(type: "TEXT", nullable: true),
                    ProductId = table.Column<string>(type: "TEXT", nullable: true),
                    SkuId = table.Column<string>(type: "TEXT", nullable: true),
                    ProductName = table.Column<string>(type: "TEXT", nullable: true),
                    SkuName = table.Column<string>(type: "TEXT", nullable: true),
                    ServiceName = table.Column<string>(type: "TEXT", nullable: true),
                    ServiceId = table.Column<string>(type: "TEXT", nullable: true),
                    ServiceFamily = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    UnitOfMeasure = table.Column<string>(type: "TEXT", nullable: true),
                    IsPrimaryMeterRegion = table.Column<bool>(type: "INTEGER", nullable: true),
                    ArmSkuName = table.Column<string>(type: "TEXT", nullable: true),
                    LastUpdatedUtc = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AzureApiPricesDto", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AzureCostCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FixedCosts = table.Column<decimal>(type: "TEXT", nullable: false),
                    VariableCosts = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalAzureCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    PerMillionEventsReceived = table.Column<decimal>(type: "TEXT", nullable: false),
                    PerMillionEventsRetained = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AzureCostCalculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalculationMargins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PeriodNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    AzureCostCalculationId = table.Column<int>(type: "INTEGER", nullable: false),
                    CalculationGroupId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Margin = table.Column<decimal>(type: "TEXT", nullable: false),
                    MarginPercent = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationMargins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CalculationGroupId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PeriodNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    ActiveUsers = table.Column<int>(type: "INTEGER", nullable: false),
                    EventsPerPeriod = table.Column<int>(type: "INTEGER", nullable: false),
                    RetentionPeriods = table.Column<int>(type: "INTEGER", nullable: false),
                    Collector = table.Column<int>(type: "INTEGER", nullable: true),
                    BasePrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    EventCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    UserCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    RetentionCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    CollectorCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalVariablePrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCalculations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AzureApiPricesDto");

            migrationBuilder.DropTable(
                name: "AzureCostCalculations");

            migrationBuilder.DropTable(
                name: "CalculationMargins");

            migrationBuilder.DropTable(
                name: "CustomerCalculations");
        }
    }
}
