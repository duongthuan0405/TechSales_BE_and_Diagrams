# UC-33: Cập nhật trạng thái “Đã giao”

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Xác nhận trạng thái "Delivered" |
| Mã Use Case | UC-33 |
| Mô tả Use Case | Nhân viên đóng đơn hàng sau khi nhận được xác nhận kiện hàng đã đến tay khách hàng thành công, chính thức ghi nhận doanh thu. |
| Kích hoạt | Nhân viên nhấn nút "Đã giao thành công" tại đơn hàng đang vận chuyển. |
| Actors | Nhân viên Sales (Sales Staff) |
| Use Case liên quan | Usecase "Xem chi tiết đơn hàng" |
| Tiền điều kiện | Đơn hàng đang ở trạng thái "Shipping". |
| Hậu điều kiện | Đơn hàng hoàn tất vòng đời, đóng trạng thái và được tính vào báo cáo doanh thu. |
| Luồng sự kiện chính | 1. Nhân viên nhấn nút xác nhận hoàn thành đơn hàng.<br>2. Hệ thống yêu cầu xác nhận thao tác cuối cùng.<br>3. Nhân viên đồng ý.<br>4. Hệ thống cập nhật trạng thái đơn hàng thành "Delivered" (Đã giao).<br>5. Hệ thống ghi nhận dữ liệu tài chính vào phân hệ báo cáo doanh thu.<br>6. Hệ thống hiển thị thông báo hoàn tất đơn hàng. |
| Luồng sự kiện thay thế | Không có. |
| Luồng sự kiện ngoại lệ | - Lỗi cập nhật CSDL: Hệ thống hiển thị thông báo sự cố và yêu cầu thử lại thao tác. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR158 | Loading & Confirmation Rules:<br>1. Hệ thống tiếp nhận tương tác từ Nhân viên Sales tại màn hình chi tiết đơn hàng đang ở trạng thái 'Shipping'. <br>2. Các trường dữ liệu yêu cầu từ client: [orderId]. <br>3. Hệ thống hiển thị hộp thoại xác nhận hoàn tất giao hàng với nội dung: MSG56. |
| (3) | BR159 | Completion Confirmation Rules:<br>Hệ thống chỉ thực hiện các bước tiếp theo khi người dùng nhấn nút xác nhận trên hộp thoại. Nếu người dùng đóng hoặc hủy hộp thoại, hệ thống giữ nguyên trạng thái đơn hàng hiện tại. |
| (4) & (5) | BR160 | Processing & Audit Rules:<br>1. Hệ thống cập nhật trạng thái đơn hàng trong CSDL: OrderRepository.updateStatus([orderId], 'DELIVERED'). <br>2. Cập nhật thời gian hoàn thành thực tế: [order.completedAt] = DateTime.Now. <br>3. Ghi nhận hành động vào nhật ký hệ thống để phục vụ kiểm tra: AuditLogger.log([userId], 'SET_DELIVERED', [orderId]). |
| (6) | BR161 | Finalization Notification Rules:<br>1. Trả về phản hồi 200-OK kèm thông báo thành công MSG57. <br>2. Hệ thống cập nhật lại giao diện người dùng, chuyển trạng thái đơn hàng sang 'Delivered' và vô hiệu hóa (disable) các nút tương tác thay đổi trạng thái khác. |

