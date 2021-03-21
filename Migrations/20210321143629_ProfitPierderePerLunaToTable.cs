using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Migrations
{
    public partial class ProfitPierderePerLunaToTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Pierdere_luna",
                table: "ProfitPierdere",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Profit_luna",
                table: "ProfitPierdere",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pierdere_luna",
                table: "ProfitPierdere");

            migrationBuilder.DropColumn(
                name: "Profit_luna",
                table: "ProfitPierdere");
        }
    }
}
