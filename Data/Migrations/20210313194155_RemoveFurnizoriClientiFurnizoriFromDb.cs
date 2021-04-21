using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class RemoveFurnizoriClientiFurnizoriFromDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientFurnizor");

            migrationBuilder.DropTable(
                name: "Furnizor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Furnizor",
                columns: table => new
                {
                    FurnizorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denumire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    adresa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cod_fiscal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    judet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    tara = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Furnizor", x => x.FurnizorID);
                });

            migrationBuilder.CreateTable(
                name: "ClientFurnizor",
                columns: table => new
                {
                    ClientFurnizorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    FurnizorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFurnizor", x => x.ClientFurnizorId);
                    table.ForeignKey(
                        name: "FK_ClientFurnizor_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientFurnizor_Furnizor_FurnizorId",
                        column: x => x.FurnizorId,
                        principalTable: "Furnizor",
                        principalColumn: "FurnizorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientFurnizor_ClientId",
                table: "ClientFurnizor",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientFurnizor_FurnizorId",
                table: "ClientFurnizor",
                column: "FurnizorId");
        }
    }
}
