using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amozegar.Migrations
{
    /// <inheritdoc />
    public partial class editNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentsHomeworks");

            migrationBuilder.DropTable(
                name: "StudentsHomeworskStates");

            migrationBuilder.CreateTable(
                name: "ClassStudentsToHomeworkStates",
                columns: table => new
                {
                    ClassStudentsToHomeworkStateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PersianState = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStudentsToHomeworkStates", x => x.ClassStudentsToHomeworkStateId);
                });

            migrationBuilder.CreateTable(
                name: "ClassStudentsToHomeworks",
                columns: table => new
                {
                    ClassStudentsToHomeworkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeworkId = table.Column<int>(type: "int", nullable: false),
                    ClassStudentId = table.Column<int>(type: "int", nullable: false),
                    ClassStudentHomeworkStateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStudentsToHomeworks", x => x.ClassStudentsToHomeworkId);
                    table.ForeignKey(
                        name: "FK_ClassStudentsToHomeworks_ClassStudentsToHomeworkStates_ClassStudentHomeworkStateId",
                        column: x => x.ClassStudentHomeworkStateId,
                        principalTable: "ClassStudentsToHomeworkStates",
                        principalColumn: "ClassStudentsToHomeworkStateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassStudentsToHomeworks_ClassesStudents_ClassStudentId",
                        column: x => x.ClassStudentId,
                        principalTable: "ClassesStudents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassStudentsToHomeworks_Homeworks_HomeworkId",
                        column: x => x.HomeworkId,
                        principalTable: "Homeworks",
                        principalColumn: "HomeworkId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudentsToHomeworks_ClassStudentHomeworkStateId",
                table: "ClassStudentsToHomeworks",
                column: "ClassStudentHomeworkStateId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudentsToHomeworks_ClassStudentId",
                table: "ClassStudentsToHomeworks",
                column: "ClassStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudentsToHomeworks_HomeworkId",
                table: "ClassStudentsToHomeworks",
                column: "HomeworkId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClassStudentsToHomeworks");

            migrationBuilder.DropTable(
                name: "ClassStudentsToHomeworkStates");

            migrationBuilder.CreateTable(
                name: "StudentsHomeworskStates",
                columns: table => new
                {
                    StudentHomeworkStateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersianState = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    State = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsHomeworskStates", x => x.StudentHomeworkStateId);
                });

            migrationBuilder.CreateTable(
                name: "StudentsHomeworks",
                columns: table => new
                {
                    StudentHomeworkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassStudentId = table.Column<int>(type: "int", nullable: false),
                    HomeworkId = table.Column<int>(type: "int", nullable: false),
                    StudentHomeworkStateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsHomeworks", x => x.StudentHomeworkId);
                    table.ForeignKey(
                        name: "FK_StudentsHomeworks_ClassesStudents_ClassStudentId",
                        column: x => x.ClassStudentId,
                        principalTable: "ClassesStudents",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentsHomeworks_Homeworks_HomeworkId",
                        column: x => x.HomeworkId,
                        principalTable: "Homeworks",
                        principalColumn: "HomeworkId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentsHomeworks_StudentsHomeworskStates_StudentHomeworkStateId",
                        column: x => x.StudentHomeworkStateId,
                        principalTable: "StudentsHomeworskStates",
                        principalColumn: "StudentHomeworkStateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentsHomeworks_ClassStudentId",
                table: "StudentsHomeworks",
                column: "ClassStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsHomeworks_HomeworkId",
                table: "StudentsHomeworks",
                column: "HomeworkId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsHomeworks_StudentHomeworkStateId",
                table: "StudentsHomeworks",
                column: "StudentHomeworkStateId");
        }
    }
}
