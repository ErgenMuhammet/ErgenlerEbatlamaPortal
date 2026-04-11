using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ForDeletedUsersConfiguration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BackPanels_AppUsers_OwnerID",
                table: "BackPanels");

            migrationBuilder.DropForeignKey(
                name: "FK_Glues_AppUsers_OwnerID",
                table: "Glues");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_AppUsers_OwnerId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Mdfs_AppUsers_OwnerID",
                table: "Mdfs");

            migrationBuilder.AddForeignKey(
                name: "FK_BackPanels_AppUsers_OwnerID",
                table: "BackPanels",
                column: "OwnerID",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Glues_AppUsers_OwnerID",
                table: "Glues",
                column: "OwnerID",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_AppUsers_OwnerId",
                table: "Invoices",
                column: "OwnerId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Mdfs_AppUsers_OwnerID",
                table: "Mdfs",
                column: "OwnerID",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BackPanels_AppUsers_OwnerID",
                table: "BackPanels");

            migrationBuilder.DropForeignKey(
                name: "FK_Glues_AppUsers_OwnerID",
                table: "Glues");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_AppUsers_OwnerId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Mdfs_AppUsers_OwnerID",
                table: "Mdfs");

            migrationBuilder.AddForeignKey(
                name: "FK_BackPanels_AppUsers_OwnerID",
                table: "BackPanels",
                column: "OwnerID",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Glues_AppUsers_OwnerID",
                table: "Glues",
                column: "OwnerID",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_AppUsers_OwnerId",
                table: "Invoices",
                column: "OwnerId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mdfs_AppUsers_OwnerID",
                table: "Mdfs",
                column: "OwnerID",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }
    }
}
