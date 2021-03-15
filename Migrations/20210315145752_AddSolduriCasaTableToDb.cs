using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Migrations
{
    public partial class AddSolduriCasaTableToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SolduriCasa",
                columns: table => new
                {
                    data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    sold_prec = table.Column<float>(type: "real", nullable: false),
                    incasari = table.Column<float>(type: "real", nullable: false),
                    plati = table.Column<float>(type: "real", nullable: false),
                    sold_zi = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolduriCasa", columns: x => new { x.data, x.ClientId });
                    table.ForeignKey(
                        name: "FK_SolduriCasa_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolduriCasa_ClientId",
                table: "SolduriCasa",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolduriCasa");
        }
    }
}
