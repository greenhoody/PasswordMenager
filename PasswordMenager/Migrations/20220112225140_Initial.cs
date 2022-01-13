using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordMenager.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientPasswordIndexVM");

            migrationBuilder.DropTable(
                name: "ClientPasswordVM");

            migrationBuilder.AddColumn<byte[]>(
                name: "Password",
                table: "clientPasswords",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "clientPasswords");

            migrationBuilder.CreateTable(
                name: "ClientPasswordIndexVM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URI = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPasswordIndexVM", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientPasswordVM",
                columns: table => new
                {
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URI = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });
        }
    }
}
