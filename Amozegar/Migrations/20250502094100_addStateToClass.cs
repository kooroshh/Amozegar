using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amozegar.Migrations
{
    /// <inheritdoc />
    public partial class addStateToClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CLassStateId",
                table: "Classes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClassStates",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PersianState = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStates", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CLassStateId",
                table: "Classes",
                column: "CLassStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_ClassStates_CLassStateId",
                table: "Classes",
                column: "CLassStateId",
                principalTable: "ClassStates",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_ClassStates_CLassStateId",
                table: "Classes");

            migrationBuilder.DropTable(
                name: "ClassStates");

            migrationBuilder.DropIndex(
                name: "IX_Classes_CLassStateId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "CLassStateId",
                table: "Classes");
        }
    }
}
