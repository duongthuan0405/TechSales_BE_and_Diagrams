# UC-53: Xem báo cáo sản phẩm bán chạy

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Xem báo cáo sản phẩm bán chạy |
| Mã Use Case | UC-53 |
| Mô tả Use Case | Quản lý xem danh sách xếp hạng các sản phẩm có số lượng bán ra cao nhất để đưa ra chiến lược nhập hàng hoặc tiếp thị phù hợp. |
| Kích hoạt | Quản lý chọn mục "Sản phẩm bán chạy" trong phân hệ báo cáo. |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Quản lý đã đăng nhập thành công. |
| Hậu điều kiện | Danh sách các sản phẩm bán chạy nhất được hiển thị theo thứ tự giảm dần. |
| Luồng sự kiện chính | 1. Quản lý điều hướng đến trang báo cáo sản phẩm.<br>2. Quản lý chọn khoảng thời gian cần phân tích.<br>3. Hệ thống truy vấn dữ liệu từ các đơn hàng đã hoàn tất.<br>4. Hệ thống tính toán tổng số lượng đã bán cho từng mặt hàng và sắp xếp giảm dần.<br>5. Hệ thống hiển thị danh sách xếp hạng lên giao diện. |
| Luồng sự kiện thay thế | 3a. Không có sản phẩm nào được bán ra trong thời gian chọn: Hệ thống hiển thị bảng danh sách trống kèm thông báo. |
| Luồng sự kiện ngoại lệ | - Dữ liệu quá lớn gây quá thời gian xử lý (Timeout): Hệ thống hiển thị thông báo lỗi và gợi ý quản lý thu hẹp lại khoảng thời gian cần xem. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR277 | Loading & Filter Rules:<br>1. Tải màn hình BestSellingProducts_Report. <br>2. Hệ thống yêu cầu bộ lọc khoảng thời gian: [startDate], [endDate]. |
| (2) | BR278 | Validate Date Rules:<br>1. If isEmpty([startDate]) OR isEmpty([endDate]) then returns 400-BAD_REQUEST error with MSG1.<br>2. If [endDate] < [startDate] then returns 400-BAD_REQUEST error with MSG101. |
| (3) | BR279 | Data Retrieval Rules:<br>1. [orderItems] = OrderRepository.findDeliveredOrderItems([startDate], [endDate]). <br>2. Chỉ truy vấn các sản phẩm thuộc đơn hàng đã hoàn tất (trạng thái Delivered/Completed) để đảm bảo tính chính xác của dữ liệu thực thu. |
| (4) | BR280 | Calculation & Ranking Rules:<br>1. [rankingList] = [orderItems].GroupBy(i => i.ProductId).Select(g => new { ProductName = g.First().Name, TotalSold = g.Sum(x => x.Quantity), Revenue = g.Sum(x => x.UnitPrice * x.Quantity) }). <br>2. Thực hiện sắp xếp giảm dần: [rankingList].OrderByDescending(x => x.TotalSold). |
| (5) | BR281 | Display Rules (Has Data):<br>1. if [rankingList].Count > 0 then hiển thị danh sách Top sản phẩm lên giao diện bảng kèm các cột: Hạng, Tên sản phẩm, Số lượng đã bán, Doanh thu đóng góp. |
| (6) | BR282 | Empty State Rules:<br>1. if [rankingList].Count == 0 then trả về 200-OK với bảng trống và thông báo MSG103. |

