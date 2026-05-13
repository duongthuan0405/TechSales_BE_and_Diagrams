# UC-22: Xem lịch sử đơn hàng

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Xem lịch sử đơn hàng |
| Mã Use Case | UC-22 |
| Mô tả Use Case | Khách hàng xem lại danh sách toàn bộ các giao dịch mua sắm trên hệ thống trong quá khứ và hiện tại để tiện quản lý. |
| Kích hoạt | Khách hàng nhấn vào mục "Lịch sử đơn hàng" hoặc "Quản lý đơn hàng" trong trang tài khoản cá nhân. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Khách hàng đã đăng nhập vào hệ thống. |
| Hậu điều kiện | Danh sách các đơn hàng được hiển thị đầy đủ và sắp xếp theo trình tự thời gian. |
| Luồng sự kiện chính | 1. Khách hàng chọn chức năng xem lịch sử đơn hàng.<br>2. Hệ thống truy vấn cơ sở dữ liệu để lấy danh sách tất cả các đơn hàng được gắn với mã tài khoản của khách hàng này.<br>3. Hệ thống sắp xếp danh sách theo thời gian tạo (mới nhất xếp trước) và thực hiện phân trang.<br>4. Hệ thống hiển thị danh sách lên màn hình (bao gồm mã đơn, ngày tạo, tổng tiền và trạng thái hiện tại). |
| Luồng sự kiện thay thế | 2a. Khách hàng chưa từng mua hàng (danh sách trống): Hệ thống hiển thị thông báo không có đơn hàng nào và gợi ý một số sản phẩm nổi bật để khuyến khích mua sắm. |
| Luồng sự kiện ngoại lệ | - Lỗi kết nối máy chủ: Hệ thống hiển thị cảnh báo lỗi tải dữ liệu và yêu cầu tải lại trang. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR106 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu truy cập lịch sử đơn hàng từ giao diện người dùng. <br>2. Các trường dữ liệu yêu cầu từ client: [userId], [pageNumber] (mặc định là 1), [pageSize] (mặc định là 10). |
| (2) | BR107 | Order Query Rules:<br>1. Hệ thống thực hiện truy vấn danh sách đơn hàng của người dùng: [orderHistoryList] = OrderRepository.findByUserId([userId]). <br>2. If [orderHistoryList].isEmpty() then chuyển sang Activity (3). <br>3. Else chuyển sang Activity (4). |
| (3) | BR108 | Empty State & Suggestion Rules:<br>1. Trả về phản hồi 200-OK kèm mảng rỗng và MSG42. <br>2. Hiển thị thông báo danh sách trống và các sản phẩm gợi ý trên giao diện. |
| (4) & (5) | BR109 | Sorting, Pagination & Display Rules:<br>1. Hệ thống thực hiện sắp xếp danh sách theo thời gian tạo giảm dần (mới nhất lên đầu): [sortedOrders] = [orderHistoryList].sortBy('createdAt', 'DESC'). <br>2. Thực hiện phân trang dữ liệu dựa trên [pageNumber] và [pageSize]. <br>3. Trả về phản hồi 200-OK kèm dữ liệu [paginatedOrders]. <br>4. Hiển thị danh sách lịch sử đơn hàng lên màn hình orderHistoryScreen. |

