using Microsoft.EntityFrameworkCore.Migrations;

namespace server_api.Migrations
{
    public partial class addGenreField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Persons_FirstName_SecondName_LastName",
                table: "Persons");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Persons",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Genres",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_FirstName_SecondName_LastName",
                table: "Persons",
                columns: new[] { "FirstName", "SecondName", "LastName" },
                unique: true,
                filter: "[SecondName] IS NOT NULL AND [LastName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_Name",
                table: "Genres",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Persons_FirstName_SecondName_LastName",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Genres_Name",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "Caption",
                table: "Genres");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Persons",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Genres",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.CreateIndex(
                name: "IX_Persons_FirstName_SecondName_LastName",
                table: "Persons",
                columns: new[] { "FirstName", "SecondName", "LastName" },
                unique: true,
                filter: "[FirstName] IS NOT NULL AND [SecondName] IS NOT NULL AND [LastName] IS NOT NULL");
        }
    }
}
