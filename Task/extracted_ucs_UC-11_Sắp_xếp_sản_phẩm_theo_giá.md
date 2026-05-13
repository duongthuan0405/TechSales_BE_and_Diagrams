# UC-11: Sắp xếp sản phẩm theo giá

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Sắp xếp sản phẩm theo giá |
| Mã Use Case | UC-11 |
| Mô tả Use Case | Cho phép khách hàng thay đổi thứ tự hiển thị của danh sách sản phẩm dựa trên mức giá để dễ dàng tìm được thiết bị phù hợp với ngân sách. |
| Kích hoạt | Khách hàng chọn một tiêu chí sắp xếp (ví dụ: Giá: Thấp đến Cao) trên trang danh sách sản phẩm. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Khách hàng đang xem danh mục sản phẩm hoặc trang kết quả tìm kiếm. |
| Hậu điều kiện | Danh sách sản phẩm được sắp xếp lại dựa trên tiêu chí giá đã chọn. |
| Luồng sự kiện chính | 1. Khách hàng nhấn vào menu thả xuống để sắp xếp (Sort).<br>2. Khách hàng chọn một tùy chọn sắp xếp theo giá.<br>3. Hệ thống xử lý yêu cầu sắp xếp dựa trên dữ liệu sản phẩm.<br>4. Hệ thống cập nhật và hiển thị lại danh sách sản phẩm theo đúng thứ tự. |
| Luồng sự kiện thay thế | Không có. |
| Luồng sự kiện ngoại lệ | - Lỗi mạng không thể tải dữ liệu: Hệ thống hiển thị thông báo lỗi và yêu cầu tải lại trang. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (2) | BR48 | Sort Processing Rules:<br>1. Hệ thống tiếp nhận tiêu chí sắp xếp theo giá từ yêu cầu của khách hàng: [sortOrder] (có thể là 'ASC' cho tăng dần hoặc 'DESC' cho giảm dần). <br>2. Thực hiện truy xuất và sắp xếp danh sách sản phẩm dựa trên tiêu chí hiện tại: [sortedProductList] = ProductRepository.getProductsSortedByPrice([sortOrder]). |
| (3) | BR49 | Display Rules:<br>1. Hệ thống làm mới giao diện và hiển thị dữ liệu [sortedProductList] lên màn hình danh sách sản phẩm theo đúng thứ tự mới. |

