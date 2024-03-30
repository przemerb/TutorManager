using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorManager.Migrations
{
    /// <inheritdoc />
    public partial class SplitUndo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserModel",
                table: "UserModel");

            migrationBuilder.RenameTable(
                name: "UserModel",
                newName: "TutorTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TutorTable",
                table: "TutorTable",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TutorTable",
                table: "TutorTable");

            migrationBuilder.RenameTable(
                name: "TutorTable",
                newName: "UserModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserModel",
                table: "UserModel",
                column: "Id");
        }
    }
}
