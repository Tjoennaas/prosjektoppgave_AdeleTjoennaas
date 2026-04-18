using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace prosjektoppgave_AdeleTjoennaas.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAzureModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "ArmRegionName",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "ArmSkuName",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveStartDate",
                table: "AzurePrices",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimaryMeterRegion",
                table: "AzurePrices",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeterId",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MeterName",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceFamily",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceId",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceName",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkuId",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SkuName",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TierMinimumUnits",
                table: "AzurePrices",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitOfMeasure",
                table: "AzurePrices",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "AzurePrices",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArmSkuName",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "EffectiveStartDate",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "IsPrimaryMeterRegion",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "MeterId",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "MeterName",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "ServiceFamily",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "ServiceName",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "SkuId",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "SkuName",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "TierMinimumUnits",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "UnitOfMeasure",
                table: "AzurePrices");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "AzurePrices");

            migrationBuilder.AlterColumn<string>(
                name: "ProductName",
                table: "AzurePrices",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyCode",
                table: "AzurePrices",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ArmRegionName",
                table: "AzurePrices",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
