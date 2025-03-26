using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auto1040.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_paySlip_s3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "F36",
                table: "PaySlipData",
                newName: "Field36");

            migrationBuilder.RenameColumn(
                name: "F248_249",
                table: "PaySlipData",
                newName: "Field248_249");

            migrationBuilder.RenameColumn(
                name: "F218_219",
                table: "PaySlipData",
                newName: "Field218_219");

            migrationBuilder.RenameColumn(
                name: "F158_172",
                table: "PaySlipData",
                newName: "Field158_172");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PaySlipData",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "S3Key",
                table: "PaySlipData",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "S3Url",
                table: "PaySlipData",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "TaxYear",
                table: "PaySlipData",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "S3Key",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "S3Url",
                table: "PaySlipData");

            migrationBuilder.DropColumn(
                name: "TaxYear",
                table: "PaySlipData");

            migrationBuilder.RenameColumn(
                name: "Field36",
                table: "PaySlipData",
                newName: "F36");

            migrationBuilder.RenameColumn(
                name: "Field248_249",
                table: "PaySlipData",
                newName: "F248_249");

            migrationBuilder.RenameColumn(
                name: "Field218_219",
                table: "PaySlipData",
                newName: "F218_219");

            migrationBuilder.RenameColumn(
                name: "Field158_172",
                table: "PaySlipData",
                newName: "F158_172");
        }
    }
}
