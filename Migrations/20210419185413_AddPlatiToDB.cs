using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Migrations
{
    public partial class AddPlatiToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipPlati",
                columns: table => new
                {
                    TipPlataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denumire = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipPlati", x => x.TipPlataId);
                });

            migrationBuilder.CreateTable(
                name: "Plati",
                columns: table => new
                {
                    PlataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Suma = table.Column<float>(type: "real", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataScadenta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipPlataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plati", x => x.PlataId);
                    table.ForeignKey(
                        name: "FK_Plati_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Plati_TipPlati_TipPlataId",
                        column: x => x.TipPlataId,
                        principalTable: "TipPlati",
                        principalColumn: "TipPlataId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plati_ClientId",
                table: "Plati",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Plati_TipPlataId",
                table: "Plati",
                column: "TipPlataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plati");

            migrationBuilder.DropTable(
                name: "TipPlati");
        }
    }
}
