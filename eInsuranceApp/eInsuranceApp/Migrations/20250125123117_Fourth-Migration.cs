using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eInsuranceApp.Migrations
{
    /// <inheritdoc />
    public partial class FourthMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CommissionRate",
                table: "Agents",
                type: "DECIMAL(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Commission",
                columns: table => new
                {
                    CommissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentID = table.Column<int>(type: "int", nullable: false),
                    AgentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PolicyID = table.Column<int>(type: "int", nullable: false),
                    PremiumID = table.Column<int>(type: "int", nullable: false),
                    CommissionAmount = table.Column<decimal>(type: "DECIMAL(10,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commission", x => x.CommissionID);
                    table.ForeignKey(
                        name: "FK_Commission_Agents_AgentID",
                        column: x => x.AgentID,
                        principalTable: "Agents",
                        principalColumn: "AgentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Commission_Premiums_PremiumID",
                        column: x => x.PremiumID,
                        principalTable: "Premiums",
                        principalColumn: "PremiumID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commission_AgentID",
                table: "Commission",
                column: "AgentID");

            migrationBuilder.CreateIndex(
                name: "IX_Commission_PremiumID",
                table: "Commission",
                column: "PremiumID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commission");

            migrationBuilder.DropColumn(
                name: "CommissionRate",
                table: "Agents");
        }
    }
}
