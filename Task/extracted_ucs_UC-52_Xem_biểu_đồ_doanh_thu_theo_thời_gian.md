# UC-52: Xem biểu đồ doanh thu theo thời gian

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Xem biểu đồ doanh thu theo thời gian |
| Mã Use Case | UC-52 |
| Mô tả Use Case | Quản lý theo dõi dữ liệu doanh thu bán hàng được biểu diễn trực quan dưới dạng biểu đồ để đánh giá hiệu quả kinh doanh của nền tảng. |
| Kích hoạt | Quản lý chọn mục "Báo cáo doanh thu" trên thanh điều hướng. |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Quản lý đã đăng nhập vào phân hệ nội bộ. |
| Hậu điều kiện | Biểu đồ doanh thu tương ứng với khoảng thời gian đã chọn được hiển thị chính xác. |
| Luồng sự kiện chính | 1. Quản lý truy cập vào trang báo cáo doanh thu.<br>2. Quản lý thiết lập bộ lọc khoảng thời gian (ví dụ: theo tuần, tháng, năm).<br>3. Hệ thống truy vấn các đơn hàng ở trạng thái "Đã giao" (Delivered) trong khoảng thời gian đó.<br>4. Hệ thống tổng hợp số liệu và kết xuất dữ liệu thành biểu đồ trực quan.<br>5. Hệ thống hiển thị biểu đồ và các chỉ số tóm tắt (tổng doanh thu, tổng đơn hàng) lên màn hình. |
| Luồng sự kiện thay thế | 3a. Không có đơn hàng thành công nào trong khoảng thời gian đã chọn: Hệ thống hiển thị biểu đồ rỗng kèm theo thông báo không có dữ liệu để hiển thị. |
| Luồng sự kiện ngoại lệ | - Lỗi truy xuất cơ sở dữ liệu: Hệ thống hiển thị cảnh báo sự cố kỹ thuật và yêu cầu thử lại. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR271 | Loading & Filter Rules:<br>1. Tải màn hình RevenueReport_Screen. <br>2. Hệ thống yêu cầu bộ lọc khoảng thời gian: [startDate], [endDate].<br>3. Mặc định: startDate là ngày đầu tháng hiện tại, endDate là ngày hiện tại. |
| (2) | BR272 | Validate Date Rules:<br>1. If isEmpty([startDate]) OR isEmpty([endDate]) then returns 400-BAD_REQUEST error with MSG1.<br>2. If [endDate] < [startDate] then returns 400-BAD_REQUEST error with MSG101. |
| (3) | BR273 | Data Retrieval Rules:<br>1. [orderList] = OrderRepository.findOrdersByStatusAndDate('DELIVERED', [startDate], [endDate]). <br>2. Chỉ các đơn hàng đã giao thành công (Delivered) mới được tính vào doanh thu thực tế. |
| (4) | BR274 | Aggregation Rules:<br>1. [totalRevenue] = sum([orderList.totalPrice]).<br>2. [chartData] = [orderList].GroupBy(o => o.OrderDate.Date).Select(g => new { Date = g.Key, Value = g.Sum(x => x.TotalPrice) }). |
| (5) | BR275 | Display Rules (Has Data):<br>1. if [orderList].Count > 0 then kết xuất dữ liệu lên biểu đồ (Line/Bar Chart) kèm các chỉ số tóm tắt: Tổng doanh thu, Số đơn hàng hoàn tất. |
| (6) | BR276 | Empty State Rules:<br>1. if [orderList].Count == 0 then returns 200-OK với biểu đồ rỗng và thông báo MSG102. |

