using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorManager.Migrations
{
    /// <inheritdoc />
    public partial class AddIsTutor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTutor",
                table: "UsersTable",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTutor",
                table: "UsersTable");
        }
    }
}
