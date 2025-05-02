using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amozegar.Migrations
{
    /// <inheritdoc />
    public partial class addStatesForClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_ClassStates_CLassStateId",
                table: "Classes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassStates",
                table: "ClassStates");

            migrationBuilder.RenameTable(
                name: "ClassStates",
                newName: "ClassesStates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassesStates",
                table: "ClassesStates",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_ClassesStates_CLassStateId",
                table: "Classes",
                column: "CLassStateId",
                principalTable: "ClassesStates",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_ClassesStates_CLassStateId",
                table: "Classes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassesStates",
                table: "ClassesStates");

            migrationBuilder.RenameTable(
                name: "ClassesStates",
                newName: "ClassStates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassStates",
                table: "ClassStates",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_ClassStates_CLassStateId",
                table: "Classes",
                column: "CLassStateId",
                principalTable: "ClassStates",
                principalColumn: "id");
        }
    }
}
