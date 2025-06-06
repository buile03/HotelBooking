# 🏨 Hotel Booking

> Hệ thống quản lý và đặt phòng khách sạn – được xây dựng bằng ASP.NET Core theo kiến trúc phân lớp (modular architecture) nhằm tối ưu hoá khả năng mở rộng, bảo trì và phát triển lâu dài.

---

## 📌 Mục tiêu dự án

- Đặt phòng khách sạn nhanh chóng
- Quản lý phòng, loại phòng, trạng thái phòng
- Quản lý người dùng và phân quyền (Admin, Khách hàng)
- Quản lý lịch sử đặt phòng
- Kiểm tra tình trạng phòng
- Tích hợp xác thực, gửi mail, v.v.

---

## 🏗️ Cấu trúc thư mục hệ thống

```plaintext
HotelBookingSolution/
├── App/                 # ASP.NET Core MVC Web App chính (giao diện Razor)
│   ├── Controllers/
│   ├── Views/
│   ├── ViewModels/
│   └── Program.cs
│
├── API/                 # ASP.NET Core Web API (dành cho SPA/mobile nếu cần)
│   └── Controllers/
│
├── Common/              # Thư viện dùng chung (Helpers, Extensions, Exception, etc.)
│   ├── Extensions/
│   ├── Exceptions/
│   ├── Resources/
│   └── Common.csproj
│
├── Data/                # Thư viện truy cập dữ liệu
│   ├── Data/            # AppDbContext, Db configs
│   ├── Repositories/    # Repositories theo domain
│   ├── Migrations/
│   └── Data.csproj
│
├── Model/               # DTOs, Entities, Enums, ViewModels
│   ├── Entities/
│   ├── DTOs/
│   ├── Enums/
│   └── Model.csproj
│
├── Service/             # Business logic services (Email, Booking, Auth)
│   ├── Interfaces/
│   ├── Implementations/
│   └── Service.csproj
│
└── HotelBookingSolution.sln
