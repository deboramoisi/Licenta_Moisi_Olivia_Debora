using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class ModificariClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CapitalSocial",
                table: "Client",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CasaDeMarcat",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodCAEN",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TVA",
                table: "Client",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipFirma",
                table: "Client",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CapitalSocial",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CasaDeMarcat",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "CodCAEN",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "TVA",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "TipFirma",
                table: "Client");
        }
    }
}
