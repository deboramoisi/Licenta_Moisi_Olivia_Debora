using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class NotificariToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notificari",
                columns: table => new
                {
                    NotificareId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificari", x => x.NotificareId);
                });

            migrationBuilder.CreateTable(
                name: "NotificareUsers",
                columns: table => new
                {
                    NotificareId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Seen = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificareUsers", x => new { x.NotificareId, x.ApplicationUserId });
                    table.ForeignKey(
                        name: "FK_NotificareUsers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificareUsers_Notificari_NotificareId",
                        column: x => x.NotificareId,
                        principalTable: "Notificari",
                        principalColumn: "NotificareId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificareUsers_ApplicationUserId",
                table: "NotificareUsers",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificareUsers");

            migrationBuilder.DropTable(
                name: "Notificari");
        }
    }
}
