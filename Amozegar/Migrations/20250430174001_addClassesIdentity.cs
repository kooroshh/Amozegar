using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amozegar.Migrations
{
    /// <inheritdoc />
    public partial class addClassesIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClassIdentity",
                table: "Classes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassIdentity",
                table: "Classes");
        }
    }
}
