using Microsoft.AspNetCore.Mvc;
using QLPhongGym.Data;
using QLPhongGym.Models;
using System.Linq;

namespace QLPhongGym.Controllers
{
    public class HuanLuyenVienController : BaseController
    {
        private readonly GymDbContext _context;

        public HuanLuyenVienController(GymDbContext context)
        {
            _context = context;
        }

        // Danh sách + tìm kiếm
        public IActionResult Index(string keyword)
        {
            if (!IsAdmin())
            {
                return RedirectAccessDenied();
            }
            var dsHLV = _context.HuanLuyenViens.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                dsHLV = dsHLV.Where(x =>
                    x.HoTen.Contains(keyword) ||
                    (x.SoDienThoai != null && x.SoDienThoai.Contains(keyword)) ||
                    (x.ChuyenMon != null && x.ChuyenMon.Contains(keyword)));
            }

            ViewBag.Keyword = keyword;
            return View(dsHLV.ToList());
        }

        // Chi tiết
        public IActionResult Details(int id)
        {
            if (!IsAdmin())
            {
                return RedirectAccessDenied();
            }
            var hlv = _context.HuanLuyenViens.FirstOrDefault(x => x.MaHLV == id);
            if (hlv == null)
            {
                return NotFound();
            }

            return View(hlv);
        }

        // Thêm
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HuanLuyenVien hlv)
        {
            if (!IsAdmin())
            {
                return RedirectAccessDenied();
            }
            if (ModelState.IsValid)
            {
                _context.HuanLuyenViens.Add(hlv);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hlv);
        }

        // Sửa
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectAccessDenied();
            }
            var hlv = _context.HuanLuyenViens.FirstOrDefault(x => x.MaHLV == id);
            if (hlv == null)
            {
                return NotFound();
            }

            return View(hlv);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HuanLuyenVien hlv)
        {
            if (ModelState.IsValid)
            {
                _context.HuanLuyenViens.Update(hlv);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hlv);
        }

        // Xóa
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectAccessDenied();
            }
            var hlv = _context.HuanLuyenViens.FirstOrDefault(x => x.MaHLV == id);
            if (hlv == null)
            {
                return NotFound();
            }

            return View(hlv);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var hlv = _context.HuanLuyenViens.FirstOrDefault(x => x.MaHLV == id);
            if (hlv == null)
            {
                return NotFound();
            }

            _context.HuanLuyenViens.Remove(hlv);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
