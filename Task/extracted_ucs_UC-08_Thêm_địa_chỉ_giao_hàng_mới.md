# UC-08: Thêm địa chỉ giao hàng mới

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Thêm địa chỉ giao hàng mới |
| Mã Use Case | UC-08 |
| Mô tả Use Case | Khách hàng thêm một vị trí giao hàng mới (ví dụ: địa chỉ công ty) để có thể chọn nhanh trong quá trình thanh toán. |
| Kích hoạt | Khách hàng nhấn "Add new address" trong sổ địa chỉ hoặc ngay trong bước thanh toán (Checkout). |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Khách hàng đã đăng nhập vào hệ thống. |
| Hậu điều kiện | Một địa chỉ mới được thêm thành công vào danh sách sổ địa chỉ của khách hàng. |
| Luồng sự kiện chính | 1. Khách hàng nhấn "Add new address".<br>2. Hệ thống hiển thị một biểu mẫu địa chỉ trống.<br>3. Khách hàng nhập các chi tiết bắt buộc (Tỉnh/Thành, Quận/Huyện, Địa chỉ cụ thể) và nhấn "Save".<br>4. Hệ thống xác thực dữ liệu đầu vào.<br>5. Hệ thống lưu địa chỉ mới vào CSDL.<br>6. Hệ thống hiển thị thông báo lưu thành công. |
| Luồng sự kiện thay thế | 4a. Khách hàng để trống các trường bắt buộc: Hệ thống tô đỏ các trường đó và yêu cầu nhập liệu trước khi cho phép lưu. |
| Luồng sự kiện ngoại lệ | - Khách hàng đóng biểu mẫu trước khi lưu: Use case dừng. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (2) | BR34 | Loading Rules:<br>1. Hệ thống nhận yêu cầu khi người dùng nhấn chọn thêm địa chỉ mới.<br>2. Tải màn hình addAddressScreen và hiển thị biểu mẫu địa chỉ trống để người dùng nhập liệu. |
| (4) | BR35 | Validate Data Rules:<br>(Luồng xử lý khi người dùng nhấn lưu)<br>1. Kiểm tra tính hợp lệ và định dạng của dữ liệu đầu vào.<br>2. If anyIsEmpty([houseNumber], [ward], [province]) then returns 400-BAD_REQUEST error with MSG1. |
| (7) | BR36 | Message Rules (Invalid Data):<br>Nếu dữ liệu không hợp lệ: Hệ thống trả về lỗi và hiển thị thông báo cảnh báo các trường bỏ trống hoặc sai định dạng tương ứng là MSG1. |
| (5) | BR37 | Storage Rules:<br>(Luồng khi dữ liệu hợp lệ)<br>1. Hệ thống thực hiện lưu thông tin địa chỉ mới vào CSDL và liên kết với tài khoản người dùng hiện tại.<br>2. AddressRepository.create([addressData])<br>3. returns 201-CREATED response with MSG23. |
| (6) | BR38 | Message Rules (Success):<br>Hiển thị thông báo lưu địa chỉ thành công trên giao diện người dùng: MSG23. |

