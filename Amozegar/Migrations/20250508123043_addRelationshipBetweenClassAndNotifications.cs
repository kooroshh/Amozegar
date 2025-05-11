using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amozegar.Migrations
{
    /// <inheritdoc />
    public partial class addRelationshipBetweenClassAndNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ClassId",
                table: "Notifications",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Classes_ClassId",
                table: "Notifications",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Classes_ClassId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ClassId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Notifications");
        }
    }
}
