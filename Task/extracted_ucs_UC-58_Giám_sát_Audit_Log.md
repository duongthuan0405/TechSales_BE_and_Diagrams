# UC-58: Giám sát Audit Log

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Giám sát Audit Log |
| Mã Use Case | UC-58 |
| Mô tả Use Case | Admin Kỹ thuật theo dõi nhật ký hoạt động của toàn bộ hệ thống (ai đã làm gì, vào lúc nào, từ IP nào) để truy vết khi có sự cố dữ liệu hoặc vi phạm bảo mật. |
| Kích hoạt | Admin Kỹ thuật chọn mục "Audit Log / Nhật ký hệ thống". |
| Actors | Quản trị Kỹ thuật (Technical Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Admin Kỹ thuật đã đăng nhập thành công. |
| Hậu điều kiện | Danh sách các sự kiện hệ thống được hiển thị chi tiết. |
| Luồng sự kiện chính | 1. Admin Kỹ thuật truy cập vào màn hình Audit Log.<br>2. Admin thiết lập các bộ lọc (theo ID người dùng, theo khoảng thời gian, theo hành động cụ thể).<br>3. Hệ thống truy xuất dữ liệu từ bảng nhật ký an toàn.<br>4. Hệ thống hiển thị danh sách các bản ghi nhật ký được phân trang rõ ràng lên màn hình. |
| Luồng sự kiện thay thế | 3a. Không có dữ liệu nhật ký nào khớp với bộ lọc: Hệ thống hiển thị bảng trống kèm thông báo không tìm thấy bản ghi. |
| Luồng sự kiện ngoại lệ | - Dữ liệu nhật ký quá lớn gây lỗi truy vấn: Hệ thống hiển thị cảnh báo lỗi tràn dữ liệu và yêu cầu thu hẹp phạm vi ngày tìm kiếm. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR306 | Loading & Filter Rules:<br>1. Tải màn hình AuditLog_Monitor. <br>2. Hệ thống cung cấp bộ lọc tra cứu bao gồm: [userId], [actionType] (Ví dụ: CREATE, UPDATE, DELETE, LOGIN), và [timestampRange]. |
| (3) | BR307 | Data Retrieval Rules:<br>1. Hệ thống thực hiện truy vấn: [logEntries] = AuditRepository.findLogsByFilters([filters]). <br>2. Dữ liệu nhật ký phải được truy xuất theo chế độ Read-Only để đảm bảo tính toàn vẹn, không được phép chỉnh sửa hoặc xóa. |
| (4) | BR308 | Pagination Rules:<br>1. Hệ thống thực hiện sắp xếp: [logEntries].OrderByDescending(l => l.Timestamp). <br>2. Áp dụng phân trang phía Server (Server-side Pagination): [pagedLogs] = [logEntries].Skip(offset).Take(limit) để tối ưu hiệu năng khi số lượng bản ghi nhật ký lớn. |
| (5) | BR309 | Display Rules (Has Data):<br>1. if [pagedLogs].Count > 0 then render dữ liệu lên màn hình theo các cột: Thời gian, Người thực hiện, Hành động, Đối tượng tác động, và Chi tiết thay đổi (Old Value/New Value). |
| (6) | BR310 | Empty State Rules:<br>1. if [pagedLogs].Count == 0 then hiển thị giao diện bảng trống kèm thông báo MSG112. |

