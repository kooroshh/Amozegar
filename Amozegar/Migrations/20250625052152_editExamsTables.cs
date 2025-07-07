using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amozegar.Migrations
{
    /// <inheritdoc />
    public partial class editExamsTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions");

            migrationBuilder.AddColumn<int>(
                name: "SelectedOptionId",
                table: "ClassStudentsToExamsToQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudentsToExamsToQuestions_SelectedOptionId",
                table: "ClassStudentsToExamsToQuestions",
                column: "SelectedOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudentsToExamsToQuestions_QuestionOptions_SelectedOptionId",
                table: "ClassStudentsToExamsToQuestions",
                column: "SelectedOptionId",
                principalTable: "QuestionOptions",
                principalColumn: "QuestionOptionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudentsToExamsToQuestions_QuestionOptions_SelectedOptionId",
                table: "ClassStudentsToExamsToQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropIndex(
                name: "IX_ClassStudentsToExamsToQuestions_SelectedOptionId",
                table: "ClassStudentsToExamsToQuestions");

            migrationBuilder.DropColumn(
                name: "SelectedOptionId",
                table: "ClassStudentsToExamsToQuestions");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
