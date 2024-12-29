using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetsMeet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UsersRoomsConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Rooms_RoomId",
                schema: "LetsMeet",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoomId",
                schema: "LetsMeet",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoomId",
                schema: "LetsMeet",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AppUserRoom",
                schema: "LetsMeet",
                columns: table => new
                {
                    RoomsId = table.Column<string>(type: "text", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoom", x => new { x.RoomsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AppUserRoom_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "LetsMeet",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserRoom_Rooms_RoomsId",
                        column: x => x.RoomsId,
                        principalSchema: "LetsMeet",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoom_UsersId",
                schema: "LetsMeet",
                table: "AppUserRoom",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserRoom",
                schema: "LetsMeet");

            migrationBuilder.AddColumn<string>(
                name: "RoomId",
                schema: "LetsMeet",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

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
    }
}
