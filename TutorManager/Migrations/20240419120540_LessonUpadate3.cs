using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorManager.Migrations
{
    /// <inheritdoc />
    public partial class LessonUpadate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "LessonTable",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject",
                table: "LessonTable");
        }
    }
}
