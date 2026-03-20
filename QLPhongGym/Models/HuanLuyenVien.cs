using System;
using System.ComponentModel.DataAnnotations;

namespace QLPhongGym.Models
{
    public class HuanLuyenVien
    {
        [Key]
        [Display(Name = "Mã huấn luyện viên")]
        public int MaHLV { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100)]
        [Display(Name = "Họ tên")]
        public string HoTen { get; set; } = string.Empty;

        [StringLength(10)]
        [Display(Name = "Giới tính")]
        public string? GioiTinh { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime? NgaySinh { get; set; }

        [StringLength(15)]
        [Display(Name = "Số điện thoại")]
        public string? SoDienThoai { get; set; }

        [StringLength(100)]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [StringLength(100)]
        [Display(Name = "Chuyên môn")]
        public string? ChuyenMon { get; set; }

        [StringLength(50)]
        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "Đang làm";
    }
}
