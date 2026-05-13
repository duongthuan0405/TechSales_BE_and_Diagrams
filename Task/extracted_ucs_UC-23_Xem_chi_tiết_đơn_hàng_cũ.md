# UC-23: Xem chi tiết đơn hàng cũ

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Xem chi tiết đơn hàng cũ |
| Mã Use Case | UC-23 |
| Mô tả Use Case | Khách hàng kiểm tra các thông tin chi tiết cụ thể của một đơn hàng (bao gồm giá cả, số lượng mặt hàng và địa chỉ giao hàng) tại đúng thời điểm chốt đơn. |
| Kích hoạt | Khách hàng nhấn vào mã đơn hàng hoặc nút "Xem chi tiết" tại một đơn hàng cụ thể trong danh sách lịch sử. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase "Xem lịch sử đơn hàng" |
| Tiền điều kiện | Khách hàng đang ở màn hình lịch sử đơn hàng. |
| Hậu điều kiện | Hệ thống hiển thị chi tiết hóa đơn. Các dữ liệu lịch sử này (giá sản phẩm lúc mua, địa chỉ nhận hàng) được giữ nguyên vẹn, không bị ảnh hưởng bởi những cập nhật giá trị hiện tại ở nơi khác. |
| Luồng sự kiện chính | 1. Khách hàng nhấn xem chi tiết một đơn hàng cụ thể.<br>2. Hệ thống truy vấn dữ liệu chi tiết của bản ghi đơn hàng đó từ CSDL.<br>3. Hệ thống hiển thị giao diện chi tiết bao gồm danh sách sản phẩm, mức giá, mã giảm giá đã áp dụng và thông tin người nhận. |
| Luồng sự kiện thay thế | Không có. |
| Luồng sự kiện ngoại lệ | - Dữ liệu đơn hàng không tồn tại hoặc lỗi truy xuất: Hệ thống hiển thị cảnh báo lỗi và đưa người dùng quay lại danh sách tổng. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR110 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu xem chi tiết của một đơn hàng cụ thể từ màn hình orderHistoryScreen. <br>2. Trường dữ liệu yêu cầu từ client: [orderId]. |
| (2) | BR111 | Order Detail Query Rules:<br>1. Hệ thống thực hiện truy vấn dữ liệu chi tiết: [orderDetails] = OrderRepository.findDetailsByOrderId([orderId]). <br>2. If [orderDetails] == null then returns 404-NOT_FOUND kèm MSG43. <br>3. Else chuyển sang Activity (4). |
| (3) | BR112 | Message & Redirect Rules (Not Found):<br>1. Hiển thị thông báo lỗi truy xuất: MSG43. <br>2. Thực hiện điều hướng người dùng quay trở lại màn hình danh sách tổng orderHistoryScreen. |
| (4) | BR113 | Success Display Rules:<br>1. Trả về phản hồi 200-OK kèm dữ liệu [orderDetails]. <br>2. Hệ thống tải màn hình orderDetailScreen và hiển thị đầy đủ thông tin đơn hàng bao gồm: mã đơn hàng, trạng thái, danh sách sản phẩm, đơn giá, phí vận chuyển và tổng tiền. |

