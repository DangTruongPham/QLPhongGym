using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLPhongGym.Data;
using QLPhongGym.Models;
using System;
using System.Linq;

namespace QLPhongGym.Controllers
{
    public class DangKyGoiTapController : BaseController
    {
        private readonly GymDbContext _context;

        public DangKyGoiTapController(GymDbContext context)
        {
            _context = context;
        }

        // Danh sách
        public IActionResult Index(string keyword)
        {
            var dsDangKy = _context.DangKyGoiTaps
                .Include(x => x.HoiVien)
                .Include(x => x.GoiTap)
                .Include(x => x.HuanLuyenVien)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                dsDangKy = dsDangKy.Where(x =>
                    x.HoiVien != null && x.HoiVien.HoTen.Contains(keyword));
            }

            ViewBag.Keyword = keyword;
            return View(dsDangKy.ToList());
        }

        // Chi tiết
        public IActionResult Details(int id)
        {
            var dangKy = _context.DangKyGoiTaps
                .Include(x => x.HoiVien)
                .Include(x => x.GoiTap)
                .Include(x => x.HuanLuyenVien)
                .FirstOrDefault(x => x.MaDangKy == id);

            if (dangKy == null)
            {
                return NotFound();
            }

            return View(dangKy);
        }

        // Thêm
        [HttpGet]
        public IActionResult Create()
        {
            LoadDataDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DangKyGoiTap dangKy)
        {
            if (ModelState.IsValid)
            {
                var goiTap = _context.GoiTaps.FirstOrDefault(x => x.MaGoiTap == dangKy.MaGoiTap);
                if (goiTap == null)
                {
                    ModelState.AddModelError("", "Gói tập không tồn tại");
                    LoadDataDropDown();
                    return View(dangKy);
                }

                dangKy.NgayKetThuc = dangKy.NgayBatDau.AddMonths(goiTap.ThoiHanThang);

                if (dangKy.NgayKetThuc.Date < DateTime.Today)
                {
                    dangKy.TrangThai = "Hết hạn";
                }
                else
                {
                    dangKy.TrangThai = "Còn hạn";
                }

                _context.DangKyGoiTaps.Add(dangKy);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadDataDropDown();
            return View(dangKy);
        }

        // Sửa
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var dangKy = _context.DangKyGoiTaps.FirstOrDefault(x => x.MaDangKy == id);
            if (dangKy == null)
            {
                return NotFound();
            }

            LoadDataDropDown();
            return View(dangKy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(DangKyGoiTap dangKy)
        {
            if (ModelState.IsValid)
            {
                var goiTap = _context.GoiTaps.FirstOrDefault(x => x.MaGoiTap == dangKy.MaGoiTap);
                if (goiTap == null)
                {
                    ModelState.AddModelError("", "Gói tập không tồn tại");
                    LoadDataDropDown();
                    return View(dangKy);
                }

                dangKy.NgayKetThuc = dangKy.NgayBatDau.AddMonths(goiTap.ThoiHanThang);

                if (dangKy.NgayKetThuc.Date < DateTime.Today)
                {
                    dangKy.TrangThai = "Hết hạn";
                }
                else
                {
                    dangKy.TrangThai = "Còn hạn";
                }

                _context.DangKyGoiTaps.Update(dangKy);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            LoadDataDropDown();
            return View(dangKy);
        }

        // Xóa
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var dangKy = _context.DangKyGoiTaps
                .Include(x => x.HoiVien)
                .Include(x => x.GoiTap)
                .Include(x => x.HuanLuyenVien)
                .FirstOrDefault(x => x.MaDangKy == id);

            if (dangKy == null)
            {
                return NotFound();
            }

            return View(dangKy);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var dangKy = _context.DangKyGoiTaps.FirstOrDefault(x => x.MaDangKy == id);
            if (dangKy == null)
            {
                return NotFound();
            }

            _context.DangKyGoiTaps.Remove(dangKy);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private void LoadDataDropDown()
        {
            ViewBag.MaHoiVien = new SelectList(_context.HoiViens.ToList(), "MaHoiVien", "HoTen");
            ViewBag.MaGoiTap = new SelectList(_context.GoiTaps.ToList(), "MaGoiTap", "TenGoiTap");
            ViewBag.MaHLV = new SelectList(_context.HuanLuyenViens.ToList(), "MaHLV", "HoTen");
        }
    }
}