# UC-57: Tạo tài khoản Business Admin ban đầu

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Tạo tài khoản Business Admin ban đầu |
| Mã Use Case | UC-57 |
| Mô tả Use Case | Khởi tạo tài khoản Quản trị Kinh doanh đầu tiên để bàn giao cho phía đội ngũ vận hành doanh nghiệp bắt đầu thiết lập cửa hàng. |
| Kích hoạt | Admin Kỹ thuật chạy công cụ khởi tạo tài khoản trên giao diện quản trị. |
| Actors | Quản trị Kỹ thuật (Technical Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Hệ thống vừa được triển khai và chưa có tài khoản Quản trị Kinh doanh nào. |
| Hậu điều kiện | Tài khoản Business Admin được tạo với quyền hạn cao nhất của phân hệ kinh doanh. |
| Luồng sự kiện chính | 1. Admin Kỹ thuật nhập thông tin cho tài khoản mới (Email, Họ tên, Mật khẩu tạm thời).<br>2. Admin chọn vai trò "Business Admin" và nhấn tạo.<br>3. Hệ thống kiểm tra định dạng email và mã hóa mật khẩu theo chuẩn bảo mật.<br>4. Hệ thống lưu tài khoản vào cơ sở dữ liệu cấu hình.<br>5. Hệ thống hiển thị thông báo khởi tạo thành công. |
| Luồng sự kiện thay thế | 3a. Email cung cấp sai định dạng: Hệ thống chặn thao tác và hiển thị cảnh báo lỗi định dạng. |
| Luồng sự kiện ngoại lệ | Không có. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR301 | Input & Role Selection Rules:<br>1. Hệ thống tiếp nhận các thông tin từ Technical Admin: [email], [fullName], [password]. <br>2. Vai trò mặc định được gán cho tài khoản này là BUSINESS_ADMIN. |
| (3) | BR302 | Validate Format Rules:<br>1. If any in [email], [fullName], [password] is empty then returns 400-BAD_REQUEST error with MSG1. <br>2. If pattern.compile('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$').notMatch([email]) then returns 400-BAD_REQUEST error with MSG2. |
| (4) & (5) | BR303 | Security & Persistence Rules:<br>1. Password Encryption: Hệ thống thực hiện mã hóa mật khẩu: [hashedPassword] = hash([password]) trước khi lưu trữ. <br>2. Database Save: Thực hiện ConfigRepository.save(new Account([email], [hashedPassword], 'BUSINESS_ADMIN')) vào CSDL cấu hình hệ thống. |
| (6) | BR304 | Success Notification Rules:<br>1. returns 201-CREATED response với MSG111. <br>2. Hiển thị thông báo khởi tạo thành công và cho phép đăng nhập bằng tài khoản quản trị mới. |
| (7) | BR305 | Error Handling Rules:<br>1. returns 400-BAD_REQUEST kèm MSG2 để cảnh báo lỗi định dạng email. |

