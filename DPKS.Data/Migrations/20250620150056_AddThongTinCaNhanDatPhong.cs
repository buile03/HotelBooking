using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddThongTinCaNhanDatPhong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "DatPhong",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "DatPhong",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoTen",
                table: "DatPhong",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SDT",
                table: "DatPhong",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "DatPhong");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "DatPhong");

            migrationBuilder.DropColumn(
                name: "HoTen",
                table: "DatPhong");

            migrationBuilder.DropColumn(
                name: "SDT",
                table: "DatPhong");
        }
    }
}
