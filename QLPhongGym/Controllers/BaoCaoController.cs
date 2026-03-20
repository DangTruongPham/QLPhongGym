using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLPhongGym.Data;
using System;
using System.Linq;

namespace QLPhongGym.Controllers
{
    public class BaoCaoController : BaseController
    {
        private readonly GymDbContext _context;

        public BaoCaoController(GymDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!IsAdmin())
            {
                return RedirectAccessDenied();
            }
            ViewBag.TongHoiVien = _context.HoiViens.Count();
            ViewBag.HoiVienConHan = _context.DangKyGoiTaps.Count(x => x.NgayKetThuc >= DateTime.Today);
            ViewBag.HoiVienHetHan = _context.DangKyGoiTaps.Count(x => x.NgayKetThuc < DateTime.Today);

            ViewBag.DoanhThuHomNay = _context.ThanhToans
                .Where(x => x.NgayThanhToan.Date == DateTime.Today)
                .Sum(x => (decimal?)x.SoTien) ?? 0;

            ViewBag.DoanhThuThangNay = _context.ThanhToans
                .Where(x => x.NgayThanhToan.Month == DateTime.Today.Month &&
                            x.NgayThanhToan.Year == DateTime.Today.Year)
                .Sum(x => (decimal?)x.SoTien) ?? 0;

            var sapHetHan = _context.DangKyGoiTaps
                .Include(x => x.HoiVien)
                .Include(x => x.GoiTap)
                .Where(x => x.NgayKetThuc >= DateTime.Today && x.NgayKetThuc <= DateTime.Today.AddDays(7))
                .OrderBy(x => x.NgayKetThuc)
                .ToList();

            return View(sapHetHan);
        }
    }
}