using Microsoft.AspNetCore.Mvc;
using QLPhongGym.Data;
using QLPhongGym.Models;
using System.Linq;

namespace QLPhongGym.Controllers
{
    public class HoiVienController : BaseController
    {
        private readonly GymDbContext _context;

        public HoiVienController(GymDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string keyword)
        {
            var dsHoiVien = _context.HoiViens.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                dsHoiVien = dsHoiVien.Where(x =>
                    x.HoTen.Contains(keyword) ||
                    (x.SoDienThoai != null && x.SoDienThoai.Contains(keyword)));
            }

            ViewBag.Keyword = keyword;
            return View(dsHoiVien.ToList());
        }

        public IActionResult Details(int id)
        {
            var hoiVien = _context.HoiViens.FirstOrDefault(x => x.MaHoiVien == id);
            if (hoiVien == null)
            {
                return NotFound();
            }

            return View(hoiVien);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HoiVien hoiVien)
        {
            if (ModelState.IsValid)
            {
                _context.HoiViens.Add(hoiVien);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hoiVien);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var hoiVien = _context.HoiViens.FirstOrDefault(x => x.MaHoiVien == id);
            if (hoiVien == null)
            {
                return NotFound();
            }

            return View(hoiVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HoiVien hoiVien)
        {
            if (ModelState.IsValid)
            {
                _context.HoiViens.Update(hoiVien);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hoiVien);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var hoiVien = _context.HoiViens.FirstOrDefault(x => x.MaHoiVien == id);
            if (hoiVien == null)
            {
                return NotFound();
            }

            return View(hoiVien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var hoiVien = _context.HoiViens.FirstOrDefault(x => x.MaHoiVien == id);
            if (hoiVien == null)
            {
                return NotFound();
            }

            _context.HoiViens.Remove(hoiVien);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}