# UC-36: Tìm kiếm đơn hàng

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Tìm kiếm đơn hàng |
| Mã Use Case | UC-36 |
| Mô tả Use Case | Nhân viên Sales định vị nhanh một đơn hàng cụ thể bằng mã số đơn hoặc số điện thoại của khách để hỗ trợ tra cứu thông tin kịp thời. |
| Kích hoạt | Nhân viên nhập từ khóa vào thanh tìm kiếm tại màn hình quản lý đơn hàng và nhấn tìm kiếm. |
| Actors | Nhân viên Sales (Sales Staff) |
| Use Case liên quan | Usecase "Xem chi tiết đơn hàng" |
| Tiền điều kiện | Nhân viên đang ở màn hình danh sách đơn hàng. |
| Hậu điều kiện | Hệ thống hiển thị kết quả các đơn hàng khớp với từ khóa tìm kiếm. |
| Luồng sự kiện chính | 1. Nhân viên nhập mã đơn hàng hoặc số điện thoại vào ô tìm kiếm.<br>2. Hệ thống tiếp nhận yêu cầu và truy vấn cơ sở dữ liệu.<br>3. Hệ thống lọc ra các đơn hàng có thông tin trùng khớp.<br>4. Hệ thống hiển thị danh sách kết quả lên màn hình. |
| Luồng sự kiện thay thế | 3a. Không có đơn hàng nào khớp với dữ liệu tìm kiếm: Hệ thống hiển thị thông báo không tìm thấy kết quả. |
| Luồng sự kiện ngoại lệ | - Lỗi truy xuất cơ sở dữ liệu: Hệ thống hiển thị thông báo lỗi hệ thống và yêu cầu nhân viên thử lại. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR179 | Loading & Input Rules:<br>1. Hệ thống tiếp nhận từ khóa tìm kiếm từ ô nhập liệu tại trang quản trị đơn hàng. <br>2. Tham số yêu cầu từ client: [searchKeyword]. |
| (3) | BR180 | Query Logic Rules:<br>1. Hệ thống thực hiện truy vấn trong CSDL theo cơ chế so khớp chính xác hoặc gần đúng (Like query). <br>2. Điều kiện tìm kiếm: [order.id] == [searchKeyword] OR [customer.phoneNumber] == [searchKeyword]. |
| (5) | BR181 | Search Success Rules:<br>1. Trả về phản hồi 200-OK kèm danh sách [matchingOrders]. <br>2. Hiển thị danh sách kết quả dưới dạng bảng, làm nổi bật từ khóa tìm kiếm trong kết quả (nếu có). |
| (6) | BR182 | Empty Result Rules:<br>1. Trả về phản hồi 200-OK kèm mảng dữ liệu rỗng. <br>2. Hiển thị thông báo không tìm thấy kết quả phù hợp: MSG65. |

