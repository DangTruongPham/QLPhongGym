using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLPhongGym.Data;
using QLPhongGym.Models;
using System;
using System.Linq;

namespace QLPhongGym.Controllers
{
    public class CheckInController : BaseController
    {
        private readonly GymDbContext _context;

        public CheckInController(GymDbContext context)
        {
            _context = context;
        }

        // Danh sách check-in
        public IActionResult Index(string keyword)
        {
            var dsCheckIn = _context.CheckIns
                .Include(x => x.HoiVien)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                dsCheckIn = dsCheckIn.Where(x =>
                    x.HoiVien != null &&
                    (x.HoiVien.HoTen.Contains(keyword) ||
                     (x.HoiVien.SoDienThoai != null && x.HoiVien.SoDienThoai.Contains(keyword))));
            }

            ViewBag.Keyword = keyword;
            return View(dsCheckIn.OrderByDescending(x => x.ThoiGianCheckIn).ToList());
        }

        // Chi tiết
        public IActionResult Details(int id)
        {
            var checkIn = _context.CheckIns
                .Include(x => x.HoiVien)
                .FirstOrDefault(x => x.MaCheckIn == id);

            if (checkIn == null)
            {
                return NotFound();
            }

            return View(checkIn);
        }

        // Thêm
        [HttpGet]
        public IActionResult Create()
        {
            LoadHoiVienDropDown();
            return View(new CheckIn
            {
                ThoiGianCheckIn = DateTime.Now
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CheckIn checkIn)
        {
            if (ModelState.IsValid)
            {
                var hoiVien = _context.HoiViens.FirstOrDefault(x => x.MaHoiVien == checkIn.MaHoiVien);
                if (hoiVien == null)
                {
                    ModelState.AddModelError("", "Hội viên không tồn tại");
                    LoadHoiVienDropDown();
                    return View(checkIn);
                }

                var dangKyHopLe = _context.DangKyGoiTaps
                    .Where(x => x.MaHoiVien == checkIn.MaHoiVien
                                && x.NgayBatDau <= checkIn.ThoiGianCheckIn.Date
                                && x.NgayKetThuc >= checkIn.ThoiGianCheckIn.Date
                                && x.TrangThai == "Còn hạn")
                    .OrderByDescending(x => x.NgayKetThuc)
                    .FirstOrDefault();

                if (dangKyHopLe == null)
                {
                    ModelState.AddModelError("", "Hội viên không có gói tập còn hạn để check-in");
                    LoadHoiVienDropDown();
                    return View(checkIn);
                }

                _context.CheckIns.Add(checkIn);
                _context.SaveChanges();
                TempData["Success"] = "Check-in thành công";
                return RedirectToAction("Index");
            }

            LoadHoiVienDropDown();
            return View(checkIn);
        }

        // Xóa
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var checkIn = _context.CheckIns
                .Include(x => x.HoiVien)
                .FirstOrDefault(x => x.MaCheckIn == id);

            if (checkIn == null)
            {
                return NotFound();
            }

            return View(checkIn);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var checkIn = _context.CheckIns.FirstOrDefault(x => x.MaCheckIn == id);
            if (checkIn == null)
            {
                return NotFound();
            }

            _context.CheckIns.Remove(checkIn);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private void LoadHoiVienDropDown()
        {
            var dsHoiVien = _context.HoiViens
                .ToList()
                .Select(x => new
                {
                    x.MaHoiVien,
                    TenHienThi = x.HoTen + " - " + (x.SoDienThoai ?? "")
                });

            ViewBag.MaHoiVien = new SelectList(dsHoiVien, "MaHoiVien", "TenHienThi");
        }
    }
}