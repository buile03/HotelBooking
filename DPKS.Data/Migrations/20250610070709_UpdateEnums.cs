using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tienNghiTheoLoaiPhongs_LoaiPhong_LoaiPhongId",
                table: "tienNghiTheoLoaiPhongs");

            migrationBuilder.DropForeignKey(
                name: "FK_tienNghiTheoLoaiPhongs_TienNghi_TienNghiId",
                table: "tienNghiTheoLoaiPhongs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tienNghiTheoLoaiPhongs",
                table: "tienNghiTheoLoaiPhongs");

            migrationBuilder.RenameTable(
                name: "tienNghiTheoLoaiPhongs",
                newName: "TienNghiTheoLoaiPhongs");

            migrationBuilder.RenameIndex(
                name: "IX_tienNghiTheoLoaiPhongs_TienNghiId",
                table: "TienNghiTheoLoaiPhongs",
                newName: "IX_TienNghiTheoLoaiPhongs_TienNghiId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TienNghiTheoLoaiPhongs",
                table: "TienNghiTheoLoaiPhongs",
                columns: new[] { "LoaiPhongId", "TienNghiId" });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_TienNghiTheoLoaiPhongs_LoaiPhong_LoaiPhongId",
                table: "TienNghiTheoLoaiPhongs",
                column: "LoaiPhongId",
                principalTable: "LoaiPhong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TienNghiTheoLoaiPhongs_TienNghi_TienNghiId",
                table: "TienNghiTheoLoaiPhongs",
                column: "TienNghiId",
                principalTable: "TienNghi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TienNghiTheoLoaiPhongs_LoaiPhong_LoaiPhongId",
                table: "TienNghiTheoLoaiPhongs");

            migrationBuilder.DropForeignKey(
                name: "FK_TienNghiTheoLoaiPhongs_TienNghi_TienNghiId",
                table: "TienNghiTheoLoaiPhongs");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TienNghiTheoLoaiPhongs",
                table: "TienNghiTheoLoaiPhongs");

            migrationBuilder.RenameTable(
                name: "TienNghiTheoLoaiPhongs",
                newName: "tienNghiTheoLoaiPhongs");

            migrationBuilder.RenameIndex(
                name: "IX_TienNghiTheoLoaiPhongs_TienNghiId",
                table: "tienNghiTheoLoaiPhongs",
                newName: "IX_tienNghiTheoLoaiPhongs_TienNghiId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tienNghiTheoLoaiPhongs",
                table: "tienNghiTheoLoaiPhongs",
                columns: new[] { "LoaiPhongId", "TienNghiId" });

            migrationBuilder.AddForeignKey(
                name: "FK_tienNghiTheoLoaiPhongs_LoaiPhong_LoaiPhongId",
                table: "tienNghiTheoLoaiPhongs",
                column: "LoaiPhongId",
                principalTable: "LoaiPhong",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tienNghiTheoLoaiPhongs_TienNghi_TienNghiId",
                table: "tienNghiTheoLoaiPhongs",
                column: "TienNghiId",
                principalTable: "TienNghi",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
