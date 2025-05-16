using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amozegar.Migrations
{
    /// <inheritdoc />
    public partial class editSomeBugs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassStudentId",
                table: "StudentsHomeworks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentsHomeworks_ClassStudentId",
                table: "StudentsHomeworks",
                column: "ClassStudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsHomeworks_ClassesStudents_ClassStudentId",
                table: "StudentsHomeworks",
                column: "ClassStudentId",
                principalTable: "ClassesStudents",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentsHomeworks_ClassesStudents_ClassStudentId",
                table: "StudentsHomeworks");

            migrationBuilder.DropIndex(
                name: "IX_StudentsHomeworks_ClassStudentId",
                table: "StudentsHomeworks");

            migrationBuilder.DropColumn(
                name: "ClassStudentId",
                table: "StudentsHomeworks");
        }
    }
}
