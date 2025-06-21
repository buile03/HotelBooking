using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIconPhuongThucThanhToan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "PhuongThucThanhToan",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "PhuongThucThanhToan");
        }
    }
}
