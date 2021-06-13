using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class RemoveColumnsFromClient2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipFirma",
                table: "Client");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipFirma",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
