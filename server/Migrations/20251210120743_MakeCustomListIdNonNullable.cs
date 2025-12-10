using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VynylAPI.Migrations
{
    /// <inheritdoc />
    public partial class MakeCustomListIdNonNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAlbums_CustomLists_CustomListId",
                table: "UserAlbums");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAlbums",
                table: "UserAlbums");

            migrationBuilder.AlterColumn<int>(
                name: "CustomListId",
                table: "UserAlbums",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAlbums",
                table: "UserAlbums",
                columns: new[] { "UserId", "AlbumId", "ListType", "CustomListId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserAlbums_CustomLists_CustomListId",
                table: "UserAlbums",
                column: "CustomListId",
                principalTable: "CustomLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAlbums_CustomLists_CustomListId",
                table: "UserAlbums");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAlbums",
                table: "UserAlbums");

            migrationBuilder.AlterColumn<int>(
                name: "CustomListId",
                table: "UserAlbums",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAlbums",
                table: "UserAlbums",
                columns: new[] { "UserId", "AlbumId", "ListType" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserAlbums_CustomLists_CustomListId",
                table: "UserAlbums",
                column: "CustomListId",
                principalTable: "CustomLists",
                principalColumn: "Id");
        }
    }
}
