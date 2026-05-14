# UC-14: Thêm sản phẩm vào giỏ hàng

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Thêm sản phẩm vào giỏ hàng |
| Mã Use Case | UC-14 |
| Mô tả Use Case | Khách hàng lưu trữ các sản phẩm mong muốn vào giỏ hàng cá nhân để có thể tiếp tục duyệt và mua sắm các mặt hàng khác. |
| Kích hoạt | Khách hàng nhấn nút "Thêm vào giỏ hàng" (Add to Cart) trên trang sản phẩm. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Sản phẩm đang ở trạng thái còn hàng. |
| Hậu điều kiện | Sản phẩm được ghi nhận vào giỏ hàng. Dữ liệu giỏ hàng được lưu trữ cố định (persistent) ngay cả khi khách hàng đóng trình duyệt. |
| Luồng sự kiện chính | 1. Khách hàng chọn số lượng mong muốn và nhấn nút "Thêm vào giỏ hàng".<br>2. Hệ thống xác thực số lượng yêu cầu so với lượng hàng tồn kho hiện có.<br>3. Hệ thống thêm sản phẩm vào dữ liệu giỏ hàng của người dùng.<br>4. Hệ thống cập nhật bộ đếm số lượng trên biểu tượng giỏ hàng (Cart icon).<br>5. Hệ thống hiển thị popup thông báo thêm vào giỏ thành công. |
| Luồng sự kiện thay thế | 2a. Số lượng yêu cầu vượt quá tồn kho hiện tại: Hệ thống hiển thị cảnh báo và tự động điều chỉnh số lượng khách muốn mua xuống mức tối đa có sẵn. |
| Luồng sự kiện ngoại lệ | - Lỗi cơ sở dữ liệu hệ thống: Hệ thống hiển thị thông báo lỗi và sản phẩm không được thêm vào giỏ. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (2) | BR57 | Quantity Validation Rules:<br>1. Hệ thống tiếp nhận mã sản phẩm [productId] và số lượng yêu cầu [requestedQuantity]. <br>2. Lấy số lượng hàng khả dụng: [availableQuantity] = InventoryRepository.getRealTimeStock([productId]). <br>3. If [requestedQuantity] > [availableQuantity] thì chuyển sang Activity (3). <br>4. Else chuyển sang Activity (4). |
| (3) | BR58 | Insufficient Stock Handling Rules:<br>1. Trả về thông báo lỗi MSG27 để cảnh báo khách hàng về việc thiếu hàng. <br>2. Tự động điều chỉnh số lượng trên giao diện về mức tối đa có thể đáp ứng: [inputQuantityField].setValue([availableQuantity]). |
| (4) & (5) | BR59 | Cart Storage & Counter Rules:<br>1. Thêm sản phẩm vào giỏ hàng hiện tại của người dùng (trong Session hoặc Database): CartRepository.addToCart([productId], [requestedQuantity]). <br>2. Tính toán lại tổng số lượng trong giỏ hàng: [cartBadgeCount] = CartRepository.getTotalItemsCount(). <br>3. Cập nhật số hiển thị trên biểu tượng giỏ hàng ở Header. |
| (6) | BR60 | Success Notification Rules:<br>1. Trả về phản hồi thành công và hiển thị thông báo MSG28 trên giao diện. |

