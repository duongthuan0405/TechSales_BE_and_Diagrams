# TechSales Management API

Hệ thống Backend (Web API) quản lý bán hàng công nghệ (Tech Sales Management E-Commerce) được phát triển bằng **ASP.NET Core (.NET 9)** áp dụng mô hình kiến trúc sạch **Clean Architecture (Onion Architecture)**. Dự án được thiết kế có hiệu năng cao, khả năng mở rộng tốt, bảo mật cao và dễ bảo trì.

---

## 🚀 Công Nghệ Sử Dụng (Tech Stack)

*   **Framework chính:** .NET 9.0 (ASP.NET Core Web API)
*   **Database ORM:** Entity Framework Core với **PostgreSQL** (`Npgsql.EntityFrameworkCore.PostgreSQL`)
*   **Caching:** **Redis** (`StackExchange.Redis`) cho hiệu năng truy xuất nhanh
*   **Authentication & Authorization:** **JWT (JSON Web Token)** với cơ chế Role-Based Access Control (RBAC) chi tiết đến từng Quyền (Permission)
*   **Bảo mật mật khẩu:** `BCrypt.Net-Next`
*   **File/Image Storage:** **Cloudinary** (`CloudinaryDotNet`)
*   **Thanh toán trực tuyến:** Tích hợp ví điện tử **MoMo**, **VnPay** và hình thức COD
*   **Gửi Email:** **MailKit / MimeKit** (để gửi OTP kích hoạt, khôi phục mật khẩu)
*   **Validation:** **FluentValidation** tự động xác thực dữ liệu đầu vào
*   **Logging:** **Serilog** ghi log ra cả Console và File
*   **Tài liệu API:** **Swagger / OpenAPI**

---

## 🏛️ Kiến Trúc Dự Án (Clean Architecture)

Cấu trúc chi tiết đến cấp độ thư mục con trong hệ thống `TechSalesManagement`:

```text
TechSalesManagement/
│
├── Domain/                           # Định nghĩa các thực thể cốt lõi và nghiệp vụ (Core Domain)
│   ├── Common/                       # Các Base class chung của Entity
│   ├── Entities/                     # Các Entity (User, Product, Order, Cart, Review, Voucher,...)
│   ├── Enums/                        # Định nghĩa các enum (Trạng thái đơn hàng, thanh toán,...)
│   └── Specifications/               # Các Specification mẫu dùng để lọc truy vấn dữ liệu
│
├── Application/                      # Tầng xử lý logic nghiệp vụ chính (Business Logic)
│   ├── Common/                       # Các lớp cấu hình, hằng số dùng chung trong Application
│   ├── Enums/                        # Enum riêng phục vụ logic Application
│   ├── Exceptions/                   # Custom Exceptions (NotFound, BadRequest,...)
│   ├── HelperServices/               # Interface định nghĩa các service phụ trợ (Email, OTP, Caching,...)
│   ├── Interfaces/                   # Định nghĩa UnitOfWork và các interface dùng chung
│   ├── Repositories/                 # Định nghĩa các Interface cho Repository (IUserRepository,...)
│   └── Services/                     # Logic nghiệp vụ chính
│       ├── Implementations/          # Hiện thực hoá các Services (OrderService, UserService,...)
│       ├── Interfaces/               # Các Interface định nghĩa Services tương ứng
│       ├── Params/                   # Các DTO làm tham số đầu vào cho API/Service
│       └── Strategies/               # Các Strategy Pattern phục vụ nghiệp vụ
│           ├── Payment/              # Thanh toán: CodPayment, MomoPayment, VnPayPayment...
│           ├── Refund/               # Hoàn tiền: CodRefund, VnPayRefund...
│           └── VoucherStrategies/    # Xử lý chiến lược tính giảm giá (Fixed, Percent,...)
│
├── Infrastructure/                   # Tầng tích hợp dịch vụ ngoài và lưu trữ dữ liệu
│   ├── HelperServices/               # Implement các Helper: Caching, Email, OTP, Upload ảnh...
│   ├── Persistence/                  # Cấu hình DbContext, Fluent API mapping và Migrations
│   ├── Repositories/                 # Implement các Repository (EF Core queries)
│   └── Services/                     # Cài đặt chi tiết các dịch vụ bên ngoài (Payment gateways,...)
│
├── Presentation_WebAPI/              # Tầng giao diện API (API Endpoints)
│   ├── Controllers/                  # Các API Controller tiếp nhận và xử lý HTTP Request
│   ├── DTOs/                         # Request/Response Data Transfer Objects cho API
│   ├── Extensions/                   # Dependency Injection, Swagger, Auth, Validation setup
│   └── Middlewares/                  # Middlewares (Global Exception Handler, Request Logging,...)
│
├── Migrations/                       # Database Migrations được sinh ra tự động
└── Properties/                       # Cấu hình launchSettings.json để chạy debug dự án
```

---

## 🌟 Các Phân Hệ Chính (Core Features)

1.  **Hệ thống Thành viên (Identity & RBAC):**
    *   Đăng ký, Đăng nhập (JWT), kích hoạt tài khoản bằng OTP.
    *   Phân quyền chi tiết theo vai trò (Role) và quyền hạn cụ thể (Permission).
    *   Lưu vết hoạt động (Audit Logging).
2.  **Quản lý Sản phẩm & Kho hàng (Product & Inventory):**
    *   Quản lý danh mục sản phẩm đa cấp.
    *   Thông tin sản phẩm, hình ảnh (tích hợp Cloudinary).
    *   Cập nhật và theo dõi số lượng tồn kho tự động.
3.  **Giỏ hàng & Đặt hàng (Cart & Checkout):**
    *   Thêm/sửa/xóa sản phẩm trong giỏ hàng lưu trữ Database.
    *   Tính toán thuế, phí vận chuyển và áp dụng Voucher.
    *   Quy trình đặt hàng (Checkout) đa dạng phương thức thanh toán.
4.  **Hệ thống Giảm giá (Voucher & Discount Strategies):**
    *   Áp dụng **Strategy Pattern** cho việc tính giảm giá theo số tiền cố định (Fixed) hoặc phần trăm (Percent).
5.  **Cổng thanh toán (Payment Integrations):**
    *   Thanh toán bằng **MoMo**, **VnPay** hoặc **COD (Thanh toán khi nhận hàng)**.
    *   Hỗ trợ hoàn tiền (Refund Strategy) tùy theo phương thức đã thanh toán ban đầu.
6.  **Đánh giá & Phản hồi (Reviews & Engagement):**
    *   Khách hàng đánh giá sản phẩm.
    *   Quản trị viên phản hồi lại đánh giá của khách hàng.

---

## 🛠️ Hướng Dẫn Cài Đặt & Chạy Dự Án

### 1. Yêu cầu hệ thống
*   [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
*   [PostgreSQL Database](https://www.postgresql.org/)
*   [Redis Server](https://redis.io/) (Khuyên dùng Docker hoặc cài trực tiếp)

### 2. Cấu hình Môi trường
Tạo một file `.env.development` trong thư mục gốc của dự án `TechSalesManagement/` (hoặc cấu hình trực tiếp vào biến môi trường hệ thống) với nội dung tương tự sau:

```env
# Database Connection String (PostgreSQL)
DB__ConnectionString="Host=localhost;Port=5432;Database=TechSalesDb;Username=postgres;Password=your_password"

# Redis Cache Connection
Redis__ConnectionString="localhost:6379"

# JWT Config
JWT__Secret="YourSuperSecretJWTKeyThatIsLongEnoughToMeetRequirements"
JWT__Issuer="TechSalesManagement"
JWT__Audience="TechSalesClients"
JWT__DurationInMinutes=1440

# Cloudinary Config
Cloudinary__CloudName="your_cloud_name"
Cloudinary__ApiKey="your_api_key"
Cloudinary__ApiSecret="your_api_secret"

# Mail Settings (Gmail SMTP example)
MAIL__Host="smtp.gmail.com"
MAIL__Port=587
MAIL__DisplayName="Tech Sales E-Shop"
MAIL__Mail="your_email@gmail.com"
MAIL__Password="your_app_password"

# Momo API Config
Momo__PartnerCode="your_momo_partner_code"
Momo__AccessKey="your_momo_access_key"
Momo__SecretKey="your_momo_secret_key"
Momo__Endpoint="https://test-payment.momo.vn/v2/gateway/api/create"
```

### 3. Khởi tạo Cơ sở dữ liệu (Migrations)
Mở terminal tại thư mục dự án chứa file `.sln` và chạy lệnh sau để áp dụng các bản cập nhật CSDL (Database Migrations):

```bash
cd TechSalesManagement
dotnet ef database update
```

*(Lưu ý: Bạn cần cài đặt công cụ `dotnet-ef` trước bằng lệnh: `dotnet tool install --global dotnet-ef` nếu chưa có).*

### 4. Khởi chạy dự án
Chạy dự án ở môi trường Development:

```bash
dotnet run
```

Sau khi ứng dụng khởi chạy thành công, bạn có thể truy cập tài liệu Swagger UI tại địa chỉ:
*   `http://localhost:5000/swagger` hoặc `https://localhost:5001/swagger` (tùy thuộc vào cổng cấu hình thực tế).
