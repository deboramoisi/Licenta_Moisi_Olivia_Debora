using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Migrations
{
    public partial class AddColsToProfitPierdereTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "cred_prec",
                table: "ProfitPierdere",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "deb_prec",
                table: "ProfitPierdere",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "fin_c",
                table: "ProfitPierdere",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "fin_d",
                table: "ProfitPierdere",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "rulaj_c",
                table: "ProfitPierdere",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "rulaj_d",
                table: "ProfitPierdere",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cred_prec",
                table: "ProfitPierdere");

            migrationBuilder.DropColumn(
                name: "deb_prec",
                table: "ProfitPierdere");

            migrationBuilder.DropColumn(
                name: "fin_c",
                table: "ProfitPierdere");

            migrationBuilder.DropColumn(
                name: "fin_d",
                table: "ProfitPierdere");

            migrationBuilder.DropColumn(
                name: "rulaj_c",
                table: "ProfitPierdere");

            migrationBuilder.DropColumn(
                name: "rulaj_d",
                table: "ProfitPierdere");
        }
    }
}
