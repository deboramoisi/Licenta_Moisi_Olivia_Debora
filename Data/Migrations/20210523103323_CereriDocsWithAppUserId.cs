using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class CereriDocsWithAppUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CereriDocumente_Client_ClientId",
                table: "CereriDocumente");

            migrationBuilder.DropIndex(
                name: "IX_CereriDocumente_ClientId",
                table: "CereriDocumente");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "CereriDocumente");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "CereriDocumente",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CereriDocumente_ApplicationUserId",
                table: "CereriDocumente",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CereriDocumente_AspNetUsers_ApplicationUserId",
                table: "CereriDocumente",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CereriDocumente_AspNetUsers_ApplicationUserId",
                table: "CereriDocumente");

            migrationBuilder.DropIndex(
                name: "IX_CereriDocumente_ApplicationUserId",
                table: "CereriDocumente");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "CereriDocumente");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "CereriDocumente",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CereriDocumente_ClientId",
                table: "CereriDocumente",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_CereriDocumente_Client_ClientId",
                table: "CereriDocumente",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
