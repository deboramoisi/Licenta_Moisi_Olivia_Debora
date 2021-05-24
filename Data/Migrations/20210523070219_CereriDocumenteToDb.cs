using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class CereriDocumenteToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CerereDocumentId",
                table: "Salariat",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CereriDocumente",
                columns: table => new
                {
                    CerereDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TipCerereId = table.Column<int>(type: "int", nullable: false),
                    SalariatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CereriDocumente", x => x.CerereDocumentId);
                    table.ForeignKey(
                        name: "FK_CereriDocumente_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TipCereri",
                columns: table => new
                {
                    TipCerereId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denumire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CerereDocumentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipCereri", x => x.TipCerereId);
                    table.ForeignKey(
                        name: "FK_TipCereri_CereriDocumente_CerereDocumentId",
                        column: x => x.CerereDocumentId,
                        principalTable: "CereriDocumente",
                        principalColumn: "CerereDocumentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Salariat_CerereDocumentId",
                table: "Salariat",
                column: "CerereDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_CereriDocumente_ClientId",
                table: "CereriDocumente",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_TipCereri_CerereDocumentId",
                table: "TipCereri",
                column: "CerereDocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Salariat_CereriDocumente_CerereDocumentId",
                table: "Salariat",
                column: "CerereDocumentId",
                principalTable: "CereriDocumente",
                principalColumn: "CerereDocumentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salariat_CereriDocumente_CerereDocumentId",
                table: "Salariat");

            migrationBuilder.DropTable(
                name: "TipCereri");

            migrationBuilder.DropTable(
                name: "CereriDocumente");

            migrationBuilder.DropIndex(
                name: "IX_Salariat_CerereDocumentId",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "CerereDocumentId",
                table: "Salariat");
        }
    }
}
