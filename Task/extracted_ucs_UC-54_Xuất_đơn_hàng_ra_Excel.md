# UC-54: Xuất đơn hàng ra Excel

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Xuất đơn hàng ra Excel |
| Mã Use Case | UC-54 |
| Mô tả Use Case | Quản lý trích xuất dữ liệu danh sách đơn hàng ra tệp định dạng Excel chuẩn để cung cấp cho bộ phận kế toán hoặc lưu trữ ngoại tuyến. |
| Kích hoạt | Quản lý nhấn nút "Xuất Excel" tại màn hình quản lý đơn hàng. |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Danh sách đơn hàng đang hiển thị trên màn hình có ít nhất một bản ghi. |
| Hậu điều kiện | Tệp tin Excel chứa dữ liệu tương ứng được tải xuống thiết bị của quản lý. |
| Luồng sự kiện chính | 1. Quản lý áp dụng các bộ lọc cần thiết (theo ngày, theo trạng thái) và nhấn nút xuất tệp.<br>2. Hệ thống tiếp nhận yêu cầu và tổng hợp dữ liệu tương ứng với bộ lọc hiện tại.<br>3. Hệ thống định dạng dữ liệu và khởi tạo tệp Excel.<br>4. Hệ thống gửi luồng dữ liệu tệp về trình duyệt của quản lý.<br>5. Trình duyệt tự động tải tệp xuống.<br>6. Hệ thống hiển thị thông báo kết xuất thành công. |
| Luồng sự kiện thay thế | Không có. |
| Luồng sự kiện ngoại lệ | - Lỗi thư viện tạo tệp của máy chủ: Hệ thống hiển thị thông báo lỗi không thể tạo tệp và yêu cầu quản lý thử lại sau. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR283 | Input & Filter Rules:<br>1. Hệ thống tiếp nhận bộ lọc dữ liệu từ giao diện: [startDate], [endDate], [orderStatus]. <br>2. if isEmpty([startDate]) OR isEmpty([endDate]) then returns 400-BAD_REQUEST error with MSG1. |
| (2) | BR284 | Data Aggregation Rules:<br>1. Thực hiện truy vấn: [exportData] = OrderRepository.findForExport([startDate], [endDate], [orderStatus]). <br>2. Dữ liệu bao gồm các trường: Mã đơn hàng, Ngày đặt, Khách hàng, Tổng tiền, Trạng thái, Phương thức thanh toán. |
| (3) | BR285 | Excel Initialization Rules:<br>1. if [exportData].Count == 0 then proceeds to Activity (7). <br>2. else sử dụng thư viện (như EPPlus hoặc ClosedXML) để khởi tạo Workbook và ánh xạ dữ liệu vào các ô (Cells) theo định dạng bảng chuẩn. |
| (4) & (5) | BR286 | File Stream Rules:<br>1. Chuyển đổi Workbook thành MemoryStream.<br>2. Thiết lập Header cho Response: Content-Type: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet. <br>3. Thiết lập Content-Disposition: attachment; filename=Orders_Export_{Date}.xlsx để trình duyệt tự động kích hoạt tiến trình tải xuống. |
| (6) | BR287 | Success Notification:<br>1. returns 200-OK kèm MSG104. <br>2. Hiển thị thông báo kết xuất thành công trên giao diện quản trị. |
| (7) | BR288 | Error Handling Rules:<br>1. returns 404-NOT_FOUND error với MSG105. <br>2. Hiển thị thông báo lỗi không thể tạo tệp do không có dữ liệu phù hợp với bộ lọc. |

