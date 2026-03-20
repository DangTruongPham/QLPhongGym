using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLPhongGym.Models
{
    public class CheckIn
    {
        [Key]
        [Display(Name = "Mã check-in")]
        public int MaCheckIn { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn hội viên")]
        [Display(Name = "Hội viên")]
        public int MaHoiVien { get; set; }

        [Display(Name = "Thời gian check-in")]
        public DateTime ThoiGianCheckIn { get; set; } = DateTime.UtcNow;

        [StringLength(255)]
        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        [ForeignKey("MaHoiVien")]
        public HoiVien? HoiVien { get; set; }
    }
}
