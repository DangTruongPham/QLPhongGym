using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QLPhongGym.Data;
using QLPhongGym.Models;
using QLPhongGym.Services;
using System;
using System.Linq;

namespace QLPhongGym.Controllers
{
    public class AuthController : Controller
    {
        private readonly GymDbContext _context;
        private readonly EmailService _emailService;
        private readonly IPasswordHasher<TaiKhoan> _passwordHasher;

        public AuthController(
            GymDbContext context,
            EmailService emailService,
            IPasswordHasher<TaiKhoan> passwordHasher)
        {
            _context = context;
            _emailService = emailService;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("TenDangNhap") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var taiKhoan = _context.TaiKhoans.FirstOrDefault(x =>
                x.TenDangNhap == model.TenDangNhap && x.TrangThai);

            if (taiKhoan == null)
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View(model);
            }

            var verifyResult = _passwordHasher.VerifyHashedPassword(
                taiKhoan,
                taiKhoan.MatKhau,
                model.MatKhau);

            if (verifyResult == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View(model);
            }

            HttpContext.Session.SetString("MaTaiKhoan", taiKhoan.MaTaiKhoan.ToString());
            HttpContext.Session.SetString("TenDangNhap", taiKhoan.TenDangNhap);
            HttpContext.Session.SetString("HoTen", taiKhoan.HoTen ?? "");
            HttpContext.Session.SetString("VaiTro", taiKhoan.VaiTro ?? "");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var taiKhoan = _context.TaiKhoans.FirstOrDefault(x => x.Email == model.Email && x.TrangThai);

            if (taiKhoan == null)
            {
                ViewBag.Error = "Email không tồn tại trong hệ thống";
                return View(model);
            }

            if (taiKhoan.OtpCanResendAt.HasValue && taiKhoan.OtpCanResendAt.Value > DateTime.UtcNow)
            {
                TempData["Error"] = "Vui lòng chờ trước khi gửi lại OTP";
                return RedirectToAction("VerifyOtp", new { email = model.Email });
            }

            var otp = GenerateOtp();

            taiKhoan.OtpCode = otp;
            taiKhoan.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
            taiKhoan.OtpCanResendAt = DateTime.UtcNow.AddSeconds(60);
            taiKhoan.OtpVerified = false;

            _context.SaveChanges();

            var subject = "Mã OTP khôi phục mật khẩu";
            var body = $@"
                <h3>Xin chào {taiKhoan.HoTen},</h3>
                <p>Mã OTP đặt lại mật khẩu của bạn là:</p>
                <h2 style='color:#0d6efd'>{otp}</h2>
                <p>Mã có hiệu lực trong 5 phút.</p>";

            _emailService.SendEmail(taiKhoan.Email!, subject, body);

            TempData["Success"] = "OTP đã được gửi về email";
            return RedirectToAction("VerifyOtp", new { email = model.Email });
        }

        [HttpGet]
        public IActionResult VerifyOtp(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ForgotPassword");
            }

            var taiKhoan = _context.TaiKhoans.FirstOrDefault(x => x.Email == email && x.TrangThai);
            if (taiKhoan == null)
            {
                return RedirectToAction("ForgotPassword");
            }

            int remainingSeconds = 0;
            if (taiKhoan.OtpCanResendAt.HasValue && taiKhoan.OtpCanResendAt.Value > DateTime.UtcNow)
            {
                remainingSeconds = (int)(taiKhoan.OtpCanResendAt.Value - DateTime.UtcNow).TotalSeconds;
            }

            ViewBag.RemainingSeconds = remainingSeconds;

            return View(new VerifyOtpViewModel
            {
                Email = email
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyOtp(VerifyOtpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var taiKhoan = _context.TaiKhoans.FirstOrDefault(x => x.Email == model.Email && x.TrangThai);

            if (taiKhoan == null)
            {
                ViewBag.Error = "Tài khoản không tồn tại";
                return View(model);
            }

            if (taiKhoan.OtpCode != model.OtpCode || !taiKhoan.OtpExpiry.HasValue || taiKhoan.OtpExpiry.Value < DateTime.UtcNow)
            {
                ViewBag.Error = "OTP không đúng hoặc đã hết hạn";
                ViewBag.RemainingSeconds = taiKhoan.OtpCanResendAt.HasValue && taiKhoan.OtpCanResendAt > DateTime.UtcNow
                    ? (int)(taiKhoan.OtpCanResendAt.Value - DateTime.UtcNow).TotalSeconds
                    : 0;
                return View(model);
            }

            taiKhoan.OtpVerified = true;
            _context.SaveChanges();

            HttpContext.Session.SetString("ResetPasswordEmail", model.Email);

            return RedirectToAction("ResetPassword");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResendOtp(string email)
        {
            var taiKhoan = _context.TaiKhoans.FirstOrDefault(x => x.Email == email && x.TrangThai);

            if (taiKhoan == null)
            {
                TempData["Error"] = "Email không tồn tại";
                return RedirectToAction("ForgotPassword");
            }

            if (taiKhoan.OtpCanResendAt.HasValue && taiKhoan.OtpCanResendAt.Value > DateTime.UtcNow)
            {
                TempData["Error"] = "Chưa đến thời gian gửi lại OTP";
                return RedirectToAction("VerifyOtp", new { email });
            }

            var otp = GenerateOtp();

            taiKhoan.OtpCode = otp;
            taiKhoan.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
            taiKhoan.OtpCanResendAt = DateTime.UtcNow.AddSeconds(60);
            taiKhoan.OtpVerified = false;

            _context.SaveChanges();

            _emailService.SendEmail(
                taiKhoan.Email!,
                "Mã OTP mới khôi phục mật khẩu",
                $"<p>Mã OTP mới của bạn là: <strong>{otp}</strong></p><p>Mã có hiệu lực trong 5 phút.</p>");

            TempData["Success"] = "Đã gửi lại OTP";
            return RedirectToAction("VerifyOtp", new { email });
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            var email = HttpContext.Session.GetString("ResetPasswordEmail");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("ForgotPassword");
            }

            return View(new ResetPasswordViewModel
            {
                Email = email
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var emailSession = HttpContext.Session.GetString("ResetPasswordEmail");

            if (string.IsNullOrEmpty(emailSession) || emailSession != model.Email)
            {
                return RedirectToAction("ForgotPassword");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var taiKhoan = _context.TaiKhoans.FirstOrDefault(x => x.Email == model.Email && x.TrangThai);

            if (taiKhoan == null || !taiKhoan.OtpVerified)
            {
                return RedirectToAction("ForgotPassword");
            }

            taiKhoan.MatKhau = _passwordHasher.HashPassword(taiKhoan, model.NewPassword);
            taiKhoan.OtpCode = null;
            taiKhoan.OtpExpiry = null;
            taiKhoan.OtpCanResendAt = null;
            taiKhoan.OtpVerified = false;

            _context.SaveChanges();

            HttpContext.Session.Remove("ResetPasswordEmail");
            TempData["Success"] = "Đổi mật khẩu thành công. Vui lòng đăng nhập lại.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private string GenerateOtp()
        {
            return new Random().Next(100000, 999999).ToString();
        }
    }
}
