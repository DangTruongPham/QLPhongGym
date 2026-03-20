using System.Collections.Generic;

namespace QLPhongGym.Models
{
    public class DashboardViewModel
    {
        public int TongHoiVien { get; set; }
        public int TongGoiTap { get; set; }
        public int TongHuanLuyenVien { get; set; }
        public int TongDangKy { get; set; }
        public int TongCheckInHomNay { get; set; }
        public decimal DoanhThuThangNay { get; set; }

        public int HoiVienConHan { get; set; }
        public int HoiVienHetHan { get; set; }

        public List<string> RevenueLabels { get; set; } = new();
        public List<decimal> RevenueData { get; set; } = new();

        public List<string> CheckInLabels { get; set; } = new();
        public List<int> CheckInData { get; set; } = new();

        public List<DangKyGoiTap> DangKySapHetHan { get; set; } = new();
        public List<CheckIn> CheckInGanDay { get; set; } = new();
    }
}
