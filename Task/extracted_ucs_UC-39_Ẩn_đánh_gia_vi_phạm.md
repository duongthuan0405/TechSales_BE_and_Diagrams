# UC-39: Ẩn đánh gia vi phạm

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Ẩn đánh giá vi phạm |
| Mã Use Case | UC-39 |
| Mô tả Use Case | Nhân viên Sales thực hiện ẩn hoặc gỡ bỏ các bình luận chứa từ ngữ thô tục, spam hoặc vi phạm chính sách để bảo vệ môi trường mua sắm. Thao tác này bắt buộc phải giải trình. |
| Kích hoạt | Nhân viên nhấn nút "Ẩn đánh giá" tại một bài đánh giá cụ thể. |
| Actors | Nhân viên Sales (Sales Staff) |
| Use Case liên quan | Usecase "Xem phản hồi mới nhất" |
| Tiền điều kiện | Nhân viên đang xem danh sách các đánh giá. |
| Hậu điều kiện | Đánh giá bị ẩn khỏi giao diện khách hàng. Thao tác và lý do được lưu vào Audit Log. |
| Luồng sự kiện chính | 1. Nhân viên nhấn nút ẩn đánh giá.<br>2. Hệ thống hiển thị hộp thoại yêu cầu nhập lý do giải trình cho hành động này.<br>3. Nhân viên nhập lý do và nhấn xác nhận.<br>4. Hệ thống bắt đầu giao dịch xử lý:<br>- Thay đổi trạng thái bài đánh giá thành "Bị ẩn" trong cơ sở dữ liệu.<br>- Tạo một bản ghi trong Audit Log chứa ID nhân viên, thời gian và lý do vừa nhập.<br>5. Hệ thống hoàn tất (Commit) giao dịch.<br>6. Hệ thống hiển thị thông báo ẩn thành công và làm mới danh sách. |
| Luồng sự kiện thay thế | 3a. Nhân viên không nhập lý do giải trình: Hệ thống chặn thao tác và hiển thị cảnh báo bắt buộc điền lý do theo quy định. |
| Luồng sự kiện ngoại lệ | - 4a. Hệ thống Audit Log bị lỗi không thể ghi nhận: Hệ thống tự động Rollback, bài đánh giá không bị ẩn và hiển thị thông báo lỗi hệ thống tạm thời. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR193 | Loading Rules:<br>1. Hệ thống hiển thị hộp thoại HideReview_Dialog. <br>2. Các trường dữ liệu: [violationReason] (Danh sách chọn sẵn) và [violationDescription] (Trường nhập văn bản, mặc định bị ẩn hoặc vô hiệu hóa). |
| (3) | BR194 | Dynamic UI Rules:<br>1. Nếu violationReason == 'OTHER', hệ thống tự động kích hoạt (enable) trường [violationDescription]. <br>2. Nếu chọn các lý do định nghĩa sẵn khác, trường mô tả sẽ bị vô hiệu hóa để tránh dữ liệu thừa. |
| (4) | BR195 | Complex Validation Rules:<br>1. Check 1: Nếu [violationReason] chưa được chọn then trả về MSG70. <br>2. Check 2: Nếu [violationReason] == 'OTHER' VÀ [violationDescription] trống then trả về MSG71. <br>3. Nếu vượt qua các kiểm tra, chuyển sang Activity (5). |
| (5) & (6) | BR196 | Status & Audit Rules:<br>1. Cập nhật trạng thái đánh giá thành 'HIDDEN'. <br>2. Ghi nhận vào Audit Log: [staffId], [reviewId], [violationReason], [violationDescription] (nếu có). |
| (7) | BR197 | Success Notification Rules:<br>Trả về mã 200-OK kèm thông báo MSG69 và cập nhật giao diện quản trị. |
| (8) | BR198 | Error Display Rules:<br>Hiển thị cảnh báo tương ứng (MSG70 hoặc MSG71) ngay trên hộp thoại để nhân viên bổ sung. |

