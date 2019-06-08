using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Labo2.Migrations
{
    public partial class AddDataRegisteredInUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DataRegistered",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataRegistered",
                table: "Users");
        }
    }
}
