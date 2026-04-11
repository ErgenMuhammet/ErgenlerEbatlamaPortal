using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ForDeletedUsersConfiguration4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_AppUsers_OwnerId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_Income_AppUsers_OwnerId",
                table: "Income");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AppUsers_OwnerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfitLossSituation_AppUsers_OwnerId",
                table: "ProfitLossSituation");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_AppUsers_OwnerId",
                table: "Expense",
                column: "OwnerId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Income_AppUsers_OwnerId",
                table: "Income",
                column: "OwnerId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AppUsers_OwnerId",
                table: "Orders",
                column: "OwnerId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfitLossSituation_AppUsers_OwnerId",
                table: "ProfitLossSituation",
                column: "OwnerId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_AppUsers_OwnerId",
                table: "Expense");

            migrationBuilder.DropForeignKey(
                name: "FK_Income_AppUsers_OwnerId",
                table: "Income");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AppUsers_OwnerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfitLossSituation_AppUsers_OwnerId",
                table: "ProfitLossSituation");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_AppUsers_OwnerId",
                table: "Expense",
                column: "OwnerId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Income_AppUsers_OwnerId",
                table: "Income",
                column: "OwnerId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AppUsers_OwnerId",
                table: "Orders",
                column: "OwnerId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfitLossSituation_AppUsers_OwnerId",
                table: "ProfitLossSituation",
                column: "OwnerId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }
    }
}
