using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoaiPhong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiPhong", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhuongThucThanhToan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loaiThanhToan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucThanhToan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuocGia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaQG = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuocGia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descritption = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TienNghi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TienNghi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrangThaiDatPhong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    trangThaiDatPhong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrangThaiDatPhong", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrangThaiPhong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    trangThaiPhong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrangThaiPhong", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tinh",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuocGiaId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tinh", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tinh_QuocGia_QuocGiaId",
                        column: x => x.QuocGiaId,
                        principalTable: "QuocGia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tienNghiTheoLoaiPhongs",
                columns: table => new
                {
                    LoaiPhongId = table.Column<int>(type: "int", nullable: false),
                    TienNghiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tienNghiTheoLoaiPhongs", x => new { x.LoaiPhongId, x.TienNghiId });
                    table.ForeignKey(
                        name: "FK_tienNghiTheoLoaiPhongs_LoaiPhong_LoaiPhongId",
                        column: x => x.LoaiPhongId,
                        principalTable: "LoaiPhong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tienNghiTheoLoaiPhongs_TienNghi_TienNghiId",
                        column: x => x.TienNghiId,
                        principalTable: "TienNghi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Phong",
                columns: table => new
                {
                    PhongId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SoPhong = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LoaiPhongId = table.Column<int>(type: "int", nullable: false),
                    TrangThaiPhongId = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    loaiGiuong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    loaiView = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phong", x => x.PhongId);
                    table.ForeignKey(
                        name: "FK_Phong_LoaiPhong_LoaiPhongId",
                        column: x => x.LoaiPhongId,
                        principalTable: "LoaiPhong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Phong_TrangThaiPhong_TrangThaiPhongId",
                        column: x => x.TrangThaiPhongId,
                        principalTable: "TrangThaiPhong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    QuocGiaId = table.Column<int>(type: "int", nullable: false),
                    TinhId = table.Column<int>(type: "int", nullable: false),
                    EmailConfirmationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConfirmationCodeExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResetPasswordCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetPasswordCodeExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TwoFactorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwoFactorCodeExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_QuocGia_QuocGiaId",
                        column: x => x.QuocGiaId,
                        principalTable: "QuocGia",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Tinh_TinhId",
                        column: x => x.TinhId,
                        principalTable: "Tinh",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AnhPhong",
                columns: table => new
                {
                    PhotoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhongId = table.Column<int>(type: "int", nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnhPhong", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_AnhPhong_Phong_PhongId",
                        column: x => x.PhongId,
                        principalTable: "Phong",
                        principalColumn: "PhongId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DatPhong",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhongId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayNhanPhong = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayTraPhong = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoLuongKhach = table.Column<int>(type: "int", nullable: false),
                    SoDem = table.Column<int>(type: "int", nullable: false),
                    TrangThaiDatPhongId = table.Column<int>(type: "int", nullable: false),
                    TrangThaiPhongId = table.Column<int>(type: "int", nullable: false),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatPhong", x => x.Id);
                    table.CheckConstraint("CK_Reservation_SoDem", "SoDem >=1 AND SoDem<=31");
                    table.CheckConstraint("CK_Reservation_SoLuongKhach", "SoLuongKhach >=1 AND SoLuongKhach<=4");
                    table.ForeignKey(
                        name: "FK_DatPhong_Phong_PhongId",
                        column: x => x.PhongId,
                        principalTable: "Phong",
                        principalColumn: "PhongId");
                    table.ForeignKey(
                        name: "FK_DatPhong_TrangThaiDatPhong_TrangThaiDatPhongId",
                        column: x => x.TrangThaiDatPhongId,
                        principalTable: "TrangThaiDatPhong",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DatPhong_TrangThaiPhong_TrangThaiPhongId",
                        column: x => x.TrangThaiPhongId,
                        principalTable: "TrangThaiPhong",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DatPhong_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.UserId, x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedBacks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DatPhongId = table.Column<int>(type: "int", nullable: false),
                    DanhGia = table.Column<int>(type: "int", nullable: false),
                    BinhLuan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBacks", x => x.Id);
                    table.CheckConstraint("CK_FeedBacks_DanhGia", "DanhGia >=1 AND DanhGia<=5");
                    table.ForeignKey(
                        name: "FK_FeedBacks_DatPhong_DatPhongId",
                        column: x => x.DatPhongId,
                        principalTable: "DatPhong",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FeedBacks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThanhToan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatPhongId = table.Column<int>(type: "int", nullable: false),
                    PhuongThucThanhToanId = table.Column<int>(type: "int", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ThoiDiemThanhToan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LateModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThanhToan_DatPhong_DatPhongId",
                        column: x => x.DatPhongId,
                        principalTable: "DatPhong",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ThanhToan_PhuongThucThanhToan_PhuongThucThanhToanId",
                        column: x => x.PhuongThucThanhToanId,
                        principalTable: "PhuongThucThanhToan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnhPhong_PhongId",
                table: "AnhPhong",
                column: "PhongId");

            migrationBuilder.CreateIndex(
                name: "IX_AnhPhong_PhotoName",
                table: "AnhPhong",
                column: "PhotoName",
                unique: true,
                filter: "[PhotoName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DatPhong_PhongId",
                table: "DatPhong",
                column: "PhongId");

            migrationBuilder.CreateIndex(
                name: "IX_DatPhong_TrangThaiDatPhongId",
                table: "DatPhong",
                column: "TrangThaiDatPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_DatPhong_TrangThaiPhongId",
                table: "DatPhong",
                column: "TrangThaiPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_DatPhong_UserId",
                table: "DatPhong",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBacks_DatPhongId",
                table: "FeedBacks",
                column: "DatPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBacks_UserId",
                table: "FeedBacks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Phong_LoaiPhongId",
                table: "Phong",
                column: "LoaiPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_Phong_SoPhong",
                table: "Phong",
                column: "SoPhong",
                unique: true,
                filter: "[SoPhong] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Phong_TrangThaiPhongId",
                table: "Phong",
                column: "TrangThaiPhongId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_DatPhongId",
                table: "ThanhToan",
                column: "DatPhongId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToan_PhuongThucThanhToanId",
                table: "ThanhToan",
                column: "PhuongThucThanhToanId");

            migrationBuilder.CreateIndex(
                name: "IX_tienNghiTheoLoaiPhongs_TienNghiId",
                table: "tienNghiTheoLoaiPhongs",
                column: "TienNghiId");

            migrationBuilder.CreateIndex(
                name: "IX_Tinh_QuocGiaId",
                table: "Tinh",
                column: "QuocGiaId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_QuocGiaId",
                table: "Users",
                column: "QuocGiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TinhId",
                table: "Users",
                column: "TinhId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnhPhong");

            migrationBuilder.DropTable(
                name: "FeedBacks");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "ThanhToan");

            migrationBuilder.DropTable(
                name: "tienNghiTheoLoaiPhongs");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "DatPhong");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToan");

            migrationBuilder.DropTable(
                name: "TienNghi");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Phong");

            migrationBuilder.DropTable(
                name: "TrangThaiDatPhong");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "LoaiPhong");

            migrationBuilder.DropTable(
                name: "TrangThaiPhong");

            migrationBuilder.DropTable(
                name: "Tinh");

            migrationBuilder.DropTable(
                name: "QuocGia");
        }
    }
}
