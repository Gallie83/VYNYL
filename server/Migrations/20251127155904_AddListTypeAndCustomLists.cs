using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VynylAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddListTypeAndCustomLists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAlbums",
                table: "UserAlbums");

            migrationBuilder.AddColumn<int>(
                name: "ListType",
                table: "UserAlbums",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CustomListId",
                table: "UserAlbums",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAlbums",
                table: "UserAlbums",
                columns: new[] { "UserId", "AlbumId", "ListType" });

            migrationBuilder.CreateTable(
                name: "CustomLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAlbums_CustomListId",
                table: "UserAlbums",
                column: "CustomListId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomLists_UserId",
                table: "CustomLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAlbums_CustomLists_CustomListId",
                table: "UserAlbums",
                column: "CustomListId",
                principalTable: "CustomLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAlbums_CustomLists_CustomListId",
                table: "UserAlbums");

            migrationBuilder.DropTable(
                name: "CustomLists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAlbums",
                table: "UserAlbums");

            migrationBuilder.DropIndex(
                name: "IX_UserAlbums_CustomListId",
                table: "UserAlbums");

            migrationBuilder.DropColumn(
                name: "ListType",
                table: "UserAlbums");

            migrationBuilder.DropColumn(
                name: "CustomListId",
                table: "UserAlbums");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAlbums",
                table: "UserAlbums",
                columns: new[] { "UserId", "AlbumId" });
        }
    }
}
