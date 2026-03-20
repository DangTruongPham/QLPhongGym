using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLPhongGym.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GoiTaps",
                columns: table => new
                {
                    MaGoiTap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenGoiTap = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ThoiHanThang = table.Column<int>(type: "int", nullable: false),
                    GiaTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoiTaps", x => x.MaGoiTap);
                });

            migrationBuilder.CreateTable(
                name: "HoiViens",
                columns: table => new
                {
                    MaHoiVien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NgayDangKy = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoiViens", x => x.MaHoiVien);
                });

            migrationBuilder.CreateTable(
                name: "HuanLuyenViens",
                columns: table => new
                {
                    MaHLV = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GioiTinh = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ChuyenMon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HuanLuyenViens", x => x.MaHLV);
                });

            migrationBuilder.CreateTable(
                name: "TaiKhoans",
                columns: table => new
                {
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VaiTro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    OtpCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    OtpExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OtpCanResendAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OtpVerified = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoans", x => x.MaTaiKhoan);
                });

            migrationBuilder.CreateTable(
                name: "CheckIns",
                columns: table => new
                {
                    MaCheckIn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoiVien = table.Column<int>(type: "int", nullable: false),
                    ThoiGianCheckIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckIns", x => x.MaCheckIn);
                    table.ForeignKey(
                        name: "FK_CheckIns_HoiViens_MaHoiVien",
                        column: x => x.MaHoiVien,
                        principalTable: "HoiViens",
                        principalColumn: "MaHoiVien",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DangKyGoiTaps",
                columns: table => new
                {
                    MaDangKy = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaHoiVien = table.Column<int>(type: "int", nullable: false),
                    MaGoiTap = table.Column<int>(type: "int", nullable: false),
                    MaHLV = table.Column<int>(type: "int", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DangKyGoiTaps", x => x.MaDangKy);
                    table.ForeignKey(
                        name: "FK_DangKyGoiTaps_GoiTaps_MaGoiTap",
                        column: x => x.MaGoiTap,
                        principalTable: "GoiTaps",
                        principalColumn: "MaGoiTap",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DangKyGoiTaps_HoiViens_MaHoiVien",
                        column: x => x.MaHoiVien,
                        principalTable: "HoiViens",
                        principalColumn: "MaHoiVien",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DangKyGoiTaps_HuanLuyenViens_MaHLV",
                        column: x => x.MaHLV,
                        principalTable: "HuanLuyenViens",
                        principalColumn: "MaHLV");
                });

            migrationBuilder.CreateTable(
                name: "ThanhToans",
                columns: table => new
                {
                    MaThanhToan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDangKy = table.Column<int>(type: "int", nullable: false),
                    NgayThanhToan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PhuongThuc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GhiChu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrangThaiThanhToan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThanhToans", x => x.MaThanhToan);
                    table.ForeignKey(
                        name: "FK_ThanhToans_DangKyGoiTaps_MaDangKy",
                        column: x => x.MaDangKy,
                        principalTable: "DangKyGoiTaps",
                        principalColumn: "MaDangKy",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckIns_MaHoiVien",
                table: "CheckIns",
                column: "MaHoiVien");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyGoiTaps_MaGoiTap",
                table: "DangKyGoiTaps",
                column: "MaGoiTap");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyGoiTaps_MaHLV",
                table: "DangKyGoiTaps",
                column: "MaHLV");

            migrationBuilder.CreateIndex(
                name: "IX_DangKyGoiTaps_MaHoiVien",
                table: "DangKyGoiTaps",
                column: "MaHoiVien");

            migrationBuilder.CreateIndex(
                name: "IX_ThanhToans_MaDangKy",
                table: "ThanhToans",
                column: "MaDangKy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckIns");

            migrationBuilder.DropTable(
                name: "TaiKhoans");

            migrationBuilder.DropTable(
                name: "ThanhToans");

            migrationBuilder.DropTable(
                name: "DangKyGoiTaps");

            migrationBuilder.DropTable(
                name: "GoiTaps");

            migrationBuilder.DropTable(
                name: "HoiViens");

            migrationBuilder.DropTable(
                name: "HuanLuyenViens");
        }
    }
}
