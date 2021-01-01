using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class ClientToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    ClientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Denumire = table.Column<string>(nullable: true),
                    NrRegComertului = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "SediuSocial",
                columns: table => new
                {
                    SediuSocialId = table.Column<int>(nullable: false),
                    Localitate = table.Column<string>(nullable: true),
                    Judet = table.Column<string>(nullable: true),
                    Sector = table.Column<int>(nullable: false),
                    Strada = table.Column<string>(nullable: true),
                    Numar = table.Column<string>(nullable: true),
                    CodPostal = table.Column<int>(maxLength: 6, nullable: false),
                    Bl = table.Column<string>(nullable: true),
                    Sc = table.Column<int>(nullable: false),
                    Et = table.Column<int>(nullable: false),
                    Ap = table.Column<int>(nullable: false),
                    Telefon = table.Column<string>(maxLength: 10, nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SediuSocial", x => x.SediuSocialId);
                    table.ForeignKey(
                        name: "FK_SediuSocial_Client_SediuSocialId",
                        column: x => x.SediuSocialId,
                        principalTable: "Client",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SediuSocial");

            migrationBuilder.DropTable(
                name: "Client");
        }
    }
}
