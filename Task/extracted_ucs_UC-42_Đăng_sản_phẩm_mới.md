# UC-42: Đăng sản phẩm mới

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Đăng sản phẩm mới |
| Mã Use Case | UC-42 |
| Mô tả Use Case | Quản lý thêm các mặt hàng công nghệ mới vào nền tảng để khách hàng có thể tìm kiếm và mua sắm. |
| Kích hoạt | Quản lý nhấn nút "Thêm sản phẩm" tại phân hệ quản lý kho/sản phẩm. |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Hệ thống đã có sẵn ít nhất một danh mục sản phẩm hợp lệ. |
| Hậu điều kiện | Sản phẩm mới được tạo và lập tức hiển thị trên giao diện cửa hàng của khách hàng (nếu được thiết lập trạng thái công khai). |
| Luồng sự kiện chính | 1. Quản lý nhấn nút thêm mới sản phẩm.<br>2. Hệ thống hiển thị biểu mẫu đăng tải.<br>3. Quản lý nhập các thông tin chi tiết: Tên sản phẩm, giá bán, số lượng tồn kho, thông số kỹ thuật, tải lên hình ảnh và chọn danh mục tương ứng.<br>4. Quản lý nhấn nút lưu.<br>5. Hệ thống xác thực toàn bộ dữ liệu đầu vào (ví dụ: giá và số lượng không được âm).<br>6. Hệ thống lưu trữ hình ảnh và ghi nhận dữ liệu sản phẩm vào cơ sở dữ liệu.<br>7. Hệ thống hiển thị thông báo tạo sản phẩm thành công. |
| Luồng sự kiện thay thế | 5a. Quản lý bỏ trống các trường bắt buộc hoặc nhập sai định dạng: Hệ thống hiển thị cảnh báo tại các trường lỗi và yêu cầu chỉnh sửa. |
| Luồng sự kiện ngoại lệ | - Lỗi máy chủ khi tải tệp hình ảnh lên: Hệ thống hiển thị thông báo lỗi tải tệp và yêu cầu thử lại. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR214 | Form Initialization:<br>Hệ thống hiển thị AddProduct_Form. Các trường dữ liệu bắt buộc (Mandatory) bao gồm: [name], [price], [categoryId], [stock], và ít nhất một [image]. |
| (3) | BR215 | Request Submission:<br>Hệ thống tiếp nhận bộ tham số Multipart/Form-Data bao gồm thông tin chi tiết sản phẩm và các tệp hình ảnh. |
| (4) | BR216 | Server-side Validation:<br>1. if isEmpty(name, price, categoryId) then return 400-BAD_REQUEST with MSG1. <br>2. if price <= 0 then return 400-BAD_REQUEST with MSG79. <br>3. if imageFile.Count == 0 then return 400-BAD_REQUEST with MSG80. |
| (5) | BR217 | Image Storage Rules:<br>1. Hệ thống thực hiện kiểm tra định dạng (.jpg, .png, .webp) và dung lượng tệp (max 5MB): [fileInvalid] = validateImageFile([image])<br>2. if [fileInvalid] = false then return 400 with MSG78. <br>3. else: Lưu trữ tệp vào hệ thống (Cloud Storage/Folder) và lấy URL. |
| (6) | BR218 | Data Persistence:<br>Thực hiện lưu trữ file ảnh: [imageUrls] = FileStorageService.store([image]);<br>Thực hiện ProductRepo.Insert([productData], [imageUrls]) vào CSDL SQL Server/PostgreSQL. Hệ thống tự động gán createdAt và status = 'Active'. |
| (7) | BR219 | Success Feedback:<br>Return 201-CREATED kèm MSG77 và chuyển hướng về trang danh sách sản phẩm. |
| (8) | BR220 | Error Handling:<br>Trả về danh sách các lỗi cụ thể (Field-level errors) để hiển thị cảnh báo tương ứng trên giao diện. |

