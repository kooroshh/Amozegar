using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amozegar.Migrations
{
    /// <inheritdoc />
    public partial class addDateToReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Reports",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Reports");
        }
    }
}
