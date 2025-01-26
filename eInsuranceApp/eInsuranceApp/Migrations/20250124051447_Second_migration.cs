using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eInsuranceApp.Migrations
{
    /// <inheritdoc />
    public partial class Second_migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasePremiumRate",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "Premium",
                table: "Policies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BasePremiumRate",
                table: "Policies",
                type: "DECIMAL(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Premium",
                table: "Policies",
                type: "DECIMAL(10,2)",
                nullable: true);
        }
    }
}
