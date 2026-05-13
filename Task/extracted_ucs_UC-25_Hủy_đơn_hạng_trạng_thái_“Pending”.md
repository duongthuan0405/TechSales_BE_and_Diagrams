# UC-25: Hủy đơn hạng trạng thái “Pending”

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Hủy đơn hàng trạng thái "Pending" |
| Mã Use Case | UC-25 |
| Mô tả Use Case | Khách hàng được phép tự chủ động hủy đơn hàng vừa đặt để thay đổi quyết định mua sắm, với điều kiện đơn hàng chưa được nhân viên bán hàng duyệt. |
| Kích hoạt | Khách hàng nhấn nút "Hủy đơn hàng" trong trang chi tiết đơn hàng. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase "Xem chi tiết đơn hàng cũ" |
| Tiền điều kiện | Đơn hàng đang ở trạng thái "Pending" (Chờ xử lý). |
| Hậu điều kiện | Đơn hàng chuyển sang trạng thái đã hủy. Số lượng sản phẩm tương ứng được cộng trả lại vào kho. |
| Luồng sự kiện chính | 1. Khách hàng nhấn nút hủy tại một đơn hàng đang "Pending".<br>2. Hệ thống hiển thị hộp thoại yêu cầu người dùng xác nhận quyết định hủy.<br>3. Khách hàng nhấn đồng ý xác nhận.<br>4. Hệ thống bắt đầu một giao dịch nguyên tử (Atomic transaction):<br>- Cập nhật trạng thái đơn hàng từ "Pending" sang "Canceled" (Đã hủy).<br>- Hoàn trả (cộng lại) số lượng các sản phẩm trong đơn vào tổng số tồn kho.<br>5. Hệ thống hoàn tất (Commit) giao dịch.<br>6. Hệ thống hiển thị thông báo hủy đơn thành công và làm mới giao diện chi tiết đơn. |
| Luồng sự kiện thay thế | 3a. Khách hàng nhấn "Đóng" hoặc từ chối tại hộp thoại xác nhận: Hệ thống ẩn hộp thoại và không thực hiện thao tác nào. |
| Luồng sự kiện ngoại lệ | - 4a. Nhân viên Sales vừa duyệt đơn hàng cách đó vài giây (Trạng thái nội bộ đã chuyển sang Approved): Hệ thống chặn yêu cầu hủy, hiển thị thông báo lỗi đơn hàng đã được xử lý không thể tự hủy, và yêu cầu khách hàng làm mới trang để cập nhật trạng thái thực.<br>- 4b. Lỗi máy chủ cập nhật CSDL: Tự động Rollback, hiển thị thông báo sự cố hệ thống. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR118 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu hủy đơn từ màn hình chi tiết đơn hàng.<br>2. Trường dữ liệu yêu cầu từ client: [orderId].<br>3. Hệ thống hiển thị hộp thoại xác nhận hủy đơn với nội dung: MSG44. |
| (3) | BR119 | Decision Rules:<br>1. Nếu khách hàng chọn "Đồng ý" (Confirm): Chuyển sang Activity (4).<br>2. Nếu khách hàng chọn "Từ chối" (Cancel): Chuyển sang Activity (9). |
| (4) | BR120 | Status Validation Rules:<br>1. Hệ thống truy vấn trạng thái thực tế của đơn hàng trong CSDL: [currentStatus] = OrderRepository.getStatus([orderId]).<br>2. If [currentStatus] != 'PENDING' then returns 400-BAD_REQUEST kèm MSG45.<br>3. Else chuyển sang Activity (5). |
| (5) & (6) | BR121 | Cancellation & Inventory Rules:<br>1. Cập nhật trạng thái đơn hàng sang "Canceled": OrderRepository.updateStatus([orderId], 'CANCELED').<br>2. Hoàn trả số lượng sản phẩm vào kho: InventoryRepository.restoreStock([orderId]).<br>3. Trả về phản hồi 200-OK kèm dữ liệu trạng thái mới. |
| (7) | BR122 | Success Message Rules:<br>Hiển thị thông báo hủy đơn hàng thành công: MSG46 và cập nhật giao diện hiển thị trạng thái mới. |
| (8) | BR123 | Error Message Rules:<br>Hiển thị thông báo lỗi đơn hàng không thể hủy do đã được xử lý: MSG45. |
| (9) | BR124 | Cancellation Rules:<br>Hệ thống đóng hộp thoại xác nhận và giữ nguyên trạng thái hiện tại của đơn hàng. |

