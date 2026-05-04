using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prosjektoppgave_AdeleTjoennaas.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "AzureCostCalculations",
                newName: "AzureCostCalculationId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculationMargins_AzureCostCalculationId",
                table: "CalculationMargins",
                column: "AzureCostCalculationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalculationMargins_AzureCostCalculations_AzureCostCalculationId",
                table: "CalculationMargins",
                column: "AzureCostCalculationId",
                principalTable: "AzureCostCalculations",
                principalColumn: "AzureCostCalculationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalculationMargins_AzureCostCalculations_AzureCostCalculationId",
                table: "CalculationMargins");

            migrationBuilder.DropIndex(
                name: "IX_CalculationMargins_AzureCostCalculationId",
                table: "CalculationMargins");

            migrationBuilder.RenameColumn(
                name: "AzureCostCalculationId",
                table: "AzureCostCalculations",
                newName: "Id");
        }
    }
}
