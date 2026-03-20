using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLPhongGym.Models
{
    public class DangKyGoiTap
    {
        [Key]
        [Display(Name = "Mã đăng ký")]
        public int MaDangKy { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hội viên")]
        [Display(Name = "Hội viên")]
        public int MaHoiVien { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn gói tập")]
        [Display(Name = "Gói tập")]
        public int MaGoiTap { get; set; }

        [Display(Name = "Huấn luyện viên")]
        public int? MaHLV { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu")]
        [Display(Name = "Ngày bắt đầu")]
        public DateTime NgayBatDau { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public DateTime NgayKetThuc { get; set; }

        [StringLength(50)]
        [Display(Name = "Trạng thái")]
        public string TrangThai { get; set; } = "Còn hạn";

        [ForeignKey("MaHoiVien")]
        public HoiVien? HoiVien { get; set; }

        [ForeignKey("MaGoiTap")]
        public GoiTap? GoiTap { get; set; }

        [ForeignKey("MaHLV")]
        public HuanLuyenVien? HuanLuyenVien { get; set; }
    }
}