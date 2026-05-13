# UC-06: Đặt địa giao hàng chỉ mặc định

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Đặt địa chỉ mặc định |
| Mã Use Case | UC-06 |
| Mô tả Use Case | Khách hàng thiết lập một địa chỉ giao hàng cụ thể làm mặc định để hệ thống tự động điền thông tin trong quá trình thanh toán, giúp tiết kiệm thời gian chốt đơn. |
| Kích hoạt | Khách hàng nhấn "Set as default" trên một địa chỉ đã lưu trong sổ địa chỉ. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Khách hàng đã đăng nhập vào hệ thống và có ít nhất một địa chỉ giao hàng đã được lưu. |
| Hậu điều kiện | Địa chỉ được chọn sẽ được đánh dấu là mặc định và tự động áp dụng cho các lần thanh toán tiếp theo. |
| Luồng sự kiện chính | 1. Khách hàng điều hướng đến trang Sổ địa chỉ (Address book).<br>2. Khách hàng chọn một địa chỉ hiện có và nhấn "Set as default".<br>3. Hệ thống cập nhật trạng thái địa chỉ trong cơ sở dữ liệu và gỡ bỏ cờ "mặc định" khỏi địa chỉ trước đó.<br>4. Hệ thống hiển thị thông báo thiết lập thành công. |
| Luồng sự kiện thay thế | Không có. |
| Luồng sự kiện ngoại lệ | - Lỗi cơ sở dữ liệu: Hệ thống hiển thị thông báo lỗi và hành động bị hủy bỏ. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (3) & (4) | BR27 | Update Default Address Rules:<br>(Luồng xử lý khi người dùng chọn một địa chỉ và yêu cầu đặt làm mặc định)<br>1. [oldDefaultAddress] = AddressRepository.findDefaultAddress(getCurrentUser().id)<br>2. If [oldDefaultAddress] != null then [oldDefaultAddress.isDefault] = false<br>3. [selectedAddress] = AddressRepository.findById([selectedAddressId])<br>4. [selectedAddress.isDefault] = true<br>5. AddressRepository.save([oldDefaultAddress], [selectedAddress])<br>6. returns 200-OK response with MSG21. |
| (5) | BR28 | Message Rules (Success):<br>Hiển thị thông báo thiết lập địa chỉ mặc định thành công trên giao diện: MSG21. |

