using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedAllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TrangThaiPhong",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TrangThaiDatPhong",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tinh",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TienNghi",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ThanhToan",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QuocGia",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PhuongThucThanhToan",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Organizations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LoaiPhong",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FeedBacks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DatPhong",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AnhPhong",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AnhLoaiPhong",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TrangThaiPhong");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TrangThaiDatPhong");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tinh");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TienNghi");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ThanhToan");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QuocGia");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PhuongThucThanhToan");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LoaiPhong");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FeedBacks");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DatPhong");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AnhPhong");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AnhLoaiPhong");
        }
    }
}
