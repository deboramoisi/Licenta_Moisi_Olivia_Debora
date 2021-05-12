using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class AddAchitataAndSuccesPlataToPlata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Achitata",
                table: "Plati",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "SuccesPlata",
                table: "Plati",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Achitata",
                table: "Plati");

            migrationBuilder.DropColumn(
                name: "SuccesPlata",
                table: "Plati");
        }
    }
}
