# UC-15: Thay đổi số lượng trong giỏ

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Thay đổi số lượng trong giỏ |
| Mã Use Case | UC-15 |
| Mô tả Use Case | Cho phép khách hàng điều chỉnh số lượng của các mặt hàng đã thêm trong giỏ hàng trước khi tiến hành thanh toán. |
| Kích hoạt | Khách hàng thay đổi con số trong ô nhập liệu hoặc nhấn nút tăng/giảm tại giao diện giỏ hàng. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Khách hàng đang ở trang giỏ hàng và có ít nhất 1 sản phẩm bên trong. |
| Hậu điều kiện | Số lượng sản phẩm được cập nhật, tổng giá trị giỏ hàng được tính toán lại chính xác. |
| Luồng sự kiện chính | 1. Khách hàng thay đổi số lượng của một mặt hàng cụ thể.<br>2. Hệ thống xác thực dữ liệu đầu vào.<br>3. Hệ thống kiểm tra số lượng tồn kho xem có đáp ứng được mức thay đổi mới không.<br>4. Hệ thống cập nhật lại số lượng trong cơ sở dữ liệu giỏ hàng.<br>5. Hệ thống tính toán lại tổng tiền giỏ hàng ngay lập tức mà không cần tải lại toàn bộ trang. |
| Luồng sự kiện thay thế | 2a. Khách hàng nhập số lượng không hợp lệ (ví dụ: nhập số âm, số 0 hoặc chữ cái): Hệ thống cảnh báo và tự động khôi phục về con số hợp lệ trước đó.<br>3a. Số lượng mới vượt quá mức tồn kho cho phép: Hệ thống hiển thị thông báo số lượng vượt quá mức tồn kho và cập nhật số lượng về số lượng tối đa đó. |
| Luồng sự kiện ngoại lệ | - Mất kết nối mạng khi đang thao tác: Hệ thống hiển thị lỗi kết nối và giữ nguyên trạng thái giỏ hàng cũ. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR62 | Loading Rules:<br>1. Hệ thống tiếp nhận tương tác tại cartScreen. <br>2. Các trường dữ liệu yêu cầu từ client: [productId], [newQuantity], [oldQuantity]. |
| (2) | BR63 | Data Format Validation Rules:<br>1. Kiểm tra định dạng của [newQuantity]. <br>2. If [newQuantity] không phải là số nguyên dương OR isEmpty([newQuantity]) then returns 400-BAD_REQUEST kèm MSG29. <br>3. Else chuyển sang Activity (4). |
| (3) | BR64 | Message Rules (Invalid Format):<br>Hiển thị thông báo lỗi định dạng không hợp lệ: MSG29 và thực hiện [quantityInput].setValue([oldQuantity]) trên giao diện. |
| (4) | BR65 | Stock Validation Rules:<br>1. Truy vấn tồn kho thực tế: [availableQuantity] = InventoryRepository.getRealTimeStock([productId]). <br>2. If [newQuantity] > [availableQuantity] then returns 400-BAD_REQUEST kèm MSG27. <br>3. Else chuyển sang Activity (6). |
| (5) | BR66 | Insufficient Stock Handling Rules:<br>Hiển thị thông báo lỗi vượt quá tồn kho: MSG27. Đồng thời cập nhật CartRepository.updateQuantity([productId], [availableQuantity]) và hiển thị mức tối đa lên giao diện. |
| (6) & (7) | BR67 | Update & Recalculate Rules:<br>1. CartRepository.updateQuantity([productId], [newQuantity]). <br>2. [totalCartPrice] = CartRepository.calculateTotal(). <br>3. Returns 200-OK kèm dữ liệu giỏ hàng đã cập nhật. <br>4. Hệ thống cập nhật lại tổng tiền hiển thị trên cartScreen. |

