using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amozegar.Migrations
{
    /// <inheritdoc />
    public partial class editSomeBugsForViewa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "UsersViews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UsersViews_ClassId",
                table: "UsersViews",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersViews_Classes_ClassId",
                table: "UsersViews",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersViews_Classes_ClassId",
                table: "UsersViews");

            migrationBuilder.DropIndex(
                name: "IX_UsersViews_ClassId",
                table: "UsersViews");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "UsersViews");
        }
    }
}
