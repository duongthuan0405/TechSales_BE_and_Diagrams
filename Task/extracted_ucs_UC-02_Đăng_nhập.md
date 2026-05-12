# UC-02: Đăng nhập

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Đăng nhập (Sign In) |
| Mã Use Case | UC-02 |
| Mô tả Use Case | Quá trình khách hàng đăng nhập vào hệ thống để sử dụng các tính năng dành cho thành viên. |
| Kích hoạt | Khách hàng nhấn vào nút "Sign In". |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase “Quên mật khẩu” <br>Usecase “Đăng ký” |
| Tiền điều kiện | Khách hàng đã đăng ký tài khoản thành công. |
| Hậu điều kiện | Khách hàng truy cập hệ thống thành công để thực hiện mua sắm. |
| Luồng sự kiện chính | 1. Khách hàng nhấn nút "Sign In". <br>2. Màn hình đăng nhập hiển thị; khách hàng nhập email và mật khẩu. <br>3. Hệ thống xác thực định dạng dữ liệu đầu vào. <br>4. Hệ thống kiểm tra thông tin đăng nhập với cơ sở dữ liệu. <br>5. Hệ thống thông báo đăng nhập thành công và chuyển hướng khách hàng về trang chủ. |
| Luồng sự kiện thay thế | 3a. Định dạng không hợp lệ: Hệ thống hiển thị thông báo lỗi và yêu cầu lặp lại bước 2. <br>4a. Tài khoản không tồn tại hoặc sai mật khẩu: Hệ thống hiển thị thông báo và yêu cầu lặp lại bước 2. <br>4b. Khách hàng nhập sai mật khẩu quá 5 lần: Hệ thống tự động khóa tài khoản tạm thời vì mục đích bảo mật. |
| Luồng sự kiện ngoại lệ | - Khách hàng nhấn "Forget Password": Use case Đăng nhập dừng và chuyển sang luồng Quên mật khẩu. <br>- Khách hàng thoát màn hình: Use case dừng. <br>- Lỗi kết nối CSDL: Hệ thống hiển thị thông báo lỗi và use case dừng. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR01 | Loading Rules:<br>Tải màn hình signInScreen.<br>Hệ thống yêu cầu các trường dữ liệu: [email], [password]. |
| (2) | BR02 | Validate Format Rules:<br>1. If any in [email], [password] is empty then returns 400-BAD_REQUEST error with MSG1.<br>2. If pattern.compile('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$').notMatch([email]) then returns 400-BAD_REQUEST error with MSG2. |
| (3) | BR03 | Message Rules:<br>Hiển thị thông báo lỗi định dạng không hợp lệ tương ứng: MSG1 hoặc MSG2. |
| (4) | BR04 | Check Credentials Rules:<br>1. [user] = UserRepository.findByEmail([email])<br>2. [hashedPassword] = hash([password])<br>3. If [user] == null OR [user.password] != [hashedPassword] then proceeds to Activity (5)<br>4. Else proceeds to Activity (8) |
| (5) | BR05 | Check Failed Attempts Rules:<br>(Luồng xử lý khi sai thông tin đăng nhập)<br>1. [user.failedLoginAttempts] = [user.failedLoginAttempts] + 1<br>2. UserRepository.save([user])<br>3. If [user.failedLoginAttempts] >= 5 then returns 403-FORBIDDEN error with MSG9.<br>4. Else returns 401-UNAUTHORIZED error with MSG10. |
| (6) | BR06 | Message Rules (Account Locked):<br>Hiển thị thông báo tài khoản bị khóa tạm thời: MSG9. |
| (7) | BR07 | Message Rules (Wrong Credentials):<br>Hiển thị thông báo sai thông tin tài khoản hoặc mật khẩu: MSG10. |
| (8) | BR08 | Success & Redirect Rules:<br>(Luồng xử lý khi thông tin chính xác)<br>1. [user.failedLoginAttempts] = 0<br>2. [sessionToken] = generateJWT([user.id])<br>3. UserRepository.save([user])<br>4. returns 200-OK response with MSG11.<br>5. Message & Redirect Rules: Hiển thị thông báo đăng nhập thành công MSG11 và chuyển hướng người dùng về trang chủ. |

