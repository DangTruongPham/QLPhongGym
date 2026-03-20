using System.ComponentModel.DataAnnotations;

namespace QLPhongGym.Models
{
    public class VerifyOtpViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mã OTP")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP phải gồm 6 số")]
        public string OtpCode { get; set; } = string.Empty;
    }
}
