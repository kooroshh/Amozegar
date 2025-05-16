using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amozegar.Migrations
{
    /// <inheritdoc />
    public partial class addHomeworksTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeworksStates",
                columns: table => new
                {
                    HomeworkStateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PersianState = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeworksStates", x => x.HomeworkStateId);
                });

            migrationBuilder.CreateTable(
                name: "StudentsHomeworskStates",
                columns: table => new
                {
                    StudentHomeworkStateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PersianState = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsHomeworskStates", x => x.StudentHomeworkStateId);
                });

            migrationBuilder.CreateTable(
                name: "Homeworks",
                columns: table => new
                {
                    HomeworkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    HomeworkStateId = table.Column<int>(type: "int", nullable: false),
                    HomeworkTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HomeworkDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homeworks", x => x.HomeworkId);
                    table.ForeignKey(
                        name: "FK_Homeworks_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "ClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Homeworks_HomeworksStates_HomeworkStateId",
                        column: x => x.HomeworkStateId,
                        principalTable: "HomeworksStates",
                        principalColumn: "HomeworkStateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentsHomeworks",
                columns: table => new
                {
                    StudentHomeworkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeworkId = table.Column<int>(type: "int", nullable: false),
                    StudentHomeworkStateId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsHomeworks", x => x.StudentHomeworkId);
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
                name: "IX_Homeworks_ClassId",
                table: "Homeworks",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Homeworks_HomeworkStateId",
                table: "Homeworks",
                column: "HomeworkStateId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsHomeworks_HomeworkId",
                table: "StudentsHomeworks",
                column: "HomeworkId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsHomeworks_StudentHomeworkStateId",
                table: "StudentsHomeworks",
                column: "StudentHomeworkStateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentsHomeworks");

            migrationBuilder.DropTable(
                name: "Homeworks");

            migrationBuilder.DropTable(
                name: "StudentsHomeworskStates");

            migrationBuilder.DropTable(
                name: "HomeworksStates");
        }
    }
}
