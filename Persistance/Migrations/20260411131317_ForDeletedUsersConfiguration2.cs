using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ForDeletedUsersConfiguration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_Jobs_JobsId",
                table: "AppUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AppUsers_UserId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_AppUsers_JobsId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "JobsId",
                table: "AppUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AppUsers_UserId",
                table: "Jobs",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_AppUsers_UserId",
                table: "Jobs");

            migrationBuilder.AddColumn<Guid>(
                name: "JobsId",
                table: "AppUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_JobsId",
                table: "AppUsers",
                column: "JobsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_Jobs_JobsId",
                table: "AppUsers",
                column: "JobsId",
                principalTable: "Jobs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_AppUsers_UserId",
                table: "Jobs",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }
    }
}
