# UC-27: Đánh giá sản phẩm đã mua

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Đánh giá sản phẩm đã mua |
| Mã Use Case | UC-27 |
| Mô tả Use Case | Khách hàng gửi bài đánh giá, xếp hạng sao để chia sẻ trải nghiệm thực tế về chất lượng sản phẩm đã nhận. |
| Kích hoạt | Khách hàng nhấn nút "Viết đánh giá" đối với một sản phẩm trong đơn hàng đã hoàn tất. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase "Xem lịch sử đơn hàng" |
| Tiền điều kiện | Đơn hàng chứa sản phẩm đó phải ở trạng thái "Đã giao thành công" (Delivered). |
| Hậu điều kiện | Bài đánh giá được lưu vào hệ thống và chờ hiển thị công khai trên trang sản phẩm. |
| Luồng sự kiện chính | 1. Khách hàng chọn chức năng viết đánh giá.<br>2. Hệ thống hiển thị biểu mẫu bao gồm phần chọn số sao (từ 1 đến 5) và khung nhập bình luận.<br>3. Khách hàng chọn mức sao, nhập nội dung (tùy chọn) và nhấn gửi.<br>4. Hệ thống kiểm tra tính hợp lệ của dữ liệu (phải có số sao tối thiểu).<br>5. Hệ thống lưu bài đánh giá vào cơ sở dữ liệu và liên kết nó với sản phẩm tương ứng.<br>6. Hệ thống hiển thị thông báo gửi đánh giá thành công. |
| Luồng sự kiện thay thế | 4a. Khách hàng chưa chọn số sao: Hệ thống hiển thị cảnh báo yêu cầu đánh giá sao trước khi gửi. |
| Luồng sự kiện ngoại lệ | - Lỗi đường truyền mạng: Hệ thống hiển thị cảnh báo lỗi kết nối và giữ nguyên nội dung khách vừa nhập để không bị mất dữ liệu. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR131 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu viết đánh giá từ màn hình chi tiết đơn hàng. <br>2. Các trường dữ liệu yêu cầu từ client: [productId], [orderId]. <br>3. Danh sách các trường nhập liệu trong biểu mẫu: [ratingStars] (từ 1 đến 5 sao), [reviewComment]. |
| (4) | BR132 | Validation Rules:<br>1. Hệ thống kiểm tra giá trị của trường [ratingStars]. <br>2. If [ratingStars] chưa được chọn (null) hoặc bằng 0 then returns 400-BAD_REQUEST kèm MSG49. <br>3. Else chuyển sang Activity (6). |
| (5) | BR133 | Message Rules (Error):<br>Hiển thị thông báo yêu cầu khách hàng phải chọn số sao tối thiểu trước khi gửi: MSG49. |
| (6) | BR134 | Storage Rules:<br>1. Hệ thống thực hiện lưu bài đánh giá vào CSDL: ReviewRepository.save([userId], [productId], [ratingStars], [reviewComment]). <br>2. Liên kết bài đánh giá với sản phẩm tương ứng để cập nhật điểm đánh giá trung bình. <br>3. Trả về phản hồi 200-OK kèm thông tin xác nhận lưu trữ thành công. |
| (7) | BR135 | Success Message Rules:<br>Hiển thị thông báo gửi đánh giá thành công trên giao diện: MSG50. |

