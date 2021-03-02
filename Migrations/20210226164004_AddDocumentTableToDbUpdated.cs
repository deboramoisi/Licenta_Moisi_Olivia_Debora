using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Migrations
{
    public partial class AddDocumentTableToDbUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_AspNetUsers_Id",
                table: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Document_Id",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Document");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Document",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Document_ApplicationUserId",
                table: "Document",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_AspNetUsers_ApplicationUserId",
                table: "Document",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_AspNetUsers_ApplicationUserId",
                table: "Document");

            migrationBuilder.DropIndex(
                name: "IX_Document_ApplicationUserId",
                table: "Document");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Document",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Document",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Document_Id",
                table: "Document",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_AspNetUsers_Id",
                table: "Document",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
