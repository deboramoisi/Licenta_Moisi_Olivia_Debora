using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class ColumnRedirectToPageNotificariTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RedirectToPage",
                table: "Notificari",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedirectToPage",
                table: "Notificari");
        }
    }
}
