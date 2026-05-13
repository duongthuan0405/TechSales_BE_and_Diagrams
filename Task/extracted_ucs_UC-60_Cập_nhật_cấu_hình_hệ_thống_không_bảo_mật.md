# UC-60: Cập nhật cấu hình hệ thống không bảo mật

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Cập nhật cấu hình hệ thống không bảo mật |
| Mã Use Case | UC-60 |
| Mô tả Use Case | Admin Kỹ thuật thay đổi các tham số hoạt động của ứng dụng thông qua giao diện trực quan mà không cần sự can thiệp của đội ngũ lập trình viên. |
| Kích hoạt | Admin Kỹ thuật chỉnh sửa tham số và nhấn "Lưu cấu hình hệ thống". |
| Actors | Quản trị Kỹ thuật (Technical Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Admin đang ở trang cấu hình tham số hệ thống. |
| Hậu điều kiện | Cấu hình mới được áp dụng và có hiệu lực ngay lập tức trên toàn hệ thống. |
| Luồng sự kiện chính | 1. Admin Kỹ thuật chọn một tham số cấu hình cần thay đổi.<br>2. Admin nhập giá trị mới vào ô tương ứng.<br>3. Admin nhấn xác nhận lưu cấu hình.<br>4. Hệ thống kiểm tra định dạng và tính hợp lệ của giá trị vừa nhập.<br>5. Hệ thống lưu thay đổi và cập nhật ngay lập tức các tham số này vào môi trường thực tế.<br>6. Hệ thống hiển thị thông báo lưu thay đổi thành công. |
| Luồng sự kiện thay thế | 4a. Giá trị nhập vào sai định dạng (ví dụ: cổng port là số âm): Hệ thống chặn thao tác lưu và hiển thị cảnh báo lỗi dữ liệu. |
| Luồng sự kiện ngoại lệ | Không có. |

## 2. Business Rules

*Không có Business Rules cụ thể cho Use Case này.*
