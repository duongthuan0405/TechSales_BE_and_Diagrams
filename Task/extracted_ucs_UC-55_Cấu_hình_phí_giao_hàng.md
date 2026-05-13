# UC-55: Cấu hình phí giao hàng

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Cấu hình phí giao hàng |
| Mã Use Case | UC-55 |
| Mô tả Use Case | Quản lý thiết lập và điều chỉnh linh hoạt các mức phí vận chuyển áp dụng cho từng vùng miền khác nhau. |
| Kích hoạt | Quản lý nhấn nút "Lưu cấu hình" tại màn hình quản lý phí vận chuyển. |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Quản lý đang ở giao diện thiết lập phí giao hàng. |
| Hậu điều kiện | Bảng phí vận chuyển mới được cập nhật và sẽ áp dụng ngay cho các đơn hàng tạo sau thời điểm này. |
| Luồng sự kiện chính | 1. Quản lý xem danh sách các khu vực vận chuyển hiện tại.<br>2. Quản lý nhập hoặc sửa đổi mức phí tương ứng cho các khu vực mong muốn.<br>3. Quản lý nhấn xác nhận lưu cấu hình.<br>4. Hệ thống kiểm tra dữ liệu đầu vào (ví dụ: phí vận chuyển phải là số không âm).<br>5. Hệ thống cập nhật các mức phí mới vào cơ sở dữ liệu.<br>6. Hệ thống hiển thị thông báo cập nhật thành công. |
| Luồng sự kiện thay thế | 4a. Quản lý nhập giá trị âm hoặc sai định dạng chữ: Hệ thống chặn thao tác lưu và hiển thị cảnh báo dữ liệu không hợp lệ. |
| Luồng sự kiện ngoại lệ | - Lỗi kết nối cơ sở dữ liệu: Hệ thống hiển thị thông báo sự cố và không lưu bất kỳ thay đổi nào. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR289 | Loading Rules:<br>1. Tải màn hình ShippingConfiguration_Screen.<br>2. Thực hiện truy vấn: [areaList] = ShippingRepository.findAllAreas(). |
| (2) & (3) | BR290 | Input & Submission Rules:<br>1. Hệ thống tiếp nhận giá trị phí vận chuyển mới [shippingFee] cho từng khu vực từ Business Admin.<br>2. Nhấn nút xác nhận để gửi danh sách cập nhật. |
| (4) | BR291 | Validate Format Rules:<br>1. if isEmpty([shippingFee]) then returns 400-BAD_REQUEST error with MSG1. <br>2. if [shippingFee] < 0 then returns 400-BAD_REQUEST error with MSG107.<br>3. if pattern.compile('^[0-9]+(.[0-9]{1,2})?$').notMatch([shippingFee]) then returns 400-BAD_REQUEST error with MSG108. |
| (5) | BR292 | Persistence Rules:<br>1. Thực hiện: ShippingRepository.UpdateFees([updatedList]).<br>2. Cập nhật dấu thời gian updatedAt cho các bản ghi thay đổi. |
| (6) | BR293 | Success & Notification Rules:<br>1. returns 200-OK response với MSG106.<br>2. Làm mới giao diện và hiển thị mức phí mới nhất. |
| (7) | BR294 | Error Handling Rules:<br>1. Hiển thị thông báo lỗi tương ứng (MSG107, MSG108) ngay tại dòng dữ liệu bị sai để người dùng điều chỉnh. |

