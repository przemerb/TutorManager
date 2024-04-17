using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTables2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Gratification",
                table: "TutorTable",
                newName: "SumGratification");

            migrationBuilder.AddColumn<int>(
                name: "ExpectedGratification",
                table: "TutorTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "TutorTable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LessonStatus",
                table: "LessonTable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "LessonTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TutorGratification",
                table: "LessonTable",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedGratification",
                table: "TutorTable");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "TutorTable");

            migrationBuilder.DropColumn(
                name: "LessonStatus",
                table: "LessonTable");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "LessonTable");

            migrationBuilder.DropColumn(
                name: "TutorGratification",
                table: "LessonTable");

            migrationBuilder.RenameColumn(
                name: "SumGratification",
                table: "TutorTable",
                newName: "Gratification");
        }
    }
}
