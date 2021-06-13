using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class RemoveColumnsFromClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CapitalSocial",
                table: "Client",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "CasaDeMarcat",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodCAEN",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TVA",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
