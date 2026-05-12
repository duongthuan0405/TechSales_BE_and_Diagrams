# UC-01: Đăng ký

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Đăng ký (Sign Up) |
| Mã Use Case | UC-01 |
| Mô tả Use Case | Quá trình khách hàng đăng ký một tài khoản mới sử dụng email. |
| Kích hoạt | Khách hàng nhấn vào nút "Sign Up" trên màn hình. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Thiết bị của khách hàng có kết nối internet. Khách hàng chưa đăng nhập vào hệ thống. |
| Hậu điều kiện | Khách hàng nhận được thông báo đăng ký thành công. Tài khoản được tạo, mật khẩu được mã hóa và lưu trữ bảo mật trong cơ sở dữ liệu. |
| Luồng sự kiện chính | 1. Khách hàng nhấn nút "Sign Up", màn hình đăng ký hiển thị. <br>2. Khách hàng nhập thông tin cá nhân gồm Email, Password, Confirm Password và nhấn nút xác nhận. <br>3. Hệ thống kiểm tra định dạng dữ liệu đầu vào.<br>4. Hệ thống xác minh email nhập vào chưa tồn tại trong cơ sở dữ liệu. <br>5. Hệ thống mã hóa mật khẩu và lưu thông tin khách hàng vào cơ sở dữ liệu tài khoản chờ xác thực<br>6. Hệ thống gửi đường dẫn xác nhận email đến email người dùng<br>7. Người dùng nhấn vào đường dẫn để xác nhận email<br>8. Hệ thống ghi nhận thông tin tài khoản người dùng vào CSDL tài khoản khách hàng.<br>9. Hệ thống hiển thị thông báo đăng ký thành công. |
| Luồng sự kiện thay thế | 3a. Định dạng không hợp lệ: Hệ thống hiển thị thông báo lỗi và yêu cầu khách hàng lặp lại bước 2. <br>4a. Email đã tồn tại trong CSDL: Hệ thống hiển thị thông báo và yêu cầu khách hàng lặp lại bước 2 hoặc chuyển sang màn hình Đăng nhập (Sign In). |
| Luồng sự kiện ngoại lệ | - Khách hàng nhấn "Already have an account?" Use case Đăng ký dừng lại và chuyển sang Đăng nhập. <br>- Khách hàng thoát màn hình đăng ký: Use case dừng lại. <br>- Lỗi hệ thống bất ngờ không thể kết nối CSDL: Hiển thị thông báo lỗi và use case dừng lại. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (2) | BR01 | Loading Rules:<br>Tải màn hình signUpScreen.<br>Yêu cầu các trường dữ liệu: [email], [password], [confirmPassword]. |
| (4) | BR02 | Validate Format Rules:<br>1. If any in [email], [password], [confirmPassword] is empty then returns 400-BAD_REQUEST error with MSG1.<br>2. If pattern.compile('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$').notMatch([email]) then returns 400-BAD_REQUEST error with MSG2.<br>3. If pattern.compile('^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[@#$%^&+=!])(?=\S+$).{8,}$').notMatch([password]) then returns 400-BAD_REQUEST error with MSG3.<br>4. If [password] != [confirmPassword] then returns 400-BAD_REQUEST error with MSG4. |
| (5) | BR03 | Message Rules:<br>Hiển thị thông báo lỗi định dạng không hợp lệ tương ứng: MSG1, MSG2, MSG3, hoặc MSG4. |
| (6) | BR04 | Check Existence Rules:<br>1. [existingUser] = UserRepository.findByEmail([email])<br>2. If [existingUser] != null then returns 400-BAD_REQUEST error with MSG5. |
| (7) | BR05 | Message Rules:<br>Hiển thị thông báo lỗi tài khoản đã tồn tại: MSG5. |
| (8) | BR06 | Saving Rules:<br>[hashedPassword] = hash([password])<br>[newUser] = UserRepository.createNewUser()<br>[newUser.email] = [email]<br>[newUser.password] = [hashedPassword]<br>[newUser.status] = 'PENDING'<br>UserRepository.save([newUser]) |
| (9) | BR07 | Email Template & Message Rules:<br>1. Email được gửi với mẫu sau:<br>- Ví dụ:<br>2. Hiển thị thông báo yêu cầu kiểm tra email: MSG6. |
| (11) | BR08 | Activation & Message Rules:<br>Khi người dùng nhấn vào [verificationLink]:<br>1. [isValid] = verifyToken([verificationToken])<br>2. If [isValid] == true then<br>- [user.status] = 'ACTIVE'<br>- UserRepository.save([user])<br>- returns 200-OK response with MSG7.<br>- Hiển thị thông báo đăng ký thành công: MSG7.<br>3. Else<br>- returns 400-BAD_REQUEST error with MSG8.<br>- Hiển thị thông báo lỗi: MSG8. |

