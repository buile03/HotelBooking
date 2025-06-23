using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DPKS.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedPhongAndTrackingFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Phong",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Tracking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ErorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsError = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tracking_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tracking_UserId",
                table: "Tracking",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tracking");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Phong");
        }
    }
}
