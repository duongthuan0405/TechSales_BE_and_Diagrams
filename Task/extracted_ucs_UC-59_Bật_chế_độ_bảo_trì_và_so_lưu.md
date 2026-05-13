# UC-59: Bật chế độ bảo trì và so lưu

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Bật chế độ bảo trì và tự động sao lưu |
| Mã Use Case | UC-59 |
| Mô tả Use Case | Admin Kỹ thuật tạm dừng hệ thống đối với khách hàng để tiến hành nâng cấp máy chủ, đồng thời hệ thống tự động chạy quy trình sao lưu toàn bộ cơ sở dữ liệu để bảo vệ an toàn thông tin. |
| Kích hoạt | Admin Kỹ thuật nhấn nút "Kích hoạt bảo trì" trên bảng điều khiển. |
| Actors | Quản trị Kỹ thuật (Technical Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Admin Kỹ thuật có quyền truy cập vào cấu hình máy chủ cốt lõi. |
| Hậu điều kiện | Hệ thống chuyển sang chế độ bảo trì và dữ liệu được sao lưu thành công. |
| Luồng sự kiện chính | 1. Admin Kỹ thuật nhấn xác nhận bật chế độ bảo trì.<br>2. Hệ thống chuyển đổi giao diện khách hàng thành màn hình thông báo đang bảo trì.<br>3. Hệ thống tự động kích hoạt tiến trình sao lưu toàn bộ cơ sở dữ liệu hiện tại.<br>4. Hệ thống kiểm tra tính toàn vẹn của tệp tin sao lưu vừa tạo.<br>5. Hệ thống hiển thị thông báo đã bật bảo trì và sao lưu thành công cho Admin. |
| Luồng sự kiện thay thế | Không có. |
| Luồng sự kiện ngoại lệ | - Tiến trình sao lưu thất bại do lỗi dung lượng ổ cứng hoặc lỗi máy chủ: Hệ thống lập tức hủy bỏ quá trình bảo trì, không chuyển trạng thái hệ thống và hiển thị cảnh báo lỗi sao lưu khẩn cấp. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR311 | Maintenance Initiation Rules:<br>1. Hệ thống tiếp nhận lệnh kích hoạt bảo trì từ Technical Admin. <br>2. Yêu cầu xác nhận lần cuối qua hộp thoại Maintenance_Confirm_Modal. |
| (2) | BR312 | UI Redirection Rules:<br>1. Hệ thống thực hiện chuyển hướng toàn bộ traffic của khách hàng sang Maintenance_Page (Màn hình 503 Service Unavailable). <br>2. Chỉ cho phép các địa chỉ IP thuộc danh sách Admin_Whitelisted_IPs truy cập vào hệ thống quản trị. |
| (3) | BR313 | Backup Execution Rules:<br>1. Hệ thống tự động kích hoạt script sao lưu toàn bộ Cơ sở dữ liệu: DatabaseService.BackupToCloudStorage(). <br>2. Tệp sao lưu phải được định dạng theo chuẩn .sql.gz hoặc .bak kèm theo dấu thời gian. |
| (4) | BR314 | Integrity Check Rules:<br>1. Hệ thống thực hiện: [isValid] = VerifyChecksum([backupFile]). <br>2. Kiểm tra dung lượng tệp sao lưu phải lớn hơn 0 và cấu trúc tệp không bị lỗi. <br>3. if [isValid] == true then proceeds to Activity (5). <br>4. else proceeds to Activity (6). |
| (5) | BR315 | Success Notification Rules:<br>1. returns 200-OK kèm MSG113. <br>2. Ghi nhận trạng thái System_Status = 'MAINTENANCE' vào bảng cấu hình hệ thống. |
| (6) | BR316 | Emergency Rollback Rules:<br>1. Hủy bỏ chế độ bảo trì và khôi phục giao diện khách hàng về trạng thái hoạt động bình thường. <br>2. returns 500-INTERNAL_SERVER_ERROR kèm cảnh báo khẩn cấp MSG114. |

