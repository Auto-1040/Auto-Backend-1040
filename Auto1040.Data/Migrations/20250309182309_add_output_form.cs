using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Auto1040.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_output_form : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutputForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    S3Key = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutputForm_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PaySlipData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TotalIncome = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TipIncome = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    HouseholdWages = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TaxableDependentBenefits = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    EmployerAdoptionBenefits = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    OtherEarnedIncome = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalTaxableIncome = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalExemptIncome = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TotalDividends = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CapitalGains = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaySlipData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaySlipData_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_OutputForm_UserId",
                table: "OutputForm",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PaySlipData_UserId",
                table: "PaySlipData",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutputForm");

            migrationBuilder.DropTable(
                name: "PaySlipData");
        }
    }
}
