using Microsoft.EntityFrameworkCore.Migrations;

namespace Labo2.Migrations
{
    public partial class MakeUsernameUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Expenses_ExpenseId",
                table: "Comment");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedById",
                table: "Expenses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AddedById",
                table: "Comment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_AddedById",
                table: "Expenses",
                column: "AddedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_AddedById",
                table: "Comment",
                column: "AddedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Users_AddedById",
                table: "Comment",
                column: "AddedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Expenses_ExpenseId",
                table: "Comment",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Users_AddedById",
                table: "Expenses",
                column: "AddedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Users_AddedById",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Expenses_ExpenseId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Users_AddedById",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_AddedById",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Comment_AddedById",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "AddedById",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "AddedById",
                table: "Comment");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Expenses_ExpenseId",
                table: "Comment",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
