using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amozegar.Migrations
{
    /// <inheritdoc />
    public partial class fixedPicturesBug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Pictures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_ClassId",
                table: "Pictures",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Classes_ClassId",
                table: "Pictures",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Classes_ClassId",
                table: "Pictures");

            migrationBuilder.DropIndex(
                name: "IX_Pictures_ClassId",
                table: "Pictures");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Pictures");
        }
    }
}
