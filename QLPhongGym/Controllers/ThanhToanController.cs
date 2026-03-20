using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLPhongGym.Data;
using QLPhongGym.Models;

namespace QLPhongGym.Controllers
{
    public class ThanhToanController : BaseController
    {
        private readonly GymDbContext _context;

        public ThanhToanController(GymDbContext context)
        {
            _context = context;
        }

        // Danh sách
        public IActionResult Index(string keyword)
        {
            var dsThanhToan = _context.ThanhToans
                .Include(x => x.DangKyGoiTap!)
                    .ThenInclude(dk => dk.HoiVien)
                .Include(x => x.DangKyGoiTap!)
                    .ThenInclude(dk => dk.GoiTap)
                .AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                dsThanhToan = dsThanhToan.Where(x =>
                    x.DangKyGoiTap != null &&
                    x.DangKyGoiTap.HoiVien != null &&
                    x.DangKyGoiTap.HoiVien.HoTen.Contains(keyword));
            }

            ViewBag.Keyword = keyword;
            return View(dsThanhToan.OrderByDescending(x => x.NgayThanhToan).ToList());
        }

        [HttpGet]
        public JsonResult GetSoTienByDangKy(int maDangKy)
        {
            var dangKy = _context.DangKyGoiTaps
                .Include(x => x.GoiTap)
                .FirstOrDefault(x => x.MaDangKy == maDangKy);

            if (dangKy == null || dangKy.GoiTap == null)
            {
                return Json(new { success = false, soTien = 0 });
            }

            return Json(new
            {
                success = true,
                soTien = dangKy.GoiTap.GiaTien
            });
        }

        [HttpGet]
        public IActionResult ConfirmPayment(int id)
        {
            var thanhToan = _context.ThanhToans.FirstOrDefault(x => x.MaThanhToan == id);
            if (thanhToan == null)
            {
                return NotFound();
            }

            thanhToan.TrangThaiThanhToan = "Đã thanh toán";
            _context.SaveChanges();

            TempData["Success"] = "Đã xác nhận thanh toán thành công";
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public IActionResult XacNhanThanhToan(int id)
        {
            var thanhToan = _context.ThanhToans.FirstOrDefault(x => x.MaThanhToan == id);

            if (thanhToan == null)
            {
                return NotFound();
            }

            thanhToan.TrangThaiThanhToan = "Đã thanh toán";

            _context.SaveChanges();

            TempData["Success"] = "Đã xác nhận thanh toán";

            return RedirectToAction("Index");
        }

        // Chi tiết
        public IActionResult Details(int id)
        {
            var thanhToan = _context.ThanhToans
                .Include(x => x.DangKyGoiTap!)
                    .ThenInclude(dk => dk.HoiVien)
                .Include(x => x.DangKyGoiTap!)
                    .ThenInclude(dk => dk.GoiTap)
                .FirstOrDefault(x => x.MaThanhToan == id);

            if (thanhToan == null)
            {
                return NotFound();
            }

            if (thanhToan.PhuongThuc == "Chuyển khoản")
            {
                thanhToan.VietQrUrl = BuildVietQrUrl(thanhToan.SoTien, BuildPaymentContent(thanhToan));
            }

            return View(thanhToan);
        }

        // Thêm
        [HttpGet]
        public IActionResult Create()
        {
            LoadDangKyDropDown();

            var model = new ThanhToan
            {
                NgayThanhToan = DateTime.Now,
                PhuongThuc = "Chuyển khoản",
                SoTien = 0
            };

            model.VietQrUrl = BuildVietQrUrl(model.SoTien, "THANH TOAN GYM");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ThanhToan thanhToan)
        {
            if (ModelState.IsValid)
            {
                _context.ThanhToans.Add(thanhToan);
                _context.SaveChanges();

                TempData["Success"] = "Tạo phiếu thanh toán thành công";
                return RedirectToAction("Details", new { id = thanhToan.MaThanhToan });
            }

            LoadDangKyDropDown();

            if (thanhToan.PhuongThuc == "Chuyển khoản")
            {
                thanhToan.VietQrUrl = BuildVietQrUrl(thanhToan.SoTien, BuildPaymentContent(thanhToan));
            }

            return View(thanhToan);
        }

        // Sửa
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var thanhToan = _context.ThanhToans.FirstOrDefault(x => x.MaThanhToan == id);
            if (thanhToan == null)
            {
                return NotFound();
            }

            if (thanhToan.PhuongThuc == "Chuyển khoản")
            {
                thanhToan.VietQrUrl = BuildVietQrUrl(thanhToan.SoTien, BuildPaymentContent(thanhToan));
            }

            LoadDangKyDropDown();
            return View(thanhToan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ThanhToan thanhToan)
        {
            if (ModelState.IsValid)
            {
                _context.ThanhToans.Update(thanhToan);
                _context.SaveChanges();

                TempData["Success"] = "Cập nhật thanh toán thành công";
                return RedirectToAction("Details", new { id = thanhToan.MaThanhToan });
            }

            LoadDangKyDropDown();

            if (thanhToan.PhuongThuc == "Chuyển khoản")
            {
                thanhToan.VietQrUrl = BuildVietQrUrl(thanhToan.SoTien, BuildPaymentContent(thanhToan));
            }

            return View(thanhToan);
        }

        // Xóa
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var thanhToan = _context.ThanhToans
                .Include(x => x.DangKyGoiTap!)
                    .ThenInclude(dk => dk.HoiVien)
                .Include(x => x.DangKyGoiTap!)
                    .ThenInclude(dk => dk.GoiTap)
                .FirstOrDefault(x => x.MaThanhToan == id);

            if (thanhToan == null)
            {
                return NotFound();
            }

            return View(thanhToan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var thanhToan = _context.ThanhToans.FirstOrDefault(x => x.MaThanhToan == id);
            if (thanhToan == null)
            {
                return NotFound();
            }

            _context.ThanhToans.Remove(thanhToan);
            _context.SaveChanges();

            TempData["Success"] = "Xóa thanh toán thành công";
            return RedirectToAction("Index");
        }

        private void LoadDangKyDropDown()
        {
            var dsDangKy = _context.DangKyGoiTaps
                .Include(x => x.HoiVien)
                .Include(x => x.GoiTap)
                .ToList()
                .Select(x => new
                {
                    x.MaDangKy,
                    TenHienThi = (x.HoiVien != null ? x.HoiVien.HoTen : "N/A")
                                 + " - "
                                 + (x.GoiTap != null ? x.GoiTap.TenGoiTap : "N/A")
                                 + " - "
                                 + x.NgayBatDau.ToString("dd/MM/yyyy")
                });

            ViewBag.MaDangKy = new SelectList(dsDangKy, "MaDangKy", "TenHienThi");
        }

        private string BuildVietQrUrl(decimal amount, string? addInfo)
        {
            var bankId = "VCB";
            var accountNo = "1031674306";
            var accountName = Uri.EscapeDataString("PHAN DUY KHANG");
            var template = "compact2";
            var amountStr = Convert.ToInt64(amount).ToString();
            var content = Uri.EscapeDataString(string.IsNullOrWhiteSpace(addInfo) ? "THANH TOAN GYM" : addInfo);

            return $"https://img.vietqr.io/image/{bankId}-{accountNo}-{template}.png?amount={amountStr}&addInfo={content}&accountName={accountName}";
        }

        private string BuildPaymentContent(ThanhToan thanhToan)
        {
            return $"GYM TT {thanhToan.MaDangKy}";
        }
    }
}