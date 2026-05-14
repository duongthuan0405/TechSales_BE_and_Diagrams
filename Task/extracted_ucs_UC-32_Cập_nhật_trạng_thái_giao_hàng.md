# UC-32: Cập nhật trạng thái giao hàng

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Cập nhật trạng thái giao hàng |
| Mã Use Case | UC-32 |
| Mô tả Use Case | Nhân viên đánh dấu đơn hàng đã được bàn giao cho đơn vị vận chuyển để khách hàng có thể bắt đầu theo dõi tiến trình giao hàng. |
| Kích hoạt | Nhân viên Sales nhấn nút "Chuyển giao hàng" tại đơn hàng đã được duyệt. |
| Actors | Nhân viên Sales (Sales Staff) |
| Use Case liên quan | Usecase "Xem chi tiết đơn hàng" |
| Tiền điều kiện | Đơn hàng đang ở trạng thái "Approved" (Đã duyệt). |
| Hậu điều kiện | Đơn hàng chuyển sang trạng thái đang vận chuyển. |
| Luồng sự kiện chính | 1. Nhân viên nhấn nút cập nhật trạng thái giao hàng.<br>2. Hệ thống yêu cầu xác nhận thao tác P<br>3. Nhân viên xác nhận.<br>4. Hệ thống cập nhật trạng thái đơn hàng thành "Shipping" (Đang giao).<br>5. Hệ thống ghi nhận thao tác vào Audit Log.<br>6. Hệ thống hiển thị thông báo cập nhật thành công và làm mới giao diện. |
| Luồng sự kiện thay thế | 2a. Nhân viên bỏ trống mã vận đơn: Hệ thống hiển thị cảnh báo lỗi và yêu cầu nhập đầy đủ thông tin. |
| Luồng sự kiện ngoại lệ | - Mất kết nối mạng khi đang lưu: Hệ thống hiển thị cảnh báo lỗi đường truyền. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR148 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu từ Nhân viên Sales tại màn hình chi tiết đơn hàng. <br>2. Các trường dữ liệu yêu cầu từ client: [orderId]. <br>3. Hệ thống hiển thị form nhập liệu bao gồm trường: [trackingNumber] (Mã vận đơn). |
| (4) | BR149 | Tracking Number Validation Rules:<br>1. Kiểm tra tính hợp lệ của mã vận đơn.<br>2. If isEmpty([trackingNumber]) then returns 400-BAD_REQUEST kèm MSG1. <br>3. Else chuyển sang Activity (5). |
| (5) & (6) | BR150 | Update & Audit Rules:<br>1. Hệ thống thực hiện cập nhật trạng thái đơn hàng: OrderRepository.updateStatus([orderId], 'SHIPPING'). <br>2. Lưu mã vận đơn vào bản ghi đơn hàng: OrderRepository.setTrackingNumber([orderId], [trackingNumber]). <br>3. Ghi nhận thao tác vào hệ thống giám sát: AuditLogger.log([userId], 'UPDATE_STATUS', [orderId], 'SHIPPING'). |
| (7) | BR151 | Success Notification Rules:<br>1. Trả về phản hồi 200-OK kèm thông báo MSG53. <br>2. Cập nhật lại giao diện hiển thị trạng thái và mã vận đơn mới trên màn hình chi tiết. |
| (8) | BR152 | Error Message Rules:<br>Hiển thị cảnh báo lỗi yêu cầu nhân viên nhập đầy đủ thông tin mã vận đơn: MSG1. |

