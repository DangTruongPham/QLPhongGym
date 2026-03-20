using System.ComponentModel.DataAnnotations;

namespace QLPhongGym.Models
{
    public class GoiTap
    {
        [Key]
        [Display(Name = "Mã gói tập")]
        public int MaGoiTap { get; set; }

        [Required(ErrorMessage = "Tên gói tập không được để trống")]
        [StringLength(100)]
        [Display(Name = "Tên gói tập")]
        public string TenGoiTap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Thời hạn không được để trống")]
        [Display(Name = "Thời hạn (tháng)")]
        public int ThoiHanThang { get; set; }

        [Required(ErrorMessage = "Giá tiền không được để trống")]
        [Display(Name = "Giá tiền")]
        public decimal GiaTien { get; set; }

        [StringLength(255)]
        [Display(Name = "Mô tả")]
        public string? MoTa { get; set; }
    }
}