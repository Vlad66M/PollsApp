using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollsApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class Users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Roles_role_id",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "URoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_URoles",
                table: "URoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_URoles_role_id",
                table: "AspNetUsers",
                column: "role_id",
                principalTable: "URoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_URoles_role_id",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_URoles",
                table: "URoles");

            migrationBuilder.RenameTable(
                name: "URoles",
                newName: "Roles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Roles_role_id",
                table: "AspNetUsers",
                column: "role_id",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
