# UC-47: Dừng voucher trước thời hạn

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Dừng voucher khẩn cấp |
| Mã Use Case | UC-47 |
| Mô tả Use Case | Quản lý sử dụng tính năng "Kill-switch" để vô hiệu hóa một mã khuyến mãi ngay lập tức trước khi hết hạn nhằm kiểm soát ngân sách hoặc khi có sự cố nhầm lẫn. |
| Kích hoạt | Quản lý nhấn nút "Dừng hoạt động" tại một voucher đang có hiệu lực. |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Có ít nhất một voucher đang trong trạng thái hoạt động. |
| Hậu điều kiện | Voucher bị vô hiệu hóa toàn hệ thống. Bất kỳ khách hàng nào đang thao tác ở bước thanh toán sẽ không thể áp dụng mã này nữa. |
| Luồng sự kiện chính | 1. Quản lý chọn một voucher đang hoạt động và nhấn nút dừng khẩn cấp.<br>2. Hệ thống hiển thị hộp thoại cảnh báo về việc vô hiệu hóa mã.<br>3. Quản lý nhấn xác nhận.<br>4. Hệ thống cập nhật trạng thái của voucher thành "Đã dừng" trong cơ sở dữ liệu.<br>5. Hệ thống làm mới giao diện và hiển thị thông báo thao tác thành công. |
| Luồng sự kiện thay thế | 3a. Quản lý hủy bỏ thao tác tại hộp thoại xác nhận: Hệ thống đóng hộp thoại và voucher vẫn tiếp tục hoạt động. |
| Luồng sự kiện ngoại lệ | - Lỗi cập nhật hệ thống: Hệ thống hiển thị thông báo lỗi mạng và yêu cầu thử lại. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR244 | Action Initiation & Warning:<br>1. Hệ thống tiếp nhận [voucherId] từ yêu cầu dừng khẩn cấp của Business Admin. <br>2. Hiển thị hộp thoại EmergencyStop_Dialog kèm cảnh báo MSG91 để xác nhận hành động vô hiệu hóa. |
| (3) | BR245 | Confirmation Branching:<br>1. if phản hồi là "Confirm" then chuyển sang Activity (4). <br>2. if phản hồi là "Cancel" then chuyển sang Activity (6). |
| (4) | BR246 | Status Persistence:<br>1. Thực hiện VoucherRepo.UpdateStatus([voucherId], 'STOPPED') trong Cơ sở dữ liệu. <br>2. Mã giảm giá sẽ bị vô hiệu hóa ngay lập tức và không thể áp dụng cho các đơn hàng mới. |
| (5) | BR247 | Success Notification:<br>1. return 200-OK kèm MSG92 và làm mới trạng thái hiển thị trên giao diện quản trị. |
| (6) | BR248 | Cancellation Logic:<br>1. Đóng hộp thoại xác nhận, giữ nguyên trạng thái Active của voucher và hủy bỏ thao tác. |

