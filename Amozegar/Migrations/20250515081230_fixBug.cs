using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amozegar.Migrations
{
    /// <inheritdoc />
    public partial class fixBug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Homeworks");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Homeworks",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Homeworks");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Homeworks",
                type: "datetime2",
                nullable: true);
        }
    }
}
