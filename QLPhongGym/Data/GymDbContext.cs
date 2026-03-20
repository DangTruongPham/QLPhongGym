using Microsoft.EntityFrameworkCore;
using QLPhongGym.Models;

namespace QLPhongGym.Data
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<HoiVien> HoiViens { get; set; }
        public DbSet<GoiTap> GoiTaps { get; set; }
        public DbSet<HuanLuyenVien> HuanLuyenViens { get; set; }
        public DbSet<DangKyGoiTap> DangKyGoiTaps { get; set; }
        public DbSet<ThanhToan> ThanhToans { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }
    }
}