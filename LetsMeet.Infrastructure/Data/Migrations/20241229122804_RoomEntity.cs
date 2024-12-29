using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsMeet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RoomEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoomId",
                schema: "LetsMeet",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Rooms",
                schema: "LetsMeet",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IsLocked = table.Column<bool>(type: "boolean", nullable: false),
                    EntityStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoomId",
                schema: "LetsMeet",
                table: "AspNetUsers",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Rooms_RoomId",
                schema: "LetsMeet",
                table: "AspNetUsers",
                column: "RoomId",
                principalSchema: "LetsMeet",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Rooms_RoomId",
                schema: "LetsMeet",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Rooms",
                schema: "LetsMeet");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoomId",
                schema: "LetsMeet",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoomId",
                schema: "LetsMeet",
                table: "AspNetUsers");
        }
    }
}
