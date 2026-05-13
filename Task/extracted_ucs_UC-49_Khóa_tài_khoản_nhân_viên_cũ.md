# UC-49: Khóa tài khoản nhân viên cũ

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Khóa tài khoản của nhân viên cũ |
| Mã Use Case | UC-49 |
| Mô tả Use Case | Quản lý thu hồi quyền truy cập của những nhân viên đã nghỉ việc hoặc vi phạm quy định, đảm bảo tính bảo mật tuyệt đối cho hệ thống nội bộ. |
| Kích hoạt | Quản lý nhấn nút "Khóa tài khoản" tại hồ sơ của một nhân viên. |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Tài khoản nhân viên mục tiêu đang tồn tại trên hệ thống. |
| Hậu điều kiện | Tài khoản bị vô hiệu hóa. Nếu nhân viên đó đang đăng nhập, hệ thống sẽ ép buộc đăng xuất (Force-logout) ngay lập tức. |
| Luồng sự kiện chính | 1. Quản lý tìm kiếm và chọn hồ sơ nhân viên cần khóa.<br>2. Quản lý nhấn nút khóa tài khoản.<br>3. Hệ thống yêu cầu xác nhận thao tác thu hồi quyền.<br>4. Quản lý xác nhận.<br>5. Hệ thống thay đổi trạng thái tài khoản thành "Bị khóa" trong cơ sở dữ liệu.<br>6. Hệ thống kiểm tra và lập tức hủy bỏ các phiên làm việc (session/token) hiện tại của nhân viên đó.<br>7. Hệ thống hiển thị thông báo khóa thành công. |
| Luồng sự kiện thay thế | Không có. |
| Luồng sự kiện ngoại lệ | - Lỗi kết nối hủy token/phiên làm việc: Hệ thống hiển thị cảnh báo sự cố kỹ thuật và yêu cầu thử lại để đảm bảo nhân viên bị đăng xuất hoàn toàn. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR255 | Selection Rules:<br>1. Hệ thống tiếp nhận [staffId] từ yêu cầu khóa tài khoản của Business Admin. <br>2. [account] = AccountRepository.findById([staffId]). |
| (3) | BR256 | Confirmation Rules:<br>1. Hiển thị hộp thoại LockAccount_Dialog kèm thông báo xác nhận MSG96. |
| (4) | BR257 | Response Branching Rules:<br>1. if phản hồi là "Đồng ý" then chuyển sang Activity (5). <br>2. if phản hồi là "Từ chối" then chuyển sang Activity (8). |
| (5) & (6) | BR258 | Lock & Session Cleanup Rules:<br>1. [account.status] = 'LOCKED'.<br>2. AccountRepository.save([account]). <br>3. SessionManager.revokeAllActiveSessions([staffId]): Hệ thống thực hiện thu hồi tất cả JWT/RefreshToken hiện có của nhân viên để buộc đăng xuất ngay lập tức. |
| (7) | BR259 | Success & Message Rules:<br>1. returns 200-OK response với MSG97. <br>2. Hiển thị thông báo thành công và cập nhật trạng thái trên Dashboard. |
| (8) | BR260 | Cancellation Rules:<br>1. Đóng hộp thoại xác nhận, giữ nguyên trạng thái tài khoản và hủy bỏ thao tác. |

