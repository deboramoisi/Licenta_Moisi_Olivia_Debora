using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class MoreColumnsCerereDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataStart",
                table: "CereriDocumente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DenumireCerere",
                table: "CereriDocumente",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DenumireClient",
                table: "CereriDocumente",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataStart",
                table: "CereriDocumente");

            migrationBuilder.DropColumn(
                name: "DenumireCerere",
                table: "CereriDocumente");

            migrationBuilder.DropColumn(
                name: "DenumireClient",
                table: "CereriDocumente");
        }
    }
}
