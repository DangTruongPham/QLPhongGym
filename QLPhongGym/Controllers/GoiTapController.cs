using Microsoft.AspNetCore.Mvc;
using QLPhongGym.Data;
using QLPhongGym.Models;
using System.Linq;

namespace QLPhongGym.Controllers
{
    public class GoiTapController : BaseController
    {
        private readonly GymDbContext _context;

        public GoiTapController(GymDbContext context)
        {
            _context = context;
        }

        // Danh sách
        public IActionResult Index(string keyword)
        {
            if (!IsAdmin())
            {
                return RedirectAccessDenied();
            }

            var dsGoiTap = _context.GoiTaps.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
            {
                dsGoiTap = dsGoiTap.Where(x => x.TenGoiTap.Contains(keyword));
            }

            ViewBag.Keyword = keyword;
            return View(dsGoiTap.ToList());
        }

        // Chi tiết
        public IActionResult Details(int id)
        {
            if (!IsAdmin())
            {
                return RedirectAccessDenied();
            }
            var goiTap = _context.GoiTaps.FirstOrDefault(x => x.MaGoiTap == id);
            if (goiTap == null)
            {
                return NotFound();
            }

            return View(goiTap);
        }

        // Thêm
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(GoiTap goiTap)
        {
            if (!IsAdmin())
            {
                return RedirectAccessDenied();
            }
            if (ModelState.IsValid)
            {
                _context.GoiTaps.Add(goiTap);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(goiTap);
        }

        // Sửa
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectAccessDenied();
            }
            var goiTap = _context.GoiTaps.FirstOrDefault(x => x.MaGoiTap == id);
            if (goiTap == null)
            {
                return NotFound();
            }

            return View(goiTap);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(GoiTap goiTap)
        {
            if (ModelState.IsValid)
            {
                _context.GoiTaps.Update(goiTap);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(goiTap);
        }

        // Xóa
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectAccessDenied();
            }
            var goiTap = _context.GoiTaps.FirstOrDefault(x => x.MaGoiTap == id);
            if (goiTap == null)
            {
                return NotFound();
            }

            return View(goiTap);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var goiTap = _context.GoiTaps.FirstOrDefault(x => x.MaGoiTap == id);
            if (goiTap == null)
            {
                return NotFound();
            }

            _context.GoiTaps.Remove(goiTap);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
