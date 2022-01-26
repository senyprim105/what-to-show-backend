using Microsoft.EntityFrameworkCore.Migrations;

namespace server_api.Migrations
{
    public partial class FixGenreTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OldName",
                table: "Movies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "OldName",
                table: "Genres",
                newName: "Name");

            migrationBuilder.RenameIndex(
                name: "IX_Genres_OldName",
                table: "Genres",
                newName: "IX_Genres_Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Movies",
                newName: "OldName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Genres",
                newName: "OldName");

            migrationBuilder.RenameIndex(
                name: "IX_Genres_Name",
                table: "Genres",
                newName: "IX_Genres_OldName");
        }
    }
}
