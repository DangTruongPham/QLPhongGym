using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLPhongGym.Data;
using QLPhongGym.Models;
using QLPhongGym.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GymDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<IPasswordHasher<TaiKhoan>, PasswordHasher<TaiKhoan>>();

var app = builder.Build();

// HASH MẬT KHẨU CŨ - CHỈ CHẠY 1 LẦN
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GymDbContext>();
    var hasher = new PasswordHasher<TaiKhoan>();

    var admin = db.TaiKhoans.FirstOrDefault(x => x.TenDangNhap == "admin");
    if (admin != null && !admin.MatKhau.StartsWith("AQAAAA"))
    {
        admin.MatKhau = hasher.HashPassword(admin, "123456");
    }

    var nv = db.TaiKhoans.FirstOrDefault(x => x.TenDangNhap == "nhanvien");
    if (nv != null && !nv.MatKhau.StartsWith("AQAAAA"))
    {
        nv.MatKhau = hasher.HashPassword(nv, "123456");
    }

    db.SaveChanges();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();