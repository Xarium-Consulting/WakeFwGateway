using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WakeFW_Server_UI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TargetNetworkDevice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ip = table.Column<string>(type: "TEXT", nullable: false),
                    Mac = table.Column<string>(type: "TEXT", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetNetworkDevice", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TargetNetworkDevice");
        }
    }
}
