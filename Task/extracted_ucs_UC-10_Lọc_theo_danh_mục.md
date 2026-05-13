# UC-10: Lọc theo danh mục

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Lọc theo danh mục |
| Mã Use Case | UC-10 |
| Mô tả Use Case | Khách hàng thu hẹp phạm vi tìm kiếm sản phẩm bằng cách chọn các bộ lọc danh mục cụ thể (ví dụ: Laptop, Điện thoại, Phụ kiện). |
| Kích hoạt | Khách hàng nhấn vào một danh mục cụ thể trên thanh điều hướng hoặc menu bộ lọc bên cạnh (sidebar). |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase “Khám phá sản phẩm” |
| Tiền điều kiện | Khách hàng đang xem danh mục sản phẩm tổng hoặc trang kết quả tìm kiếm. |
| Hậu điều kiện | Danh sách sản phẩm được cập nhật, chỉ hiển thị những mặt hàng thuộc (các) danh mục đã chọn. |
| Luồng sự kiện chính | 1. Khách hàng tích chọn một hoặc nhiều bộ lọc danh mục.<br>2. Hệ thống xử lý yêu cầu lọc đa lớp.<br>3. Hệ thống truy xuất các sản phẩm khớp với điều kiện lọc từ CSDL.<br>4. Hệ thống cập nhật danh sách sản phẩm phù hợp trên giao diện. |
| Luồng sự kiện thay thế | 1a. Khách hàng bỏ tích một bộ lọc: Hệ thống gỡ bỏ điều kiện lọc đó và khôi phục lại danh sách sản phẩm tương ứng. |
| Luồng sự kiện ngoại lệ | - Lỗi truy xuất dữ liệu do rớt mạng: Hệ thống hiển thị thông báo lỗi và yêu cầu người dùng làm mới (refresh) trang. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR43 | Interaction Rules:<br>1. Hệ thống tiếp nhận hành động của người dùng khi tương tác với bộ lọc danh mục. <br>2. Xác định loại tương tác là tích chọn thêm hay bỏ tích để điều hướng luồng xử lý tương ứng. <br>3. Hệ thống nhận mã của các danh mục cần lọc [activeCategoryIds] |
| (3) | BR44 | Add Filter Rules:<br>(Luồng tích chọn thêm)<br>Hệ thống xử lý thêm điều kiện lọc danh mục vừa chọn vào bộ tham số truy vấn hiện tại. |
| (4) | BR45 | Remove Filter Rules:<br>(Luồng bỏ tích)<br>Hệ thống xử lý gỡ bỏ điều kiện lọc danh mục vừa bỏ chọn khỏi bộ tham số truy vấn hiện tại. |
| (5) | BR46 | Query Rules:<br>Hệ thống thực hiện truy xuất cơ sở dữ liệu để tìm các sản phẩm khớp với tập hợp điều kiện lọc hiện tại. (ProductRepository.filterByCategory([activeCategoryIds])) |
| (6) | BR47 | Display Rules:<br>Hệ thống tải lại dữ liệu và cập nhật danh sách sản phẩm trên giao diện theo đúng kết quả vừa truy xuất. |

