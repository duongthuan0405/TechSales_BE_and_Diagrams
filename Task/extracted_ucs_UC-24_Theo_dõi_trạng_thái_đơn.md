# UC-24: Theo dõi trạng thái đơn

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Theo dõi trạng thái đơn hàng |
| Mã Use Case | UC-24 |
| Mô tả Use Case | Khách hàng theo dõi tiến trình xử lý và vận chuyển của đơn hàng theo dòng thời gian thực tế để biết khi nào sẽ nhận được kiện hàng. |
| Kích hoạt | Tự động kích hoạt và hiển thị khi khách hàng đang xem trang chi tiết của một đơn hàng. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase "Xem chi tiết đơn hàng cũ" |
| Tiền điều kiện | Đơn hàng đang hiển thị ở màn hình chi tiết. |
| Hậu điều kiện | Dòng thời gian (timeline) các mốc thay đổi trạng thái của đơn hàng được hiển thị. |
| Luồng sự kiện chính | 1. Hệ thống truy xuất lịch sử cập nhật trạng thái của đơn hàng hiện tại.<br>2. Hệ thống ánh xạ các dữ liệu này thành một biểu đồ dòng thời gian trực quan.<br>3. Hệ thống hiển thị rõ ràng các mốc trạng thái (ví dụ: Chờ xử lý, Đã duyệt, Đang giao, Đã giao) và làm nổi bật trạng thái hiện tại kèm theo mốc thời gian cập nhật gần nhất. |
| Luồng sự kiện thay thế | Không có. |
| Luồng sự kiện ngoại lệ | Không có. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR114 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu theo dõi trạng thái đơn hàng thông qua việc kế thừa dữ liệu từ UC-23. <br>2. Nguồn dữ liệu yêu cầu: [orderId]. |
| (2) | BR115 | Status History Retrieval Rules:<br>1. Hệ thống thực hiện truy xuất toàn bộ lịch sử thay đổi trạng thái của đơn hàng: [statusHistory] = OrderRepository.getStatusHistory([orderId]). <br>2. Nếu [statusHistory] == null then returns 404-NOT_FOUND kèm MSG43 (Thông báo lỗi truy xuất dữ liệu). |
| (3) | BR116 | Data Mapping Rules:<br>1. Hệ thống thực hiện ánh xạ (map) dữ liệu từ danh sách lịch sử sang cấu trúc biểu đồ dòng thời gian: [timelineData] = mapToTimeline([statusHistory]). <br>2. Mỗi mốc trạng thái phải bao gồm: [statusName], [updatedAt], và [description]. |
| (4) | BR117 | Timeline Display Rules:<br>1. Xác định trạng thái mới nhất để làm nổi bật: [currentStatus] = [statusHistory].latest().status. <br>2. Trả về phản hồi 200-OK kèm dữ liệu [timelineData] và [currentStatus]. <br>3. Hệ thống hiển thị các mốc thời gian và làm nổi bật trạng thái hiện tại của đơn hàng trên giao diện orderDetailScreen. |

