using System;
using System.ComponentModel.DataAnnotations;

namespace QLPhongGym.Models
{
    public class TaiKhoan
    {
        [Key]
        public int MaTaiKhoan { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50)]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string MatKhau { get; set; } = string.Empty;

        [StringLength(100)]
        public string? HoTen { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? VaiTro { get; set; }

        public bool TrangThai { get; set; } = true;

        [StringLength(10)]
        public string? OtpCode { get; set; }

        public DateTime? OtpExpiry { get; set; }

        public DateTime? OtpCanResendAt { get; set; }

        public bool OtpVerified { get; set; } = false;
    }
}