using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class SalariatAndIstoricSalarToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Salariat",
                columns: table => new
                {
                    SalariatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume = table.Column<string>(nullable: false),
                    Prenume = table.Column<string>(nullable: false),
                    Pozitie = table.Column<string>(nullable: false),
                    DataAngajare = table.Column<DateTime>(nullable: false),
                    DataConcediere = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salariat", x => x.SalariatId);
                });

            migrationBuilder.CreateTable(
                name: "IstoricSalar",
                columns: table => new
                {
                    IstoricSalarId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalariatId = table.Column<int>(nullable: false),
                    DataInceput = table.Column<DateTime>(nullable: false),
                    DataSfarsit = table.Column<DateTime>(nullable: false),
                    Salariu = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IstoricSalar", x => x.IstoricSalarId);
                    table.ForeignKey(
                        name: "FK_IstoricSalar_Salariat_SalariatId",
                        column: x => x.SalariatId,
                        principalTable: "Salariat",
                        principalColumn: "SalariatId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IstoricSalar_SalariatId",
                table: "IstoricSalar",
                column: "SalariatId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IstoricSalar");

            migrationBuilder.DropTable(
                name: "Salariat");
        }
    }
}
