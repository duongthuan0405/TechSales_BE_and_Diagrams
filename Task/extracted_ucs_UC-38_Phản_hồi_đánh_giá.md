# UC-38: Phản hồi đánh giá

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Phản hồi đánh giá |
| Mã Use Case | UC-38 |
| Mô tả Use Case | Nhân viên Sales tương tác, trả lời các thắc mắc hoặc gửi lời cảm ơn đối với bình luận của khách hàng. Phản hồi này sẽ được hiển thị công khai trên trang sản phẩm. |
| Kích hoạt | Nhân viên nhấn nút "Trả lời" dưới một bài đánh giá cụ thể. |
| Actors | Nhân viên Sales (Sales Staff) |
| Use Case liên quan | Usecase "Xem phản hồi mới nhất" |
| Tiền điều kiện | Nhân viên đang xem danh sách các đánh giá. |
| Hậu điều kiện | Câu trả lời của cửa hàng được cập nhật và hiển thị công khai dưới bài đánh giá gốc. |
| Luồng sự kiện chính | 1. Nhân viên chọn bài đánh giá cần phản hồi và nhấn "Trả lời".<br>2. Hệ thống hiển thị khung soạn thảo văn bản.<br>3. Nhân viên nhập nội dung phản hồi và nhấn nút gửi.<br>4. Hệ thống kiểm tra dữ liệu đầu vào.<br>5. Hệ thống lưu nội dung phản hồi vào cơ sở dữ liệu và liên kết nó với bình luận gốc.<br>6. Hệ thống hiển thị thông báo gửi phản hồi thành công và cập nhật lại giao diện. |
| Luồng sự kiện thay thế | 4a. Nhân viên để trống khung soạn thảo nhưng vẫn nhấn gửi: Hệ thống chặn thao tác và hiển thị cảnh báo yêu cầu nhập nội dung. |
| Luồng sự kiện ngoại lệ | - Mất mạng khi đang gửi: Hệ thống hiển thị cảnh báo lỗi kết nối và giữ nguyên nội dung đang soạn dở. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR188 | Loading Rules:<br>1. Hệ thống tiếp nhận lệnh phản hồi từ Nhân viên Sales đối với một bài đánh giá cụ thể. <br>2. Tham số yêu cầu từ phía Client: [reviewId]. <br>3. Hệ thống kích hoạt và hiển thị khung soạn thảo văn bản replyTextArea trên giao diện. |
| (4) | BR189 | Content Validation Rules:<br>1. Hệ thống thực hiện kiểm tra tính hợp lệ của nội dung: [replyContent]. <br>2. If isEmpty([replyContent]) hoặc chỉ chứa khoảng trắng then trả về lỗi kèm MSG68. <br>3. Else chuyển sang Activity (5) để thực hiện lưu trữ. |
| (5) & (6) | BR190 | Data Persistence & Linking Rules:<br>1. Hệ thống thực hiện lưu nội dung vào cơ sở dữ liệu: ReviewReplyRepository.save([staffId], [reviewId], [replyContent]). <br>2. Thiết lập mối quan hệ phụ thuộc giữa phản hồi mới và bài đánh giá gốc để đảm bảo tính toàn vẹn của luồng hội thoại. |
| (7) | BR191 | Success Notification Rules:<br>1. Trả về phản hồi mã 200-OK kèm thông báo thành công MSG67. <br>2. Hệ thống tự động cập nhật trạng thái hiển thị của bài đánh giá thành "Đã phản hồi" và tải lại vùng dữ liệu tương ứng. |
| (8) | BR192 | Error Warning Rules:<br>Hiển thị cảnh báo trực quan yêu cầu nhân viên phải nhập nội dung trước khi gửi: MSG68. |

