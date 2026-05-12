# AUTHENTICATION USE CASES

---

## UC-01: Đăng ký (Sign Up)

### 1. Mô tả chi tiết & Logic nghiệp vụ

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

### 2. Business Rules

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

---

## UC-02: Đăng nhập (Sign In)

### 1. Mô tả chi tiết & Logic nghiệp vụ

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

### 2. Business Rules

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

---

## UC-03: Quên mật khẩu

### 1. Mô tả chi tiết & Logic nghiệp vụ

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

### 2. Business Rules

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

---

## UC-05: Thay đổi mật khẩu khi muốn

### 1. Mô tả chi tiết & Logic nghiệp vụ

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

### 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (2) | BR20 | Loading Rules:<br>Tải màn hình changePasswordScreen và hiển thị form cập nhật mật khẩu.<br>Hệ thống yêu cầu các trường: [currentPassword], [newPassword], [confirmPassword]. |
| (5) | BR21 | Check Current Password Rules:<br>(Luồng khi người dùng nhấn "Update")<br>1. [user] = getCurrentUser()<br>2. [hashedCurrentPassword] = hash([currentPassword])<br>3. If [user.password] != [hashedCurrentPassword] then returns 400-BAD_REQUEST error with MSG18. |
| (10) | BR22 | Message Rules (Wrong Current Password):<br>Hiển thị thông báo lỗi mật khẩu hiện tại không chính xác: MSG18. |
| (6) | BR23 | Validate New Password Rules:<br>1. If any in [currentPassword], [newPassword], [confirmPassword] is empty then returns 400-BAD_REQUEST error with MSG1.<br>2. If [newPassword] == [currentPassword] then returns 400-BAD_REQUEST error with MSG19.<br>3. If pattern.compile('^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[@#$%^&+=!])(?=\S+$).{8,}$').notMatch([newPassword]) then returns 400-BAD_REQUEST error with MSG3.<br>4. If [newPassword] != [confirmPassword] then returns 400-BAD_REQUEST error with MSG4. |
| (9) | BR24 | Message Rules (Invalid New Password):<br>Hiển thị thông báo lỗi mật khẩu mới không hợp lệ hoặc trùng mật khẩu cũ tương ứng: MSG1, MSG3, MSG4, hoặc MSG19. |
| (7) | BR25 | Update Rules:<br>(Luồng khi mật khẩu hợp lệ)<br>1. [hashedNewPassword] = hash([newPassword])<br>2. [user.password] = [hashedNewPassword]<br>3. UserRepository.save([user])<br>4. returns 200-OK response with MSG20. |
| (8) | BR26 | Message Rules (Success):<br>Hiển thị thông báo đổi mật khẩu thành công: MSG20. |