using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNgaySinhGioiTinhUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GioiTinh",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgaySinh",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GioiTinh",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NgaySinh",
                table: "Users");
        }
    }
}
