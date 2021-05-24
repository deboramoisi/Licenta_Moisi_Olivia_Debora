using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class MoreColumnsToCerereDocumente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdeverintaPath",
                table: "CereriDocumente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Resolved",
                table: "CereriDocumente",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdeverintaPath",
                table: "CereriDocumente");

            migrationBuilder.DropColumn(
                name: "Resolved",
                table: "CereriDocumente");
        }
    }
}
