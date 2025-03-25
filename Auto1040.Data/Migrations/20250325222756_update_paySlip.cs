using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auto1040.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_paySlip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalIncome",
                table: "PaySlipData",
                newName: "TotalIncomeUSD");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PaySlipData",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalIncomeILS",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "PaySlipData",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "TotalIncomeILS",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "PaySlipData");

            migrationBuilder.RenameColumn(
                name: "TotalIncomeUSD",
                table: "PaySlipData",
                newName: "TotalIncome");
        }
    }
}
