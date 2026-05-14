# UC-56: Định nghĩa vai trò người dùng

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Định nghĩa vai trò người dùng |
| Mã Use Case | UC-56 |
| Mô tả Use Case | Quản trị Kỹ thuật thiết lập và phân bổ các quyền hạn truy cập (Permissions) cho từng nhóm vai trò (Roles) thông qua Ma trận phân quyền, đảm bảo nhân viên chỉ truy cập được dữ liệu thuộc phạm vi công việc. |
| Kích hoạt | Admin Kỹ thuật nhấn nút "Lưu phân quyền" tại giao diện Ma trận Role-Permission. |
| Actors | Quản trị Kỹ thuật (Technical Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Admin Kỹ thuật đã đăng nhập vào phân hệ cấu hình hệ thống. |
| Hậu điều kiện | Quyền hạn mới được áp dụng. Nếu có thay đổi giảm quyền, người dùng thuộc nhóm đó sẽ bị giới hạn ở lần tải trang tiếp theo. |
| Luồng sự kiện chính | 1. Admin Kỹ thuật chọn chức năng quản lý phân quyền.<br>2. Hệ thống hiển thị ma trận quyền hạn cho phép tích chọn.<br>3. Admin thiết lập các quyền (thêm, sửa, xóa, xem) cho từng nhóm vai trò và nhấn lưu.<br>4. Hệ thống bắt đầu một giao dịch nguyên tử (Atomic transaction) theo quy định.<br>5. Hệ thống kiểm tra cấu trúc dữ liệu và cập nhật toàn bộ quyền mới vào cơ sở dữ liệu.<br>6. Hệ thống hoàn tất giao dịch và hiển thị thông báo thiết lập thành công. |
| Luồng sự kiện thay thế | Không có. |
| Luồng sự kiện ngoại lệ | - Lỗi cập nhật cơ sở dữ liệu: Hệ thống tự động Rollback để tránh tình trạng phân quyền bị lỗi một nửa (ai đó có quyền không đáng có), đồng thời hiển thị thông báo lỗi kỹ thuật. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR295 | Loading Matrix Rules:<br>1. Hệ thống truy vấn danh sách vai trò (Roles) và danh sách quyền (Permissions). <br>2. Hiển thị ma trận Role-Permission_Matrix cho phép Technical Admin đánh dấu chọn các quyền tương ứng cho từng vai trò. |
| (3) | BR296 | Setting Rules:<br>1. Hệ thống tiếp nhận danh sách các cặp giá trị [roleId, permissionId] được thiết lập. |
| (4) | BR297 | Data Structure Validation:<br>1. if [roleId] không tồn tại hoặc [permissionId] không hợp lệ then returns 400-BAD_REQUEST kèm MSG110. |
| (5) & (6) | BR298 | Transactional Update Rules:<br>1. Việc cập nhật phải được thực hiện trong một Transaction để đảm bảo tính toàn vẹn dữ liệu. <br>2. Thực hiện: PermissionRepo.SyncRolePermissions([roleId], [selectedPermissions]).<br>3. if quá trình cập nhật gặp lỗi (Deadlock, Connection Timeout, v.v.) then GOTO Activity (8). |
| (7) | BR299 | Success Notification:<br>1. returns 200-OK kèm MSG109. <br>2. Làm mới (Refresh) bộ nhớ đệm phân quyền (Authorization Cache) để các thay đổi có hiệu lực ngay lập tức. |
| (8) | BR300 | Rollback & Error Rules:<br>1. Thực hiện Rollback toàn bộ dữ liệu về trạng thái trước khi thay đổi. <br>2. returns 500-INTERNAL_SERVER_ERROR kèm MSG110. |

