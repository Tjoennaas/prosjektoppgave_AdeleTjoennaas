using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prosjektoppgave_AdeleTjoennaas.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupIdToMargin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerCalculationResultId",
                table: "CalculationMargins");

            migrationBuilder.AddColumn<Guid>(
                name: "CalculationGroupId",
                table: "CalculationMargins",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CalculationGroupId",
                table: "CalculationMargins");

            migrationBuilder.AddColumn<int>(
                name: "CustomerCalculationResultId",
                table: "CalculationMargins",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
