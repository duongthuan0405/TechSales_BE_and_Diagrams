# UC-45: Ngừng kinh doanh sản phẩm

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Ngừng kinh doanh sản phẩm |
| Mã Use Case | UC-45 |
| Mô tả Use Case | Quản lý đánh dấu các sản phẩm cũ là ngừng kinh doanh. Sản phẩm sẽ bị ẩn khỏi giao diện mua sắm nhưng vẫn giữ nguyên dữ liệu để phục vụ báo cáo lịch sử bán hàng. |
| Kích hoạt | Quản lý đánh dấu chọn các sản phẩm và nhấn nút "Ngừng kinh doanh". |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Quản lý đang ở danh sách sản phẩm. |
| Hậu điều kiện | Các sản phẩm chuyển sang trạng thái "Discontinued", khách hàng không thể tìm kiếm hay xem được nữa. |
| Luồng sự kiện chính | 1. Quản lý chọn một hoặc hàng loạt sản phẩm từ danh sách.<br>2. Quản lý chọn chức năng chuyển trạng thái sang ngừng kinh doanh.<br>3. Hệ thống hiển thị hộp thoại yêu cầu xác nhận thao tác.<br>4. Quản lý nhấn đồng ý.<br>5. Hệ thống bắt đầu giao dịch xử lý hàng loạt.<br>6. Hệ thống cập nhật trạng thái "Discontinued" cho tất cả các sản phẩm đã chọn.<br>7. Hệ thống hoàn tất giao dịch và hiển thị thông báo thao tác thành công. |
| Luồng sự kiện thay thế | 4a. Quản lý từ chối tại hộp thoại xác nhận: Hệ thống đóng hộp thoại và hủy bỏ thao tác. |
| Luồng sự kiện ngoại lệ | - Lỗi cập nhật trong quá trình xử lý hàng loạt: Hệ thống tự động Rollback toàn bộ các thay đổi (nguyên tắc All-or-nothing), không có sản phẩm nào bị đổi trạng thái và hiển thị thông báo lỗi. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR232 | Action Initiation:<br>1. Hệ thống tiếp nhận yêu cầu ngừng kinh doanh từ Business Admin cho sản phẩm cụ thể qua [productId]. |
| (3) | BR233 | Confirmation Request:<br>1. Hệ thống hiển thị hộp thoại xác nhận ConfirmationModal kèm nội dung thông báo MSG85. |
| (4) | BR234 | Response Branching:<br>1. if phản hồi là "Đồng ý" then chuyển sang Activity (5). <br>2. if phản hồi là "Từ chối" then chuyển sang Activity (7). |
| (5) | BR235 | Status Persistence:<br>1. Thực hiện lệnh ProductRepo.UpdateStatus([productId], 'DISCONTINUED') vào Cơ sở dữ liệu. |
| (6) | BR236 | Success Notification:<br>1. return 200-OK kèm MSG86 và cập nhật lại danh sách sản phẩm trên giao diện. |
| (7) | BR237 | Cancellation Logic:<br>1. Đóng hộp thoại xác nhận, giữ nguyên trạng thái sản phẩm hiện tại và hủy bỏ mọi giao dịch đang chờ. |

