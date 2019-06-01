using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Labo2.Migrations
{
    public partial class AddCommentForExpense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpenseId",
                table: "Comment",
                 nullable: true);
            migrationBuilder.CreateIndex(
               name: "IX_Comment_ExpenseId",
               table: "Comment",
               column: "ExpenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commens_Expenses_ExpenseId",
                table: "Comment",
                column: "ExpenseId",
                principalTable: "Expenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Expenses_ExpenseId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ExpenseId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "ExpenseId",
                table: "Comment");
        }
    }
}