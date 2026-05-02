using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prosjektoppgave_AdeleTjoennaas.Migrations
{
    /// <inheritdoc />
    public partial class FixCalculationMarginForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CalculationMargins_AzureCostCalculations_AzureCostCalculationId",
                table: "CalculationMargins");

            migrationBuilder.DropForeignKey(
                name: "FK_CalculationMargins_CustomerCalculations_CustomerCalculationResultId",
                table: "CalculationMargins");

            migrationBuilder.DropIndex(
                name: "IX_CalculationMargins_AzureCostCalculationId",
                table: "CalculationMargins");

            migrationBuilder.DropIndex(
                name: "IX_CalculationMargins_CustomerCalculationResultId",
                table: "CalculationMargins");

            migrationBuilder.DropColumn(
                name: "CustomerCalculationId",
                table: "CalculationMargins");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerCalculationId",
                table: "CalculationMargins",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CalculationMargins_AzureCostCalculationId",
                table: "CalculationMargins",
                column: "AzureCostCalculationId");

            migrationBuilder.CreateIndex(
                name: "IX_CalculationMargins_CustomerCalculationResultId",
                table: "CalculationMargins",
                column: "CustomerCalculationResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_CalculationMargins_AzureCostCalculations_AzureCostCalculationId",
                table: "CalculationMargins",
                column: "AzureCostCalculationId",
                principalTable: "AzureCostCalculations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CalculationMargins_CustomerCalculations_CustomerCalculationResultId",
                table: "CalculationMargins",
                column: "CustomerCalculationResultId",
                principalTable: "CustomerCalculations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
