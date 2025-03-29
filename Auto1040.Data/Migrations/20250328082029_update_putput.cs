using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auto1040.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_putput : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaySlipData_User_UserId",
                table: "PaySlipData");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "OutputForm",
                newName: "S3Url");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PaySlipData",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PaySlipData_User_UserId",
                table: "PaySlipData",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaySlipData_User_UserId",
                table: "PaySlipData");

            migrationBuilder.RenameColumn(
                name: "S3Url",
                table: "OutputForm",
                newName: "FilePath");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "PaySlipData",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaySlipData_User_UserId",
                table: "PaySlipData",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
