using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class ModificariSalariatIstoricClientTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IstoricSalar_SalariatId",
                table: "IstoricSalar");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Salariat",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Salariat_ClientId",
                table: "Salariat",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_IstoricSalar_SalariatId",
                table: "IstoricSalar",
                column: "SalariatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Salariat_Client_ClientId",
                table: "Salariat",
                column: "ClientId",
                principalTable: "Client",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salariat_Client_ClientId",
                table: "Salariat");

            migrationBuilder.DropIndex(
                name: "IX_Salariat_ClientId",
                table: "Salariat");

            migrationBuilder.DropIndex(
                name: "IX_IstoricSalar_SalariatId",
                table: "IstoricSalar");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Salariat");

            migrationBuilder.CreateIndex(
                name: "IX_IstoricSalar_SalariatId",
                table: "IstoricSalar",
                column: "SalariatId",
                unique: true);
        }
    }
}
