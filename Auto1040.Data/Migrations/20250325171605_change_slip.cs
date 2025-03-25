using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auto1040.Data.Migrations
{
    /// <inheritdoc />
    public partial class change_slip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapitalGains",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "EmployerAdoptionBenefits",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "HouseholdWages",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "OtherEarnedIncome",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "TaxableDependentBenefits",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "TipIncome",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "TotalDividends",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "TotalExemptIncome",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "TotalTaxableIncome",
                table: "PaySlipData");

            migrationBuilder.AddColumn<decimal>(
                name: "F158_172",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "F218_219",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "F248_249",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "F36",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "F158_172",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "F218_219",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "F248_249",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "F36",
                table: "PaySlipData");

            migrationBuilder.AddColumn<decimal>(
                name: "CapitalGains",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "EmployerAdoptionBenefits",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HouseholdWages",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "OtherEarnedIncome",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxableDependentBenefits",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TipIncome",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalDividends",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalExemptIncome",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalTaxableIncome",
                table: "PaySlipData",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
