using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLPhongGym.Models
{
    public class ThanhToan
    {
        [Key]
        [Display(Name = "Mã thanh toán")]
        public int MaThanhToan { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn đăng ký gói tập")]
        [Display(Name = "Đăng ký gói tập")]
        public int MaDangKy { get; set; }

        [Display(Name = "Ngày thanh toán")]
        public DateTime NgayThanhToan { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Số tiền không được để trống")]
        [Display(Name = "Số tiền")]
        public decimal SoTien { get; set; }

        [StringLength(50)]
        [Display(Name = "Phương thức")]
        public string? PhuongThuc { get; set; }

        [StringLength(255)]
        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        [StringLength(50)]
        [Display(Name = "Trạng thái thanh toán")]
        public string TrangThaiThanhToan { get; set; } = "Chưa thanh toán";

        [ForeignKey("MaDangKy")]
        public DangKyGoiTap? DangKyGoiTap { get; set; }

        [NotMapped]
        public string? VietQrUrl { get; set; }
    }
}
