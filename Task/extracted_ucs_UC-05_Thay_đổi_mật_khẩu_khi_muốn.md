# UC-05: Thay đổi mật khẩu khi muốn

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Chủ động đổi mật khẩu |
| Mã Use Case | UC-05 |
| Mô tả Use Case | Khách hàng cập nhật mật khẩu định kỳ để cải thiện bảo mật tài khoản. |
| Kích hoạt | Khách hàng chọn chức năng "Change Password" trong phần bảo mật tài khoản. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase “Đăng nhập”. |
| Tiền điều kiện | Khách hàng đã đăng nhập vào hệ thống. |
| Hậu điều kiện | Mật khẩu cũ bị vô hiệu hóa, mật khẩu mới có hiệu lực và được mã hóa an toàn. |
| Luồng sự kiện chính | 1. Khách hàng nhấn "Change Password", hệ thống hiển thị biểu mẫu cập nhật. <br>2. Khách hàng nhập mật khẩu hiện tại, mật khẩu mới, xác nhận mật khẩu mới, sau đó nhấn "Update". <br>3. Hệ thống xác minh mật khẩu hiện tại có khớp với CSDL hay không. <br>4. Hệ thống xác thực định dạng mật khẩu mới (đảm bảo độ mạnh và phải khác mật khẩu cũ). <br>5. Hệ thống mã hóa và lưu mật khẩu mới vào cơ sở dữ liệu. <br>6. Hệ thống hiển thị thông báo đổi mật khẩu thành công. |
| Luồng sự kiện thay thế | 3a. Mật khẩu hiện tại sai: Hệ thống hiển thị lỗi và yêu cầu nhập lại. <br>4a. Mật khẩu mới trùng với mật khẩu cũ hoặc sai định dạng: Hệ thống hiển thị lỗi và yêu cầu lặp lại bước 2. |
| Luồng sự kiện ngoại lệ | - Khách hàng nhấn "Cancel" hoặc đóng màn hình: Use case dừng. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (2) | BR20 | Loading Rules:<br>Tải màn hình changePasswordScreen và hiển thị form cập nhật mật khẩu.<br>Hệ thống yêu cầu các trường: [currentPassword], [newPassword], [confirmPassword]. |
| (5) | BR21 | Check Current Password Rules:<br>(Luồng khi người dùng nhấn "Update")<br>1. [user] = getCurrentUser()<br>2. [hashedCurrentPassword] = hash([currentPassword])<br>3. If [user.password] != [hashedCurrentPassword] then returns 400-BAD_REQUEST error with MSG18. |
| (10) | BR22 | Message Rules (Wrong Current Password):<br>Hiển thị thông báo lỗi mật khẩu hiện tại không chính xác: MSG18. |
| (6) | BR23 | Validate New Password Rules:<br>1. If any in [currentPassword], [newPassword], [confirmPassword] is empty then returns 400-BAD_REQUEST error with MSG1.<br>2. If [newPassword] == [currentPassword] then returns 400-BAD_REQUEST error with MSG19.<br>3. If pattern.compile('^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[@#$%^&+=!])(?=\S+$).{8,}$').notMatch([newPassword]) then returns 400-BAD_REQUEST error with MSG3.<br>4. If [newPassword] != [confirmPassword] then returns 400-BAD_REQUEST error with MSG4. |
| (9) | BR24 | Message Rules (Invalid New Password):<br>Hiển thị thông báo lỗi mật khẩu mới không hợp lệ hoặc trùng mật khẩu cũ tương ứng: MSG1, MSG3, MSG4, hoặc MSG19. |
| (7) | BR25 | Update Rules:<br>(Luồng khi mật khẩu hợp lệ)<br>1. [hashedNewPassword] = hash([newPassword])<br>2. [user.password] = [hashedNewPassword]<br>3. UserRepository.save([user])<br>4. returns 200-OK response with MSG20. |
| (8) | BR26 | Message Rules (Success):<br>Hiển thị thông báo đổi mật khẩu thành công: MSG20. |

