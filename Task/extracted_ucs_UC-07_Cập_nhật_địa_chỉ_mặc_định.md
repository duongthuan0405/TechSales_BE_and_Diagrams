# UC-07: Cập nhật địa chỉ mặc định

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Cập nhật địa chỉ mặc định |
| Mã Use Case | UC-07 |
| Mô tả Use Case | Khách hàng sửa đổi địa chỉ giao hàng mặc định hiện tại để đảm bảo thông tin nhận hàng luôn chính xác. |
| Kích hoạt | Khách hàng nhấn "Edit" (Chỉnh sửa) trên địa chỉ mặc định hiện tại của họ. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Khách hàng đã đăng nhập và đã có một địa chỉ mặc định được thiết lập. |
| Hậu điều kiện | Địa chỉ mặc định được cập nhật cho các đơn hàng trong tương lai. Dữ liệu lịch sử địa chỉ của các đơn hàng trong quá khứ vẫn được giữ nguyên không thay đổi. |
| Luồng sự kiện chính | 1. Khách hàng nhấn "Edit" trên địa chỉ mặc định.<br>2. Hệ thống hiển thị một biểu mẫu đã được điền sẵn dữ liệu địa chỉ hiện tại.<br>3. Khách hàng sửa đổi các chi tiết và nhấn "Save".<br>4. Hệ thống kiểm tra định dạng dữ liệu đầu vào.<br>5. Hệ thống lưu địa chỉ đã cập nhật vào CSDL.<br>6. Hệ thống hiển thị thông báo thành công. |
| Luồng sự kiện thay thế | 4a. Dữ liệu nhập không hợp lệ: Hệ thống hiển thị cảnh báo và yêu cầu khách hàng sửa lại thông tin. |
| Luồng sự kiện ngoại lệ | - Khách hàng nhấn "Cancel" để hủy các thay đổi: Use case dừng lại và không có dữ liệu nào được lưu. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (2) | BR29 | Loading Rules:<br>1. Hệ thống nhận yêu cầu khi người dùng nhấn "Edit" trên địa chỉ mặc định. <br>2. Tải màn hình editAddressScreen và hiển thị biểu mẫu điền sẵn dữ liệu hiện tại của địa chỉ đó. |
| (4) | BR30 | Validate Data Rules:<br>(Luồng xử lý khi người dùng sửa đổi thông tin và nhấn "Save")<br>1. Kiểm tra định dạng dữ liệu đầu vào. <br>2. If anyIsEmpty([houseNumber], [ward], [province]) then returns 400-BAD_REQUEST error with MSG1. |
| (7) | BR31 | Message Rules (Invalid Data):<br>Nếu dữ liệu không hợp lệ: Hệ thống trả về lỗi và hiển thị thông báo lỗi tương ứng là MSG1. |
| (5) | BR32 | Update Rules:<br>(Luồng khi dữ liệu hợp lệ)<br>1. AddressRepository.save([address]). <br>2. returns 200-OK response with MSG22. |
| (6) | BR33 | Message Rules (Success):<br>Hiển thị thông báo cập nhật địa chỉ thành công trên giao diện: MSG22. |

