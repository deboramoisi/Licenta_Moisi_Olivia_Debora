using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Migrations
{
    public partial class ModifySolduriTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SolduriCasa",
                table: "SolduriCasa");

            migrationBuilder.AddColumn<int>(
                name: "SolduriCasaId",
                table: "SolduriCasa",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolduriCasa",
                table: "SolduriCasa",
                column: "SolduriCasaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SolduriCasa",
                table: "SolduriCasa");

            migrationBuilder.DropColumn(
                name: "SolduriCasaId",
                table: "SolduriCasa");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SolduriCasa",
                table: "SolduriCasa",
                column: "data");
        }
    }
}
