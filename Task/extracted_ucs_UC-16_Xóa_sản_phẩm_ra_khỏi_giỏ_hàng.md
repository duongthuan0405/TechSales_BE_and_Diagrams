# UC-16: Xóa sản phẩm ra khỏi giỏ hàng

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Xóa sản phẩm khỏi giỏ hàng |
| Mã Use Case | UC-16 |
| Mô tả Use Case | Khách hàng loại bỏ các sản phẩm không còn nhu cầu mua ra khỏi giỏ hàng hiện tại. |
| Kích hoạt | Khách hàng nhấn vào biểu tượng "Xóa" hoặc "Thùng rác" bên cạnh một sản phẩm trong giỏ hàng. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Khách hàng đang ở trang giỏ hàng và có ít nhất 1 sản phẩm trong giỏ. |
| Hậu điều kiện | Sản phẩm được xóa khỏi cơ sở dữ liệu giỏ hàng, tổng tiền giỏ hàng được cập nhật lại. |
| Luồng sự kiện chính | 1. Khách hàng nhấn biểu tượng xóa tại một sản phẩm cụ thể.<br>2. Hệ thống yêu cầu xác nhận hành động xóa.<br>3. Khách hàng đồng ý xác nhận.<br>4. Hệ thống xóa dữ liệu sản phẩm tương ứng khỏi giỏ hàng trong cơ sở dữ liệu.<br>5. Hệ thống tính toán lại và cập nhật tổng tiền.<br>6. Hệ thống hiển thị thông báo thành công. |
| Luồng sự kiện thay thế | 3a. Khách hàng từ chối xác nhận: Hệ thống đóng hộp thoại và giữ nguyên trạng thái giỏ hàng. |
| Luồng sự kiện ngoại lệ | - Lỗi cập nhật cơ sở dữ liệu: Hệ thống hiển thị thông báo lỗi và yêu cầu khách hàng thử lại thao tác. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR68 | Loading & Confirmation Rules:<br>1. Hệ thống tiếp nhận tương tác nhấn nút xóa tại cartScreen. <br>2. Trường dữ liệu yêu cầu từ client: [productId]. <br>3. Hệ thống hiển thị hộp thoại xác nhận với nội dung: MSG30. |
| (3) | BR69 | Decision Rules:<br>1. Nếu khách hàng chọn "Có" (Confirm): Chuyển sang Activity (4). <br>2. Nếu khách hàng chọn "Không" (Cancel): Chuyển sang Activity (7). |
| (4), (5) & (6) | BR70 | Deletion & Recalculation Rules:<br>1. Hệ thống thực hiện xóa sản phẩm khỏi cơ sở dữ liệu: CartRepository.removeItem([productId]). <br>2. Tính toán lại tổng giá trị giỏ hàng: [totalCartPrice] = CartRepository.calculateTotal(). <br>3. Trả về phản hồi 200-OK kèm thông báo thành công MSG31. <br>4. Cập nhật lại giao diện cartScreen với danh sách sản phẩm và tổng tiền mới. |
| (7) | BR71 | Cancellation Rules:<br>1. Hệ thống đóng hộp thoại xác nhận. <br>2. Không thực hiện bất kỳ thay đổi nào đối với dữ liệu giỏ hàng trong CSDL. |

