using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnhLoaiPhong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "TienNghi",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DienTich",
                table: "LoaiPhong",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "HinhAnhChinh",
                table: "LoaiPhong",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AnhLoaiPhong",
                columns: table => new
                {
                    PhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LoaiPhongId = table.Column<int>(type: "int", nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnhLoaiPhong", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_AnhLoaiPhong_LoaiPhong_LoaiPhongId",
                        column: x => x.LoaiPhongId,
                        principalTable: "LoaiPhong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnhLoaiPhong_LoaiPhongId",
                table: "AnhLoaiPhong",
                column: "LoaiPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_AnhLoaiPhong_PhotoName",
                table: "AnhLoaiPhong",
                column: "PhotoName",
                unique: true,
                filter: "[PhotoName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnhLoaiPhong");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "TienNghi");

            migrationBuilder.DropColumn(
                name: "DienTich",
                table: "LoaiPhong");

            migrationBuilder.DropColumn(
                name: "HinhAnhChinh",
                table: "LoaiPhong");
        }
    }
}
