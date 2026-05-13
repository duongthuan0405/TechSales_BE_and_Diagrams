# UC-29: Xem danh sách đơn hàng trạng thái “Pending”

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Xem danh sách đơn hàng "Pending" |
| Mã Use Case | UC-29 |
| Mô tả Use Case | Nhân viên bán hàng truy cập phân hệ quản trị để xem danh sách các đơn hàng mới đến cần được xác minh và xử lý đóng gói. |
| Kích hoạt | Nhân viên chọn mục "Quản lý đơn hàng" trên thanh menu của trang quản trị. |
| Actors | Nhân viên Sales (Sales Staff) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Nhân viên Sales đã đăng nhập thành công vào phân hệ quản trị (Back-office). |
| Hậu điều kiện | Danh sách đơn hàng đang chờ xử lý được hiển thị. |
| Luồng sự kiện chính | 1. Nhân viên điều hướng đến giao diện quản lý đơn hàng.<br>2. Hệ thống mặc định thiết lập bộ lọc trạng thái là "Pending" (Chờ xử lý).<br>3. Hệ thống truy vấn CSDL để lấy danh sách các đơn hàng tương ứng.<br>4. Hệ thống hiển thị danh sách (bao gồm mã đơn, ngày đặt, tên khách hàng và tổng tiền) dưới dạng bảng. |
| Luồng sự kiện thay thế | 3a. Không có đơn hàng nào đang ở trạng thái chờ: Hệ thống hiển thị bảng danh sách trống kèm thông báo không có đơn hàng mới. |
| Luồng sự kiện ngoại lệ | - Mất phiên đăng nhập (Session timeout): Hệ thống điều hướng nhân viên về trang đăng nhập và hiển thị cảnh báo yêu cầu đăng nhập lại. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR141 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu từ Nhân viên Sales tại trang quản trị.<br>2. Thiết lập bộ lọc mặc định: [statusFilter] = 'PENDING'.<br>3. Các trường dữ liệu yêu cầu từ client: [userId], [pageNumber] (mặc định là 1), [pageSize] (mặc định là 20). |
| (3) | BR142 | Data Fetching Rules:<br>1. Hệ thống thực hiện truy vấn danh sách đơn hàng theo trạng thái: [pendingOrders] = OrderRepository.findByStatus('PENDING').<br>2. If [pendingOrders].isEmpty() then chuyển sang Activity (4).<br>3. Else chuyển sang Activity (5). |
| (4) | BR143 | Empty Result Rules:<br>1. Trả về phản hồi 200-OK kèm mảng rỗng.<br>2. Hiển thị thông báo không có đơn hàng mới: MSG52.<br>3. Hiển thị bảng dữ liệu trống trên giao diện quản lý đơn hàng. |
| (5) | BR144 | Display Rules:<br>1. Trả về phản hồi 200-OK kèm dữ liệu [pendingOrders] đã được phân trang.<br>2. Hiển thị danh sách dưới dạng bảng với các cột: Order Id, Customer Name, Total Amount, Ordering Time, Status. |

