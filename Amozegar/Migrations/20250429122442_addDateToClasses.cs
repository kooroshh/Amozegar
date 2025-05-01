using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amozegar.Migrations
{
    /// <inheritdoc />
    public partial class addDateToClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ClassImage",
                table: "Classes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "classes.png",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Classes",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Classes");

            migrationBuilder.AlterColumn<string>(
                name: "ClassImage",
                table: "Classes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "classes.png");
        }
    }
}
