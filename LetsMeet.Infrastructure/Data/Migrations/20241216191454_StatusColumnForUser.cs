using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsMeet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StatusColumnForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                schema: "LetsMeet",
                table: "AspNetUsers",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                schema: "LetsMeet",
                table: "AspNetUsers");
        }
    }
}
