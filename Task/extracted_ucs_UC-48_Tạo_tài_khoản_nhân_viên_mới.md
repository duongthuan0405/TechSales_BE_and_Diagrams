# UC-48: Tạo tài khoản nhân viên mới

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Tạo tài khoản nhân viên mới |
| Mã Use Case | UC-48 |
| Mô tả Use Case | Quản lý cấp phát tài khoản nội bộ cho các nhân sự mới để họ có thể truy cập hệ thống và thực hiện công việc (ví dụ: Nhân viên Sales). |
| Kích hoạt | Quản lý nhấn nút "Thêm nhân viên" tại phân hệ quản lý nhân sự. |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Tài khoản có quyền phân quyền nhân sự (Business Admin) đang đăng nhập. |
| Hậu điều kiện | Tài khoản nhân sự được khởi tạo thành công với các quyền hạn tương ứng. |
| Luồng sự kiện chính | 1. Quản lý chọn chức năng thêm nhân viên.<br>2. Hệ thống hiển thị biểu mẫu thông tin cơ bản và phân quyền.<br>3. Quản lý nhập Email, Họ tên và gán quyền hạn (Role) cho nhân viên đó.<br>4. Quản lý nhấn xác nhận tạo.<br>5. Hệ thống kiểm tra xem email nội bộ này đã được sử dụng hay chưa.<br>6. Hệ thống tạo tài khoản, sinh mật khẩu tạm thời và lưu vào cơ sở dữ liệu.<br>7. Hệ thống tự động gửi email chứa thông tin đăng nhập đến nhân sự mới.<br>8. Hệ thống hiển thị thông báo tạo tài khoản thành công. |
| Luồng sự kiện thay thế | 5a. Email đã được đăng ký cho một nhân viên khác: Hệ thống hiển thị cảnh báo lỗi trùng lặp dữ liệu. |
| Luồng sự kiện ngoại lệ | - 7a. Lỗi kết nối dịch vụ email SMTP: Hệ thống vẫn tạo tài khoản nhưng hiển thị cảnh báo phụ rằng email thông báo chưa được gửi, cho phép quản lý cấp lại mật khẩu thủ công sau. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR249 | Loading Rules:<br>1. Tải màn hình CreateStaff_Form.<br>2. Hệ thống yêu cầu các trường dữ liệu: [email], [fullName], [roleId]. |
| (3) & (4) | BR250 | Validate Format Rules:<br>1. If any in [email], [fullName], [roleId] is empty then returns 400-BAD_REQUEST error with MSG1. <br>2. If pattern.compile('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+.[a-zA-Z]{2,}$').notMatch([email]) then returns 400-BAD_REQUEST error with MSG2. |
| (5) | BR251 | Check Conflict Rules:<br>1. [account] = AccountRepository.findByEmail([email]).<br>2. If [account] != null then proceeds to Activity (9).<br>3. Else proceeds to Activity (6). |
| (6) & (7) | BR252 | Account Creation & Email Rules:<br>1. [tempPassword] = generateRandomPassword().<br>2. [hashedPassword] = hash([tempPassword]).<br>3. UserRepository.save([email], [roleId], [hashedPassword]).<br>4. UserProfileRepository.save([fullName])<br>4. EmailService.sendLoginDetails([email], [tempPassword]). |
| (8) | BR253 | Success & Notification Rules:<br>1. returns 201-CREATED response with MSG93.<br>2. Hiển thị thông báo thành công và làm mới danh sách nhân viên. |
| (9) | BR254 | Conflict Handling Rules:<br>1. returns 409-CONFLICT error with MSG94.<br>2. Hiển thị thông báo tài khoản đã tồn tại: MSG94. |

