using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class ClientFurnizorMN : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Furnizor",
                columns: table => new
                {
                    FurnizorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denumire = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Furnizor", x => x.FurnizorID);
                });

            migrationBuilder.CreateTable(
                name: "ClientFurnizor",
                columns: table => new
                {
                    ClientFurnizorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(nullable: false),
                    FurnizorId = table.Column<int>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientFurnizor");

            migrationBuilder.DropTable(
                name: "Furnizor");
        }
    }
}
