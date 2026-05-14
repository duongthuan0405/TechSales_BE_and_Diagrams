# UC-40: Thêm danh mục sản phẩm mới

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Thêm danh mục sản phẩm mới |
| Mã Use Case | UC-40 |
| Mô tả Use Case | Quản trị kinh doanh (Business Admin) tạo thêm các phân loại sản phẩm mới (ví dụ: Đồng hồ thông minh, Phụ kiện Gaming) để mở rộng hệ sinh thái cửa hàng. |
| Kích hoạt | Quản lý nhấn nút "Thêm danh mục" trên màn hình quản lý danh mục. |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Tài khoản có quyền Business Admin đã đăng nhập vào phân hệ quản trị. |
| Hậu điều kiện | Danh mục mới được tạo thành công và xuất hiện trên hệ thống tìm kiếm/lọc của khách hàng. |
| Luồng sự kiện chính | 1. Quản lý nhấn nút thêm danh mục.<br>2. Hệ thống hiển thị biểu mẫu bao gồm các trường: Tên danh mục, Hình ảnh đại diện, và Mô tả.<br>3. Quản lý điền thông tin và nhấn nút lưu.<br>4. Hệ thống kiểm tra để đảm bảo tên danh mục chưa từng tồn tại trên hệ thống.<br>5. Hệ thống lưu bản ghi danh mục mới vào cơ sở dữ liệu.<br>6. Hệ thống hiển thị thông báo tạo thành công. |
| Luồng sự kiện thay thế | 4a. Tên danh mục đã tồn tại: Hệ thống chặn thao tác và hiển thị cảnh báo lỗi trùng lặp dữ liệu. |
| Luồng sự kiện ngoại lệ | - Lỗi lưu trữ tệp hình ảnh đại diện: Hệ thống hiển thị thông báo sự cố máy chủ và yêu cầu thao tác lại. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR199 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu từ Business Admin tại trang quản lý kho/sản phẩm. <br>2. Hiển thị form AddCategory_Form bao gồm các trường: [categoryName], [categoryImage], [categoryDescription]. |
| (3) | BR200 | Field Validation Rules:<br>1. [categoryName] là trường bắt buộc (Mandatory). <br>2. If [categoryName] is null or empty, trả về MSG1. |
| (4) | BR201 | Duplicate Check Rules:<br>1. Hệ thống thực hiện truy vấn: [isExist] = CategoryRepository.existsByName([categoryName]). <br>2. If [isExist] = true (Tên đã tồn tại) then chuyển sang Activity (7). <br>3. Else chuyển sang Activity (5). |
| (5) | BR202 | Persistence Rules:<br>1. Lưu bản ghi mới vào CSDL: CategoryRepository.insert([categoryName], [categoryImage], [categoryDescription]). <br>2. Hệ thống tự động sinh mã định danh duy nhất (CategoryId) và gán ngày tạo. |
| (6) | BR203 | Success Notification Rules:<br>Trả về mã 201-CREATED kèm thông báo MSG72 và làm mới danh sách danh mục trên giao diện. |
| (7) | BR204 | Conflict Error Rules:<br>Trả về mã 409-CONFLICT kèm thông báo MSG73 để yêu cầu người dùng đổi tên khác. |

