# UC-03: Quên mật khẩu

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Quên mật khẩu (Forget Password) |
| Mã Use Case | UC-03 |
| Mô tả Use Case | Cách khách hàng có thể khôi phục lại mật khẩu đã quên thông qua email. |
| Kích hoạt | Khách hàng chọn chức năng "Forget Password" trên màn hình đăng nhập. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase “Đăng ký” |
| Tiền điều kiện | Khách hàng đã có tài khoản đăng ký trên hệ thống. |
| Hậu điều kiện | Mật khẩu mới được thiết lập thành công và khách hàng có thể dùng nó để đăng nhập. |
| Luồng sự kiện chính | 1. Khách hàng chọn "Forget Password" và nhập email đã đăng ký. <br>2. Hệ thống xác minh email và gửi một liên kết khôi phục dùng một lần qua email. <br>3. Khách hàng mở email và nhấn vào liên kết khôi phục.<br>4. Hệ thống hiển thị màn hình đặt lại mật khẩu. <br>5. Khách hàng nhập mật khẩu mới và xác nhận. <br>6. Hệ thống mã hóa và cập nhật mật khẩu mới vào cơ sở dữ liệu. <br>7. Hệ thống hiển thị thông báo cập nhật mật khẩu thành công. |
| Luồng sự kiện thay thế | 2a. Email không tồn tại: Hệ thống hiển thị lỗi và yêu cầu lặp lại bước 1. <br>3a. Liên kết hết hạn: Hệ thống thông báo và yêu cầu bắt đầu lại quy trình. <br>5a. Mật khẩu mới sai định dạng hoặc không khớp: Yêu cầu lặp lại bước 5. |
| Luồng sự kiện ngoại lệ | - Khách hàng thoát màn hình "Forget Password": Use case dừng.<br>- Lỗi dịch vụ email bên thứ ba: Hệ thống hiển thị lỗi và cho phép người dùng thử lại. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR01 | Loading Rules:<br>Tải màn hình forgotPasswordScreen.<br>Hệ thống yêu cầu trường dữ liệu: [email]. |
| (2) | BR02 | Check Existence Rules:<br>1. [user] = UserRepository.findByEmail([email])<br>2. If [user] == null then returns 404-NOT_FOUND error with MSG12. |
| (3) | BR03 | Message Rules (Not Found):<br>Hiển thị thông báo lỗi email không tồn tại trên giao diện: MSG12. |
| (4) | BR04 | Email Template & Success Rules:<br>(Luồng khi email tồn tại)<br>1. Gửi email theo mẫu:<br>- Ví dụ:<br>[fullName] = UserProfile.getFullNameOfUserWithEmail([email])<br><br>2. Message Rules: Hiển thị thông báo đã gửi email khôi phục: MSG13. |
| (5) | BR05 | Trigger Rules:<br>Hệ thống nhận request từ khách hàng khi truy cập vào [resetLink] từ email. |
| (6) | BR06 | Validate Token Rules:<br>1. [isValid] = verifyToken([resetToken])<br>2. If [isValid] == false OR isExpired([resetToken]) then returns 400-BAD_REQUEST error with MSG8. |
| (7) | BR07 | Message Rules (Invalid Link):<br>Hiển thị thông báo lỗi liên kết không hợp lệ hoặc đã hết hạn: MSG8. |
| (8) | BR08 | Loading Rules:<br>(Luồng khi token hợp lệ)<br>Tải màn hình resetPasswordScreen.<br>Hệ thống yêu cầu nhập: [newPassword], [confirmPassword]. |
| (9) | BR09 | Input Rules:<br>Người dùng nhập mật khẩu mới và xác nhận lưu. |
| (10) | BR10 | Validate Password Rules:<br>1. If pattern.compile('^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[@#$%^&+=!])(?=\S+$).{8,}$').notMatch([newPassword]) then returns 400-BAD_REQUEST error with MSG3.<br>2. If [newPassword] != [confirmPassword] then returns 400-BAD_REQUEST error with MSG4. |
| (11) | BR11 | Message Rules (Invalid Password):<br>Hiển thị thông báo lỗi mật khẩu không khớp hoặc sai định dạng tương ứng: MSG3 hoặc MSG4. |
| (12) | BR12 | Update & Message Rules:<br>(Luồng khi mật khẩu hợp lệ)<br>1. [hashedPassword] = hash([newPassword])<br>2. [user.password] = [hashedPassword]<br>3. UserRepository.save([user])<br>4. returns 200-OK response with MSG14.<br>5. Message Rules: Cập nhật mật khẩu mới và hiển thị thông báo thành công: MSG14. |

