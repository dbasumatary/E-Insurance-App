using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eInsuranceApp.Migrations
{
    /// <inheritdoc />
    public partial class Third_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PremiumID",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PremiumID",
                table: "Payments",
                column: "PremiumID");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Premiums_PremiumID",
                table: "Payments",
                column: "PremiumID",
                principalTable: "Premiums",
                principalColumn: "PremiumID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Premiums_PremiumID",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PremiumID",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PremiumID",
                table: "Payments");
        }
    }
}
