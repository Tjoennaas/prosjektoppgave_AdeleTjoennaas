using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prosjektoppgave_AdeleTjoennaas.Migrations
{
    /// <inheritdoc />
    public partial class AddCalculationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "CustomerCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ActiveUsers = table.Column<int>(type: "INTEGER", nullable: false),
                    EventsPerPeriod = table.Column<int>(type: "INTEGER", nullable: false),
                    RetentionPeriods = table.Column<int>(type: "INTEGER", nullable: false),
                    Collector = table.Column<int>(type: "INTEGER", nullable: true),
                    BasePrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    EventCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    UserCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    RetentionCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    CollectorCost = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCalculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalculationMargins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AzureCostCalculationId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerCalculationId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerCalculationResultId = table.Column<int>(type: "INTEGER", nullable: false),
                    Margin = table.Column<decimal>(type: "TEXT", nullable: false),
                    MarginPercent = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculationMargins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalculationMargins_AzureCostCalculations_AzureCostCalculationId",
                        column: x => x.AzureCostCalculationId,
                        principalTable: "AzureCostCalculations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalculationMargins_CustomerCalculations_CustomerCalculationResultId",
                        column: x => x.CustomerCalculationResultId,
                        principalTable: "CustomerCalculations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalculationMargins_AzureCostCalculationId",
                table: "CalculationMargins",
                column: "AzureCostCalculationId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculationMargins_CustomerCalculationResultId",
                table: "CalculationMargins",
                column: "CustomerCalculationResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CalculationMargins");

            migrationBuilder.DropTable(
                name: "AzureCostCalculations");

            migrationBuilder.DropTable(
                name: "CustomerCalculations");
        }
    }
}
