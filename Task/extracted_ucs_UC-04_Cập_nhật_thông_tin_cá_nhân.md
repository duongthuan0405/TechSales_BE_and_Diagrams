# UC-04: Cập nhật thông tin cá nhân

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Cập nhật thông tin cá nhân |
| Mã Use Case | UC-04 |
| Mô tả Use Case | Khách hàng có thể sửa đổi chi tiết liên lạc để cửa hàng dễ dàng liên hệ. |
| Kích hoạt | Khách hàng chọn chức năng "Edit Profile" trên màn hình quản lý tài khoản cá nhân. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase “Sign In”. |
| Tiền điều kiện | Khách hàng đã đăng nhập vào hệ thống. |
| Hậu điều kiện | Thông tin cá nhân được cập nhật thành công vào CSDL và hiển thị ngay lập tức trên giao diện. |
| Luồng sự kiện chính | 1. Khách hàng nhấn "Edit Profile", hệ thống hiển thị biểu mẫu với thông tin hiện tại. <br>2. Khách hàng sửa đổi thông tin (Tên, Số điện thoại) và nhấn "Save". <br>3. Hệ thống yêu cầu xác nhận lưu thay đổi. <br>4. Hệ thống xác thực dữ liệu đầu vào<br>5. Hệ thống cập nhật thông tin vào cơ sở dữ liệu. <br>6. Hệ thống hiển thị thông báo cập nhật thành công. |
| Luồng sự kiện thay thế | 3a. Khách hàng không xác nhận lưu: Hệ thống quay lại bước 1. <br>4a. Dữ liệu nhập không hợp lệ (ví dụ: SĐT chứa chữ cái): Hệ thống cảnh báo dưới ô nhập liệu và yêu cầu quay lại bước 2. |
| Luồng sự kiện ngoại lệ | - Khách hàng thoát trang mà không lưu: Dữ liệu không được cập nhật, use case dừng. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR13 | Loading Rules:<br>1. Hệ thống lấy thông tin người dùng hiện tại: [user] = getCurrentUserProfile(). <br>2. Tải màn hình editProfileScreen và hiển thị biểu mẫu với dữ liệu hiện tại vào các trường: [fullName], [phoneNumber]. |
| (3) | BR14 | Input Rules:<br>Khách hàng sửa đổi thông tin [fullName], [phoneNumber] và nhấn nút "Save". |
| (4) | BR15 | Confirmation Rules:<br>Hệ thống yêu cầu xác nhận lưu thay đổi: Hiển thị hộp thoại xác nhận MSG15. |
| (7) | BR16 | Validate Data Rules:<br>Khi khách hàng chọn "Đồng ý" (Confirm):<br>1. If [fullName] is empty then returns 400-BAD_REQUEST error with MSG1. <br>2. If pattern.compile('^[0-9]{10,11}$').notMatch([phoneNumber]) then returns 400-BAD_REQUEST error with MSG16. |
| (8) | BR17 | Message Rules (Invalid Data):<br>Hiển thị cảnh báo lỗi dữ liệu tương ứng trên giao diện: MSG1 hoặc MSG16. |
| (9) | BR18 | Update Rules:<br>(Luồng khi dữ liệu hợp lệ)<br>1. [user.fullName] = [fullName]<br>2. [user.phoneNumber] = [phoneNumber]<br>3. UserProfileRepository.save([user]) <br>4. returns 200-OK response with MSG17. |
| (9) | BR19 | Message Rules (Success):<br>Hiển thị thông báo cập nhật thông tin thành công trên giao diện: MSG17. |

