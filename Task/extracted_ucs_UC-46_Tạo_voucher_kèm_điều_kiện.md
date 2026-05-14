# UC-46: Tạo voucher kèm điều kiện

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Tạo voucher kèm điều kiện |
| Mã Use Case | UC-46 |
| Mô tả Use Case | Quản lý tạo các mã giảm giá để chạy chiến dịch tiếp thị, thiết lập kèm theo các điều kiện ràng buộc như thời hạn, ngân sách tối đa và giá trị đơn hàng tối thiểu. |
| Kích hoạt | Quản lý nhấn nút "Tạo mã giảm giá" tại phân hệ quản lý khuyến mãi. |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Tài khoản có quyền Business Admin đã đăng nhập. |
| Hậu điều kiện | Mã voucher được kích hoạt trên hệ thống và khách hàng có thể bắt đầu sử dụng để thanh toán. |
| Luồng sự kiện chính | 1. Quản lý nhấn tạo voucher mới.<br>2. Hệ thống hiển thị biểu mẫu thiết lập.<br>3. Quản lý nhập thông tin: Mã code, loại giảm giá (phần trăm hoặc số tiền cố định), điều kiện áp dụng và thời gian hiệu lực.<br>4. Quản lý nhấn lưu.<br>5. Hệ thống kiểm tra để đảm bảo mã code chưa từng tồn tại và các điều kiện nhập vào hợp lệ.<br>6. Hệ thống lưu bản ghi voucher vào cơ sở dữ liệu.<br>7. Hệ thống hiển thị thông báo tạo thành công. |
| Luồng sự kiện thay thế | 5a. Mã code đã tồn tại hoặc điều kiện ngân sách bị âm: Hệ thống hiển thị cảnh báo lỗi dữ liệu đầu vào và yêu cầu chỉnh sửa. |
| Luồng sự kiện ngoại lệ | - Lỗi máy chủ cơ sở dữ liệu: Hệ thống hiển thị thông báo sự cố và không lưu dữ liệu. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR238 | Form Initialization:<br>1. Hệ thống hiển thị CreateVoucher_Form. <br>2. Các trường dữ liệu bắt buộc: [voucherCode], [discountValue], [discountType], [quantity], [startDate], [endDate]. |
| (3) & (4) | BR239 | Data Submission:<br>1. Hệ thống tiếp nhận thông tin voucher và các điều kiện áp dụngtừ Business Admin. |
| (5) | BR240 | Validation Logic:<br>1. if isEmpty([voucherCode], [discountValue], [discountType], [quantity], [startDate], [endDate]) then return 400-BAD_REQUEST with MSG1. <br>2. if VoucherRepo.Exists([voucherCode]) then return 409-CONFLICT with MSG88. <br>3. if [endDate] <= [startDate] then return 400-BAD_REQUEST with MSG89. <br>4. if [discountValue] <= 0 or [quantity] <= 0 then return 400-BAD_REQUEST with MSG90. |
| (6) | BR241 | Database Persistence:<br>1. Thực hiện VoucherRepo.Insert([voucherData]) vào Cơ sở dữ liệu. <br>2. Hệ thống tự động thiết lập trạng thái isActive = true nếu startDate trùng với ngày hiện tại. |
| (7) | BR242 | Success Notification:<br>1. return 201-CREATED kèm MSG87 và làm mới danh sách voucher trên giao diện. |
| (8) | BR243 | Error Handling:<br>1. Hiển thị cảnh báo lỗi chi tiết cho từng trường dữ liệu không hợp lệ để người dùng điều chỉnh. |

