using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class UpdateUserWithProfileInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ProfileDetails_ProfileDetailsId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ProfileDetails");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfileDetailsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileDetailsId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "Descriere",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descriere",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "ProfileDetailsId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProfileDetails",
                columns: table => new
                {
                    ProfileDetailsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descriere = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileDetails", x => x.ProfileDetailsId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileDetailsId",
                table: "AspNetUsers",
                column: "ProfileDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ProfileDetails_ProfileDetailsId",
                table: "AspNetUsers",
                column: "ProfileDetailsId",
                principalTable: "ProfileDetails",
                principalColumn: "ProfileDetailsId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
