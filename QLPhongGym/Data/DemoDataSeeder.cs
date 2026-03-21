using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLPhongGym.Models;

namespace QLPhongGym.Data
{
    public static class DemoDataSeeder
    {
        public static void Seed(GymDbContext db)
        {
            db.Database.Migrate();

            if (db.GoiTaps.Any() || db.HoiViens.Any() || db.HuanLuyenViens.Any() || db.DangKyGoiTaps.Any())
            {
                return;
            }

            var random = new Random(20260320);

            var goiTaps = new List<GoiTap>
            {
                new() { TenGoiTap = "Gói Cơ Bản 1 Tháng", ThoiHanThang = 1, GiaTien = 399000, MoTa = "Phù hợp người mới bắt đầu, tập tự do toàn khung giờ." },
                new() { TenGoiTap = "Gói Tiêu Chuẩn 3 Tháng", ThoiHanThang = 3, GiaTien = 999000, MoTa = "Tiết kiệm chi phí, phù hợp tập ổn định." },
                new() { TenGoiTap = "Gói Nâng Cao 6 Tháng", ThoiHanThang = 6, GiaTien = 1799000, MoTa = "Dành cho người tập dài hạn, quyền lợi tốt hơn." },
                new() { TenGoiTap = "Gói Premium 12 Tháng", ThoiHanThang = 12, GiaTien = 2999000, MoTa = "Tập không giới hạn, ưu tiên tư vấn dinh dưỡng." },
                new() { TenGoiTap = "Gói Giảm Cân", ThoiHanThang = 3, GiaTien = 1299000, MoTa = "Kết hợp cardio và lịch trình giảm mỡ." },
                new() { TenGoiTap = "Gói Tăng Cơ", ThoiHanThang = 3, GiaTien = 1399000, MoTa = "Phù hợp người muốn tăng cơ, cải thiện thể hình." },
                new() { TenGoiTap = "Gói HLV Cá Nhân", ThoiHanThang = 1, GiaTien = 2499000, MoTa = "Có huấn luyện viên theo sát và giáo án riêng." },
                new() { TenGoiTap = "Gói Sinh Viên", ThoiHanThang = 1, GiaTien = 299000, MoTa = "Ưu đãi dành cho học sinh, sinh viên." }
            };

            db.GoiTaps.AddRange(goiTaps);
            db.SaveChanges();

            var hlvHo = new[] { "Nguyễn", "Trần", "Lê", "Phạm", "Hoàng", "Võ", "Đặng", "Bùi", "Đỗ", "Huỳnh" };
            var hlvTenLot = new[] { "Văn", "Thị", "Minh", "Quốc", "Anh", "Ngọc", "Thanh", "Gia", "Đức", "Hoài" };
            var hlvTen = new[] { "An", "Bình", "Dũng", "Hải", "Huy", "Khánh", "Long", "Nam", "Phúc", "Quân", "Trang", "Vy", "Yến", "Linh" };
            var chuyenMons = new[]
            {
                "Tăng cơ", "Giảm mỡ", "Yoga", "Cardio", "Functional Training",
                "Powerlifting", "Phục hồi vận động", "Dinh dưỡng thể thao"
            };

            var huanLuyenViens = new List<HuanLuyenVien>();
            for (int i = 1; i <= 12; i++)
            {
                var gioiTinh = i % 3 == 0 ? "Nữ" : "Nam";
                var hoTen = $"{hlvHo[random.Next(hlvHo.Length)]} {hlvTenLot[random.Next(hlvTenLot.Length)]} {hlvTen[random.Next(hlvTen.Length)]}";
                var birthYear = random.Next(1988, 2001);

                huanLuyenViens.Add(new HuanLuyenVien
                {
                    HoTen = hoTen,
                    GioiTinh = gioiTinh,
                    NgaySinh = new DateTime(birthYear, random.Next(1, 13), random.Next(1, 28), 0, 0, 0, DateTimeKind.Utc),
                    SoDienThoai = $"09{random.Next(10000000, 99999999)}",
                    Email = $"hlv{i}@qlphonggym.com",
                    ChuyenMon = chuyenMons[random.Next(chuyenMons.Length)],
                    TrangThai = i <= 10 ? "Đang làm việc" : "Tạm nghỉ"
                });
            }

            db.HuanLuyenViens.AddRange(huanLuyenViens);
            db.SaveChanges();

            var ho = new[] { "Nguyễn", "Trần", "Lê", "Phạm", "Hoàng", "Phan", "Vũ", "Võ", "Đặng", "Bùi", "Đỗ", "Hồ", "Ngô", "Dương", "Lý" };
            var tenLot = new[] { "Văn", "Thị", "Hữu", "Ngọc", "Đức", "Minh", "Gia", "Anh", "Quốc", "Thanh", "Khánh", "Hoài" };
            var ten = new[] { "An", "Bảo", "Bình", "Châu", "Duy", "Giang", "Hà", "Hải", "Hiếu", "Hùng", "Huy", "Khang", "Khánh", "Lâm", "Linh", "Long", "Minh", "My", "Nam", "Ngân", "Ngọc", "Như", "Phát", "Phúc", "Quân", "Quỳnh", "Sơn", "Tâm", "Thảo", "Trang", "Trinh", "Trúc", "Tuấn", "Uyên", "Vi", "Vy", "Yến" };
            var diaChiMau = new[]
            {
                "Ninh Kiều, Cần Thơ", "Bình Thủy, Cần Thơ", "Cái Răng, Cần Thơ", "Ô Môn, Cần Thơ",
                "Thốt Nốt, Cần Thơ", "Long Xuyên, An Giang", "Rạch Giá, Kiên Giang",
                "Mỹ Tho, Tiền Giang", "Cao Lãnh, Đồng Tháp", "Vị Thanh, Hậu Giang"
            };
            var trangThaiHoiVien = new[] { "Hoạt động", "Tạm nghỉ", "Mới đăng ký" };

            var hoiViens = new List<HoiVien>();
            for (int i = 1; i <= 150; i++)
            {
                var gioiTinh = i % 4 == 0 ? "Nữ" : "Nam";
                var hoTen = $"{ho[random.Next(ho.Length)]} {tenLot[random.Next(tenLot.Length)]} {ten[random.Next(ten.Length)]}";
                var birthYear = random.Next(1985, 2007);
                var ngayDangKy = DateTime.UtcNow.Date.AddDays(-random.Next(0, 360));

                hoiViens.Add(new HoiVien
                {
                    HoTen = hoTen,
                    GioiTinh = gioiTinh,
                    NgaySinh = new DateTime(birthYear, random.Next(1, 13), random.Next(1, 28), 0, 0, 0, DateTimeKind.Utc),
                    SoDienThoai = $"09{random.Next(10000000, 99999999)}",
                    Email = $"hoivien{i}@mail.com",
                    DiaChi = diaChiMau[random.Next(diaChiMau.Length)],
                    NgayDangKy = ngayDangKy,
                    TrangThai = trangThaiHoiVien[random.Next(trangThaiHoiVien.Length)]
                });
            }

            db.HoiViens.AddRange(hoiViens);
            db.SaveChanges();

            var hasher = new PasswordHasher<TaiKhoan>();
            var taiKhoans = new List<TaiKhoan>();

            void AddAccount(string username, string fullName, string email, string role, bool active = true)
            {
                if (db.TaiKhoans.Any(x => x.TenDangNhap == username)) return;

                var acc = new TaiKhoan
                {
                    TenDangNhap = username,
                    HoTen = fullName,
                    Email = email,
                    VaiTro = role,
                    TrangThai = active,
                    OtpVerified = true,
                    OtpCode = null,
                    OtpExpiry = null,
                    OtpCanResendAt = null
                };
                acc.MatKhau = hasher.HashPassword(acc, "123456");
                taiKhoans.Add(acc);
            }

            AddAccount("admin", "Quản trị hệ thống", "admin@qlphonggym.com", "Admin");
            AddAccount("nhanvien01", "Nhân viên lễ tân 01", "nhanvien01@qlphonggym.com", "NhanVien");
            AddAccount("nhanvien02", "Nhân viên lễ tân 02", "nhanvien02@qlphonggym.com", "NhanVien");

            for (int i = 0; i < 6; i++)
            {
                AddAccount($"hlv{i + 1:00}", huanLuyenViens[i].HoTen, huanLuyenViens[i].Email ?? $"hlv{i+1}@qlphonggym.com", "HuanLuyenVien");
            }

            db.TaiKhoans.AddRange(taiKhoans);
            db.SaveChanges();

            var dangKyGoiTaps = new List<DangKyGoiTap>();
            var trangThaiDangKy = new[] { "Còn hạn", "Sắp hết hạn", "Hết hạn" };

            foreach (var hoiVien in hoiViens)
            {
                int soLanDangKy = random.Next(1, 3);

                DateTime startBase = hoiVien.NgayDangKy;
                for (int i = 0; i < soLanDangKy; i++)
                {
                    var goi = goiTaps[random.Next(goiTaps.Count)];
                    var hlv = random.Next(0, 100) < 35 ? huanLuyenViens[random.Next(huanLuyenViens.Count)] : null;

                    var ngayBatDau = startBase.AddDays(random.Next(0, 20));
                    var ngayKetThuc = ngayBatDau.AddMonths(goi.ThoiHanThang);
                    var status = ngayKetThuc < DateTime.UtcNow.Date
                        ? "Hết hạn"
                        : (ngayKetThuc <= DateTime.UtcNow.Date.AddDays(7) ? "Sắp hết hạn" : "Còn hạn");

                    dangKyGoiTaps.Add(new DangKyGoiTap
                    {
                        MaHoiVien = hoiVien.MaHoiVien,
                        MaGoiTap = goi.MaGoiTap,
                        MaHLV = hlv?.MaHLV,
                        NgayBatDau = DateTime.SpecifyKind(ngayBatDau, DateTimeKind.Utc),
                        NgayKetThuc = DateTime.SpecifyKind(ngayKetThuc, DateTimeKind.Utc),
                        TrangThai = status
                    });

                    startBase = ngayKetThuc.AddDays(random.Next(3, 40));
                }
            }

            db.DangKyGoiTaps.AddRange(dangKyGoiTaps);
            db.SaveChanges();

            var thanhToans = new List<ThanhToan>();
            var phuongThucs = new[] { "Tiền mặt", "Chuyển khoản", "Momo", "ZaloPay" };

            foreach (var dk in db.DangKyGoiTaps.Include(x => x.GoiTap))
            {
                var ngayThanhToan = dk.NgayBatDau.AddDays(-random.Next(0, 3));
                thanhToans.Add(new ThanhToan
                {
                    MaDangKy = dk.MaDangKy,
                    NgayThanhToan = DateTime.SpecifyKind(ngayThanhToan, DateTimeKind.Utc),
                    SoTien = dk.GoiTap?.GiaTien ?? 0,
                    PhuongThuc = phuongThucs[random.Next(phuongThucs.Length)],
                    GhiChu = random.Next(0, 100) < 20 ? "Khách hàng thanh toán ưu đãi" : null,
                    TrangThaiThanhToan = "Đã thanh toán"
                });
            }

            db.ThanhToans.AddRange(thanhToans);
            db.SaveChanges();

            var checkIns = new List<CheckIn>();
            var dangKyConHan = db.DangKyGoiTaps
                .Where(x => x.TrangThai != "Hết hạn")
                .ToList();

            foreach (var dk in dangKyConHan)
            {
                int soLan = random.Next(6, 18);
                for (int i = 0; i < soLan; i++)
                {
                    var dayOffset = random.Next(0, 45);
                    var checkInTime = DateTime.UtcNow.Date.AddDays(-dayOffset)
                        .AddHours(random.Next(5, 22))
                        .AddMinutes(random.Next(0, 60));

                    if (checkInTime >= dk.NgayBatDau && checkInTime <= dk.NgayKetThuc.AddDays(1))
                    {
                        checkIns.Add(new CheckIn
                        {
                            MaHoiVien = dk.MaHoiVien,
                            ThoiGianCheckIn = DateTime.SpecifyKind(checkInTime, DateTimeKind.Utc),
                            GhiChu = random.Next(0, 100) < 10 ? "Check-in buổi sáng" : null
                        });
                    }
                }
            }

            db.CheckIns.AddRange(checkIns);
            db.SaveChanges();
        }
    }
}