# Hướng dẫn toàn bộ dự án QLPhongGym

## 1. Tổng quan dự án

QLPhongGym là hệ thống quản lý phòng gym được xây dựng bằng ASP.NET Core MVC với Entity Framework Core. Ứng dụng hỗ trợ:

- Quản lý tài khoản người dùng (admin/nhân viên, đăng nhập, bảo mật, OTP, khôi phục mật khẩu)
- Quản lý hội viên, gói tập, huấn luyện viên
- Quản lý đăng ký gói tập, thanh toán, check-in
- Dashboard tổng hợp doanh thu, check-in, hội viên gần hết hạn
- Giao diện Razor Views, session-based authentication

## 2. Kiến trúc ứng dụng

- **Backend:** ASP.NET Core MVC (Net 8/9)
- **ORM:** Entity Framework Core
- **CSDL:** SQL Server
- **Views:** Razor
- **Session:** Lưu thông tin đăng nhập (MaTaiKhoan, TenDangNhap, HoTen, VaiTro)
- **Dịch vụ:** EmailService gửi OTP

## 3. Cấu trúc chính files

- `Program.cs`: cấu hình services, session, DI, middleware, route
- `Data/GymDbContext.cs`: DbContext với DbSet các bảng
- `Models/`: toàn bộ model (TaiKhoan, HoiVien, GoiTap, DangKyGoiTap, ThanhToan, CheckIn, ...)
- `Controllers/`: AuthController, HomeController, CRUD controllers (HoiVien, GoiTap, DangKyGoiTap, ThanhToan, CheckIn,...)
- `Views/`: giao diện Razor theo từng controller
- `Services/EmailService.cs`: gửi email OTP
- `Migrations/`: migration EF Core

## 4. Luồng hệ thống (Flow)

### 4.1. Luồng đăng nhập / xác thực

1. Người dùng mở trang `Auth/Login`.
2. Nhập `TenDangNhap` và `MatKhau`.
3. `AuthController.Login`:
   - Kiểm tra model validation.
   - Tìm `TaiKhoan` trong DbContext với `TenDangNhap` và `TrangThai=true`.
   - Kiểm tra hash mật khẩu (`IPasswordHasher.VerifyHashedPassword`).
   - Nếu đúng: lưu session `MaTaiKhoan`, `TenDangNhap`, `HoTen`, `VaiTro`.
   - Redirect về `Home/Index`.

4. Nếu quên mật khẩu: `AuthController.ForgotPassword` gửi OTP tới email.
5. Người dùng nhập OTP tại `Auth/VerifyOtp`.
6. Khi OTP đúng + chưa hết hạn: cho phép `AuthController.ResetPassword` đổi mật khẩu.
7. Logout: `AuthController.Logout` clear session.

### 4.2. Quyền và bảo mật (hiện tại)

- Hệ thống không dùng ASP.NET Identity role-based sẵn; quyền đang kiểm soát bằng session và `VaiTro` trong controller.
- `BaseController` có thể kiểm tra session (view toàn cục). Nếu không đăng nhập sẽ redirect.
- Cần xây thêm `Authorize` và middleware role-based nếu triển khai sản phẩm thật.

### 4.3. Luồng nghiệp vụ quản lý (CRUD)

Các controller chính và chức năng:

- `HoiVienController`: tạo/sửa/xóa/chi tiết/điểm danh (tùy theo view)
- `GoiTapController`: quản lý gói tập
- `HuanLuyenVienController`: quản lý huấn luyện viên
- `DangKyGoiTapController`: tạo đăng ký gói cho hội viên, chọn HLV, gói tập, ngày bắt đầu
- `ThanhToanController`: tạo thanh toán, gán đăng ký, số tiền, trạng thái
- `CheckInController`: tạo check-in theo hội viên và thời gian

### 4.4. Dashboard và báo cáo

`HomeController.Index` tổng hợp:

- Số lượng tổng: `HoiViens`, `GoiTaps`, `HuanLuyenViens`, `DangKyGoiTaps`.
- Check-in hôm nay: query `CheckIns` theo `ThoiGianCheckIn.Date == today`.
- Doanh thu tháng: sum `ThanhToans.SoTien` trong tháng.
- Thống kê `DangKyGoiTap` còn hạn/qua hạn dựa `NgayKetThuc`.
- Đăng ký sắp hết hạn 7 ngày và check-in mới nhất.
- Biểu đồ doanh thu 6 tháng gần nhất và check-in 7 ngày.

## 5. Mô tả bảng dữ liệu và model (chi tiết)

### 5.1. `TaiKhoan`

- `MaTaiKhoan` (int, Key)
- `TenDangNhap` (string, required, max 50)
- `MatKhau` (string, required, hash)
- `HoTen` (string, optional)
- `Email` (string, optional)
- `VaiTro` (admin/nhanvien)
- `TrangThai` (bool: active/inactive)
- OTP: `OtpCode`, `OtpExpiry`, `OtpCanResendAt`, `OtpVerified`

### 5.2. `HoiVien`

- `MaHoiVien` (Key)
- `HoTen`, `GioiTinh`, `NgaySinh`, `SoDienThoai`, `Email`, `DiaChi`
- `NgayDangKy` (mặc định `DateTime.Now`)
- `TrangThai` (default "Hoạt động")

### 5.3. `GoiTap`

- `MaGoiTap`, `TenGoiTap`, `ThoiHanThang`, `GiaTien`, `MoTa`

### 5.4. `HuanLuyenVien`

- `MaHLV`, `HoTen`, `GioiTinh`, `NgaySinh`, `SoDienThoai`, `Email`, `ChuyenMon`, `TrangThai`

### 5.5. `DangKyGoiTap`

- `MaDangKy`, `MaHoiVien`, `MaGoiTap`, `MaHLV?`, `NgayBatDau`, `NgayKetThuc`, `TrangThai`
- Quan hệ 1-n với `HoiVien`, `GoiTap`, `HuanLuyenVien`

### 5.6. `ThanhToan`

- `MaThanhToan`, `MaDangKy`, `NgayThanhToan`, `SoTien`, `PhuongThuc`, `GhiChu`, `TrangThaiThanhToan`, `VietQrUrl` (NotMapped)
- Quan hệ 1-n với `DangKyGoiTap`

### 5.7. `CheckIn`

- `MaCheckIn`, `MaHoiVien`, `ThoiGianCheckIn`, `GhiChu`
- Quan hệ 1-n với `HoiVien`

## 6. Cấu trúc controller -> view maps (chi tiết)

### 6.1. `AuthController`

- `Login` (GET/POST): đăng nhập
- `ForgotPassword` (GET/POST): gửi OTP
- `VerifyOtp` (GET/POST): xác nhận OTP
- `ResendOtp` (POST): gửi lại OTP
- `ResetPassword` (GET/POST): đổi mật khẩu
- `Logout` (GET): đăng xuất
- `AccessDenied`: trang quyền truy cập

### 6.2. `HomeController`

- `Index`: trang dashboard và biểu đồ

### 6.3. `HoiVienController`/`GoiTapController`/`HuanLuyenVienController`/`DangKyGoiTapController`/`ThanhToanController`/`CheckInController`

- `Index`, `Create`, `Edit`, `Details`, `Delete` với validation model
- `Create`/`Edit` dùng `ViewBag`/`SelectList` để chọn dữ liệu liên quan (ví dụ `MaHoiVien`, `MaGoiTap`)

## 7. Setup chi tiết và chạy dự án

### 7.1. Chuẩn bị môi trường

- .NET 8 SDK hoặc .NET 9 SDK
- SQL Server (LocalDB hoặc SQL Server đầy đủ)
- Visual Studio 2022/2023 hoặc VS Code

### 7.2. Cấu hình chuỗi kết nối

Trong `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GymDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 7.3. Migrations và database

Chạy terminal trong folder chứa `.csproj`:

```bash
dotnet ef database update
```

Nếu cần tạo mới migration:

```bash
dotnet ef migrations add InitialCreate
```

### 7.4. Chạy ứng dụng

```bash
dotnet run --project QLPhongGym\QLPhongGym.csproj
```

Mở `https://localhost:5001` (hoặc port được gán).

### 7.5. Tài khoản mặc định (sau lần hash khởi tạo)

- `admin` / `123456`
- `nhanvien` / `123456`

## 8. Các điểm cần chú ý khi nâng cấp / sản xuất

1. **Bảo mật mật khẩu**: dùng ASP.NET Identity + policy password.
2. **Phân quyền**: sử dụng `[Authorize(Roles = "Admin")]`, `[Authorize(Roles = "NhanVien")]`.
3. **Session**: đổi sang claims-based cookie authentication.
4. **OTP và email**: cấu hình SMTP (host, port, user, pass) trong `appsettings`. Xác thực TLS.
5. **Xử lý lỗi**: thêm logging, middleware exception.
6. **Kiểm soát truy cập API**: thêm CSRF, chống SQL injection.

## 9. Mở rộng chức năng đề xuất

- Thêm module `Lịch tập`, `Báo cáo KPI`, `Membership renewal reminder`.
- Export CSV/PDF báo cáo doanh thu.
- Tích hợp payment gateway (VNPAY, Momo).
- Ứng dụng mobile + API JSON.

---

> Tài liệu trên đã mô tả đầy đủ chi tiết hệ thống QLPhongGym: cấu trúc, models, controllers, luồng nghiệp vụ, cài đặt và hướng nâng cấp.
