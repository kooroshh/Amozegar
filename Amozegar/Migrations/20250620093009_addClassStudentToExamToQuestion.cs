using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amozegar.Migrations
{
    /// <inheritdoc />
    public partial class addClassStudentToExamToQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudentsToExam_Exams_ExamId",
                table: "ClassStudentsToExam");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Exams_ExamId",
                table: "Questions");

            migrationBuilder.AddColumn<int>(
                name: "LastCompletedQuestion",
                table: "ClassStudentsToExam",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ClassStudentsToExamsToQuestions",
                columns: table => new
                {
                    ClassStudentsToExamQuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassStudentToExamId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    IsTrueAwnser = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStudentsToExamsToQuestions", x => x.ClassStudentsToExamQuestionId);
                    table.ForeignKey(
                        name: "FK_ClassStudentsToExamsToQuestions_ClassStudentsToExam_ClassStudentToExamId",
                        column: x => x.ClassStudentToExamId,
                        principalTable: "ClassStudentsToExam",
                        principalColumn: "ClassStudentsToExamId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassStudentsToExamsToQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudentsToExamsToQuestions_ClassStudentToExamId",
                table: "ClassStudentsToExamsToQuestions",
                column: "ClassStudentToExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudentsToExamsToQuestions_QuestionId",
                table: "ClassStudentsToExamsToQuestions",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudentsToExam_Exams_ExamId",
                table: "ClassStudentsToExam",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Exams_ExamId",
                table: "Questions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudentsToExam_Exams_ExamId",
                table: "ClassStudentsToExam");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Exams_ExamId",
                table: "Questions");

            migrationBuilder.DropTable(
                name: "ClassStudentsToExamsToQuestions");

            migrationBuilder.DropColumn(
                name: "LastCompletedQuestion",
                table: "ClassStudentsToExam");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudentsToExam_Exams_ExamId",
                table: "ClassStudentsToExam",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Exams_ExamId",
                table: "Questions",
                column: "ExamId",
                principalTable: "Exams",
                principalColumn: "ExamId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
