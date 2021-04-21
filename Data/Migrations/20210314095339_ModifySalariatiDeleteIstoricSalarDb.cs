using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Licenta.Data.Migrations
{
    public partial class ModifySalariatiDeleteIstoricSalarDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IstoricSalar");

            migrationBuilder.DropColumn(
                name: "DataAngajare",
                table: "Salariat");

            migrationBuilder.RenameColumn(
                name: "Pozitie",
                table: "Salariat",
                newName: "functie");

            migrationBuilder.RenameColumn(
                name: "DataConcediere",
                table: "Salariat",
                newName: "datai");

            migrationBuilder.AddColumn<string>(
                name: "cn",
                table: "Salariat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "cod_post",
                table: "Salariat",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "d_contract",
                table: "Salariat",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "grupa",
                table: "Salariat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "judet",
                table: "Salariat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "localitate",
                table: "Salariat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "locatie",
                table: "Salariat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nr",
                table: "Salariat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "nr_contr",
                table: "Salariat",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "nr_zile_co",
                table: "Salariat",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ore_zi",
                table: "Salariat",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "salar_brut",
                table: "Salariat",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "str",
                table: "Salariat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tip",
                table: "Salariat",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "tip_rem",
                table: "Salariat",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cn",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "cod_post",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "d_contract",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "grupa",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "judet",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "localitate",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "locatie",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "nr",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "nr_contr",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "nr_zile_co",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "ore_zi",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "salar_brut",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "str",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "tip",
                table: "Salariat");

            migrationBuilder.DropColumn(
                name: "tip_rem",
                table: "Salariat");

            migrationBuilder.RenameColumn(
                name: "functie",
                table: "Salariat",
                newName: "Pozitie");

            migrationBuilder.RenameColumn(
                name: "datai",
                table: "Salariat",
                newName: "DataConcediere");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataAngajare",
                table: "Salariat",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "IstoricSalar",
                columns: table => new
                {
                    IstoricSalarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataInceput = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataSfarsit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SalariatId = table.Column<int>(type: "int", nullable: false),
                    Salariu = table.Column<float>(type: "real", nullable: false)
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
                column: "SalariatId");
        }
    }
}
