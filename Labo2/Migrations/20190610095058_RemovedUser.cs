using Microsoft.EntityFrameworkCore.Migrations;

namespace Labo2.Migrations
{
    public partial class RemovedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Removed",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AddedById",
                table: "UserRoles",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_AddedById",
                table: "UserRoles",
                column: "AddedById");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_AddedById",
                table: "UserRoles",
                column: "AddedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_AddedById",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_AddedById",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "Removed",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddedById",
                table: "UserRoles");
        }
    }
}
