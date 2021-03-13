using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Migrations
{
    public partial class FurnizoriTableToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "adresa",
                table: "Furnizor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cod_fiscal",
                table: "Furnizor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "judet",
                table: "Furnizor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tara",
                table: "Furnizor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Furnizori",
                columns: table => new
                {
                    FurnizorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    denumire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cod_fiscal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tara = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    judet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    adresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Furnizori", x => x.FurnizorID);
                    table.ForeignKey(
                        name: "FK_Furnizori_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Furnizori_ClientId",
                table: "Furnizori",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Furnizori");

            migrationBuilder.DropColumn(
                name: "adresa",
                table: "Furnizor");

            migrationBuilder.DropColumn(
                name: "cod_fiscal",
                table: "Furnizor");

            migrationBuilder.DropColumn(
                name: "judet",
                table: "Furnizor");

            migrationBuilder.DropColumn(
                name: "tara",
                table: "Furnizor");
        }
    }
}
