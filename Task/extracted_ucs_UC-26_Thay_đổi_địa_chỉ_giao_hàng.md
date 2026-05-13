# UC-26: Thay đổi địa chỉ giao hàng

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Thay đổi địa chỉ giao hàng |
| Mã Use Case | UC-26 |
| Mô tả Use Case | Khách hàng cập nhật lại vị trí nhận hàng cho các đơn hàng đã đặt với điều kiện đơn hàng đó chưa được bàn giao cho đơn vị vận chuyển. |
| Kích hoạt | Khách hàng nhấn nút "Thay đổi địa chỉ" tại màn hình chi tiết đơn hàng. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase "Xem chi tiết đơn hàng cũ" |
| Tiền điều kiện | Đơn hàng đang hiển thị và chưa bước sang trạng thái "Đang giao" (Shipping). |
| Hậu điều kiện | Địa chỉ nhận hàng của đơn đó được cập nhật mới trong hệ thống quản lý. |
| Luồng sự kiện chính | 1. Khách hàng chọn chức năng thay đổi địa chỉ.<br>2. Hệ thống hiển thị danh sách các địa chỉ đã lưu hoặc cho phép nhập địa chỉ mới.<br>3. Khách hàng chọn một địa chỉ phù hợp và xác nhận.<br>4. Hệ thống kiểm tra lại trạng thái thực tế của đơn hàng trong cơ sở dữ liệu.<br>5. Hệ thống cập nhật địa chỉ mới cho bản ghi đơn hàng tương ứng.<br>6. Hệ thống hiển thị thông báo cập nhật thành công và làm mới giao diện. |
| Luồng sự kiện thay thế | 4a. Đơn hàng vừa được chuyển sang trạng thái "Shipping": Hệ thống chặn thao tác, hiển thị cảnh báo lỗi đơn hàng đã được gửi đi và không thể đổi địa chỉ. |
| Luồng sự kiện ngoại lệ | - Lỗi cập nhật cơ sở dữ liệu: Hệ thống thông báo lỗi tạm thời và yêu cầu thử lại sau. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR125 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu thay đổi địa chỉ từ màn hình chi tiết đơn hàng. <br>2. Các trường dữ liệu yêu cầu từ client: [orderId], [userId]. <br>3. Hệ thống thực hiện truy xuất danh sách địa chỉ: [savedAddressList] = AddressRepository.findByUserId([userId]). <br>4. Returns 200-OK kèm dữ liệu [savedAddressList] để hiển thị lên giao diện. |
| (3) | BR126 | Input Rules:<br>Khách hàng thực hiện chọn một địa chỉ mới từ danh sách và nhấn xác nhận: Hệ thống nhận trường dữ liệu [newAddressId]. |
| (4) | BR127 | Status Verification Rules:<br>1. Hệ thống kiểm tra trạng thái vận chuyển thực tế của đơn hàng: [shippingStatus] = OrderRepository.getShippingStatus([orderId]). <br>2. If [shippingStatus] == 'SHIPPED' (Đã được giao cho đơn vị vận chuyển) then returns 400-BAD_REQUEST kèm MSG48. <br>3. Else chuyển sang Activity (5). |
| (5) | BR128 | Update Rules:<br>1. Thực hiện cập nhật thông tin địa chỉ mới vào bản ghi đơn hàng: OrderRepository.updateAddress([orderId], [newAddressId]). <br>2. Returns 200-OK kèm dữ liệu đơn hàng đã cập nhật. |
| (6) | BR129 | Success Message Rules:<br>Hiển thị thông báo thay đổi địa chỉ thành công: MSG47. |
| (7) | BR130 | Error Message Rules:<br>Hiển thị thông báo lỗi không thể thay đổi địa chỉ do đơn hàng đã được gửi đi: MSG48. |

