# UC-09: Tìm kiếm sản phẩm bằng từ khóa

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Tìm kiếm sản phẩm bằng từ khóa |
| Mã Use Case | UC-09 |
| Mô tả Use Case | Cho phép khách hàng nhanh chóng tìm thấy các thiết bị công nghệ mong muốn bằng cách nhập từ khóa, hỗ trợ gợi ý tự động theo thời gian thực. |
| Kích hoạt | Khách hàng gõ từ khóa vào thanh tìm kiếm và nhấn Enter hoặc biểu tượng kính lúp. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase “Khám phá sản phẩm” |
| Tiền điều kiện | Hệ thống đang hoạt động bình thường (Không yêu cầu đăng nhập). |
| Hậu điều kiện | Hệ thống hiển thị danh sách phân trang các sản phẩm khớp với từ khóa tìm kiếm. |
| Luồng sự kiện chính | 1. Khách hàng gõ từ khóa vào thanh tìm kiếm.<br>2. Hệ thống cung cấp các gợi ý hoàn thiện tự động (autocomplete) theo thời gian thực.<br>3. Khách hàng gửi yêu cầu tìm kiếm.<br>4. Hệ thống truy vấn cơ sở dữ liệu để tìm các sản phẩm khớp.<br>5. Hệ thống hiển thị kết quả tìm kiếm được. |
| Luồng sự kiện thay thế | 4a. Không có sản phẩm nào khớp với từ khóa: Hệ thống hiển thị thông báo không tìm thấy kết quả |
| Luồng sự kiện ngoại lệ | - Lỗi mạng hoặc quá thời gian phản hồi (timeout): Hệ thống hiển thị thông báo yêu cầu người dùng thử lại sau. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (2) | BR39 | Autocomplete Rules:<br>1. Hệ thống tiếp nhận chuỗi ký tự khi người dùng gõ từ khóa vào thanh tìm kiếm. <br>2. Truy vấn nhanh và cung cấp danh sách gợi ý tự động (Autocomplete) trên giao diện. |
| (4) | BR40 | Search Query Rules:<br>(Luồng xử lý khi người dùng gửi yêu cầu tìm kiếm)<br>1. Hệ thống nhận từ khóa hoàn chỉnh và thực hiện truy vấn tìm các sản phẩm khớp. <br>2. [searchResults] = ProductRepository.searchByKeyword([keyword])<br>3. If [searchResults].isEmpty() then proceed to BR41 else proceed to BR42 |
| (6) | BR41 | Message Rules (No Result):<br>Nếu truy vấn không có sản phẩm nào khớp: Trả về trạng thái phản hồi hợp lệ (200-OK kèm mảng rỗng) và hiển thị thông báo không tìm thấy kết quả: MSG24. |
| (5) | BR42 | Display Rules (Success):<br>Nếu truy vấn có sản phẩm khớp: Trả về dữ liệu và hiển thị danh sách sản phẩm phù hợp lên màn hình kết quả tìm kiếm searchResultsScreen. |

