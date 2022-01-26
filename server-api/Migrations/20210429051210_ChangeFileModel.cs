using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace server_api.Migrations
{
    public partial class ChangeFileModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Files",
                newName: "OldPath");

            migrationBuilder.AddColumn<string>(
                name: "OldName",
                table: "Files",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Upload",
                table: "Files",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldName",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Upload",
                table: "Files");

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

            migrationBuilder.RenameColumn(
                name: "OldPath",
                table: "Files",
                newName: "Name");
        }
    }
}
