using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorManager.Migrations
{
    /// <inheritdoc />
    public partial class TablesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gratification",
                table: "TutorTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "NumOfStudents",
                table: "TutorTable",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Charge",
                table: "StudentTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LessonTable",
                columns: table => new
                {
                    LessonId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TutorId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonTable", x => x.LessonId);
                    table.ForeignKey(
                        name: "FK_LessonTable_StudentTable_StudentId",
                        column: x => x.StudentId,
                        principalTable: "StudentTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonTable_TutorTable_TutorId",
                        column: x => x.TutorId,
                        principalTable: "TutorTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LessonTable_StudentId",
                table: "LessonTable",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonTable_TutorId",
                table: "LessonTable",
                column: "TutorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LessonTable");

            migrationBuilder.DropColumn(
                name: "Gratification",
                table: "TutorTable");

            migrationBuilder.DropColumn(
                name: "NumOfStudents",
                table: "TutorTable");

            migrationBuilder.DropColumn(
                name: "Charge",
                table: "StudentTable");
        }
    }
}
