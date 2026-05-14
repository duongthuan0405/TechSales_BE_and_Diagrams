# UC-28: Đọc đánh giá từ người khác

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Đọc đánh giá từ người khác |
| Mã Use Case | UC-28 |
| Mô tả Use Case | Khách hàng xem các xếp hạng trung bình và bình luận từ những người mua trước để tham khảo chất lượng thực tế của sản phẩm. |
| Kích hoạt | Khách hàng cuộn xuống khu vực "Đánh giá của khách hàng" trên trang chi tiết sản phẩm. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase "Xem thông tin chi tiết" |
| Tiền điều kiện | Khách hàng đang ở trang chi tiết của một sản phẩm. |
| Hậu điều kiện | Danh sách các đánh giá hợp lệ được hiển thị. |
| Luồng sự kiện chính | 1. Khách hàng cuộn đến khu vực đánh giá.<br>2. Hệ thống truy vấn danh sách các bài đánh giá của sản phẩm đó từ CSDL.<br>3. Hệ thống tính toán điểm đánh giá trung bình.<br>4. Hệ thống hiển thị số điểm trung bình và phân trang danh sách các bình luận chi tiết để khách hàng đọc. |
| Luồng sự kiện thay thế | 2a. Sản phẩm chưa có bất kỳ đánh giá nào: Hệ thống hiển thị thông báo sản phẩm chưa có đánh giá. |
| Luồng sự kiện ngoại lệ | Không có. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR136 | Loading Rules:<br>1. Hệ thống tiếp nhận tương tác cuộn xuống khu vực đánh giá tại màn hình productDetailsScreen. <br>2. Trường dữ liệu yêu cầu từ client: [productId]. |
| (2) | BR137 | Review Query Rules:<br>1. Hệ thống thực hiện truy vấn danh sách các bài đánh giá của sản phẩm: [reviewList] = ReviewRepository.findByProductId([productId]). <br>2. If [reviewList].isEmpty() then chuyển sang Activity (5). <br>3. Else chuyển sang Activity (3). |
| (3) | BR138 | Average Rating Calculation Rules:<br>1. Hệ thống thực hiện tính toán điểm trung bình: [averageRating] = calculateAverage([reviewList].stars). <br>2. Kết quả [averageRating] được làm tròn đến 1 chữ số thập phân. |
| (4) | BR139 | Success Display Rules:<br>1. Trả về phản hồi 200-OK kèm dữ liệu [averageRating] và [reviewList]. <br>2. Hệ thống hiển thị điểm trung bình kèm biểu đồ phân bổ sao và danh sách các bình luận chi tiết lên giao diện. |
| (5) | BR140 | Empty State Message Rules:<br>1. Trả về phản hồi 200-OK kèm mảng rỗng. <br>2. Hiển thị thông báo sản phẩm chưa có đánh giá nào: MSG51. |

