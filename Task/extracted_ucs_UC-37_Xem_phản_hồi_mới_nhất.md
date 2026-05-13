# UC-37: Xem phản hồi mới nhất

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Xem phản hồi mới nhất |
| Mã Use Case | UC-37 |
| Mô tả Use Case | Nhân viên Sales theo dõi danh sách các đánh giá, bình luận mới nhất từ khách hàng (đặc biệt là các đánh giá tiêu cực) để kịp thời xử lý vấn đề dịch vụ. |
| Kích hoạt | Nhân viên chọn mục "Quản lý đánh giá/Phản hồi" trên thanh điều hướng nội bộ. |
| Actors | Nhân viên Sales (Sales Staff) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Nhân viên Sales đã đăng nhập vào hệ thống. |
| Hậu điều kiện | Danh sách các bài đánh giá được hiển thị theo thứ tự thời gian. |
| Luồng sự kiện chính | 1. Nhân viên điều hướng đến giao diện quản lý đánh giá.<br>2. Hệ thống truy xuất cơ sở dữ liệu để lấy danh sách các đánh giá mới nhất.<br>3. Hệ thống hiển thị danh sách (bao gồm tên khách, sản phẩm, số sao, nội dung và trạng thái).<br>4. Nhân viên có thể sử dụng bộ lọc để chỉ hiển thị các đánh giá 1-2 sao. |
| Luồng sự kiện thay thế | 2a. Chưa có bài đánh giá nào trên hệ thống: Hệ thống hiển thị danh sách trống kèm thông báo. |
| Luồng sự kiện ngoại lệ | - Lỗi tải dữ liệu: Hệ thống hiển thị thông báo sự cố và yêu cầu tải lại trang. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR183 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu truy cập từ menu "Quản lý đánh giá/Phản hồi" tại Dashboard quản trị. <br>2. Tham số yêu cầu mặc định: [pageNumber] = 1, [pageSize] = 20, [sortBy] = 'createdAt', [sortOrder] = 'DESC'. |
| (2) | BR184 | Review Retrieval Rules:<br>1. Hệ thống thực hiện truy vấn: [feedbackList] = ReviewRepository.findAllLatest(). <br>2. If [feedbackList].isEmpty() then chuyển sang Activity (7). <br>3. Else chuyển sang Activity (4). |
| (4) | BR185 | Chronological Display Rules:<br>1. Trả về phản hồi 200-OK kèm danh sách đã sắp xếp. <br>2. Hiển thị danh sách phản hồi bao gồm: Full Name, Product ID, Product Name, Rating, Content và Created At. |
| (5) & (6) | BR186 | Star Filtering Rules:<br>1. Khi nhân viên thay đổi bộ lọc [starFilter], hệ thống thực hiện truy vấn lại: ReviewRepository.findByStars([starFilter]). <br>2. Cập nhật lại giao diện danh sách mà không cần tải lại toàn bộ trang (Partial Update). |
| (7) | BR187 | Empty State Rules:<br>1. Trả về phản hồi 200-OK kèm mảng rỗng. <br>2. Hiển thị thông báo trạng thái chưa có dữ liệu: MSG66. |

