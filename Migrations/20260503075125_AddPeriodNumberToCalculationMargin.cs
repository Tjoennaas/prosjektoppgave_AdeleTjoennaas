using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prosjektoppgave_AdeleTjoennaas.Migrations
{
    /// <inheritdoc />
    public partial class AddPeriodNumberToCalculationMargin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PeriodNumber",
                table: "CalculationMargins",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PeriodNumber",
                table: "CalculationMargins");
        }
    }
}
