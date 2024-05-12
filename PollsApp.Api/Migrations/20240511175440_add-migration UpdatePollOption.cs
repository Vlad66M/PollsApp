using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PollsApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class addmigrationUpdatePollOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "text",
                table: "poll_options",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<byte[]>(
                name: "audio",
                table: "poll_options",
                type: "BLOB",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "photo",
                table: "poll_options",
                type: "BLOB",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "audio",
                table: "poll_options");

            migrationBuilder.DropColumn(
                name: "photo",
                table: "poll_options");

            migrationBuilder.AlterColumn<string>(
                name: "text",
                table: "poll_options",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);
        }
    }
}
