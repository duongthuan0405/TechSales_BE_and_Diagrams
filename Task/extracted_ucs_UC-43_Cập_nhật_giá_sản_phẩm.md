# UC-43: Cập nhật giá sản phẩm

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Cập nhật giá sản phẩm |
| Mã Use Case | UC-43 |
| Mô tả Use Case | Quản lý điều chỉnh giá bán của các sản phẩm để phù hợp với biến động thị trường. Cập nhật này áp dụng ngay cho các đơn hàng mới. |
| Kích hoạt | Quản lý nhập mức giá mới vào ô dữ liệu giá và nhấn nút "Cập nhật". |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Quản lý đang thao tác trên danh sách sản phẩm. |
| Hậu điều kiện | Giá sản phẩm được thay đổi trên toàn hệ thống. Lịch sử giá của các đơn hàng cũ được bảo toàn. |
| Luồng sự kiện chính | 1. Quản lý chọn sản phẩm cần đổi giá.<br>2. Quản lý nhập mức giá mới.<br>3. Quản lý nhấn nút xác nhận lưu.<br>4. Hệ thống kiểm tra tính hợp lệ của dữ liệu giá vừa nhập.<br>5. Hệ thống cập nhật giá trị vào cơ sở dữ liệu.<br>6. Hệ thống hiển thị thông báo cập nhật giá thành công. |
| Luồng sự kiện thay thế | 4a. Quản lý nhập giá trị âm hoặc sai định dạng: Hệ thống chặn thao tác và hiển thị cảnh báo dữ liệu không hợp lệ. |
| Luồng sự kiện ngoại lệ | - Lỗi truy xuất cơ sở dữ liệu: Hệ thống hiển thị thông báo sự cố và không lưu thay đổi. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR221 | Data Loading & Input:<br>1. Hệ thống truy vấn giá hiện tại của sản phẩm theo [productId]. <br>2. Tiếp nhận giá trị [newPrice] từ giao diện chỉnh sửa nhanh hoặc trang chi tiết. |
| (4) | BR222 | Price Validation Logic:<br>1. if isEmpty([newPrice]) then return 400-BAD_REQUEST with MSG1. <br>2. if [newPrice] <= 0 then return 400-BAD_REQUEST with MSG79. <br>3. if [newPrice] == [currentPrice] then return 400-BAD_REQUEST with MSG82. |
| (5) | BR223 | Database Update:<br>Thực hiện ProductRepo.UpdatePrice([productId], [newPrice]). Hệ thống đồng thời cập nhật trường updatedAt để phục vụ đối soát. |
| (6) | BR224 | Success Notification:<br>Return 200-OK kèm MSG81 và cập nhật hiển thị giá mới trên giao diện quản trị. |
| (7) | BR225 | Error Display:<br>Hiển thị thông báo lỗi tương ứng (MSG1, MSG79 hoặc MSG82) ngay tại ô nhập liệu để yêu cầu nhân viên điều chỉnh. |

