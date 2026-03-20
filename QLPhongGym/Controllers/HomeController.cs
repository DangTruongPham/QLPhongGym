using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLPhongGym.Data;
using QLPhongGym.Models;

namespace QLPhongGym.Controllers
{
    public class HomeController : BaseController
    {
        private readonly GymDbContext _context;

        public HomeController(GymDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var firstDayOfMonth = new DateTime(today.Year, today.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);

            var model = new DashboardViewModel
            {
                TongHoiVien = _context.HoiViens.Count(),
                TongGoiTap = _context.GoiTaps.Count(),
                TongHuanLuyenVien = _context.HuanLuyenViens.Count(),
                TongDangKy = _context.DangKyGoiTaps.Count(),

                TongCheckInHomNay = _context.CheckIns.Count(x =>
                    x.ThoiGianCheckIn >= today && x.ThoiGianCheckIn < tomorrow),

                DoanhThuThangNay = _context.ThanhToans
                    .Where(x => x.NgayThanhToan >= firstDayOfMonth && x.NgayThanhToan < firstDayOfNextMonth)
                    .Sum(x => (decimal?)x.SoTien) ?? 0,

                HoiVienConHan = _context.DangKyGoiTaps.Count(x => x.NgayKetThuc >= today),
                HoiVienHetHan = _context.DangKyGoiTaps.Count(x => x.NgayKetThuc < today),

                DangKySapHetHan = _context.DangKyGoiTaps
                    .Include(x => x.HoiVien)
                    .Include(x => x.GoiTap)
                    .Where(x => x.NgayKetThuc >= today && x.NgayKetThuc <= today.AddDays(7))
                    .OrderBy(x => x.NgayKetThuc)
                    .Take(5)
                    .ToList(),

                CheckInGanDay = _context.CheckIns
                    .Include(x => x.HoiVien)
                    .OrderByDescending(x => x.ThoiGianCheckIn)
                    .Take(5)
                    .ToList()
            };

            for (int i = 5; i >= 0; i--)
            {
                var monthDate = today.AddMonths(-i);
                var start = new DateTime(monthDate.Year, monthDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                var end = start.AddMonths(1);

                model.RevenueLabels.Add($"Tháng {monthDate.Month}");
                model.RevenueData.Add(
                    _context.ThanhToans
                        .Where(x => x.NgayThanhToan >= start && x.NgayThanhToan < end)
                        .Sum(x => (decimal?)x.SoTien) ?? 0
                );
            }

            for (int i = 6; i >= 0; i--)
            {
                var date = today.AddDays(-i);
                var nextDate = date.AddDays(1);

                model.CheckInLabels.Add(date.ToString("dd/MM"));
                model.CheckInData.Add(
                    _context.CheckIns.Count(x => x.ThoiGianCheckIn >= date && x.ThoiGianCheckIn < nextDate)
                );
            }

            return View(model);
        }
    }
}