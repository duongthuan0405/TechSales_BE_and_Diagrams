# UC-44: Cập nhật số lượng tồn kho

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Cập nhật số lượng tồn kho |
| Mã Use Case | UC-44 |
| Mô tả Use Case | Quản lý chủ động ghi nhận lại chính xác số lượng tồn kho thực tế của hàng hóa sau khi tiến hành kiểm kho hoặc nhập thêm hàng mới. |
| Kích hoạt | Quản lý thay đổi con số trong ô nhập liệu tồn kho và nhấn "Lưu". |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Quản lý đang ở giao diện quản lý tồn kho. |
| Hậu điều kiện | Số lượng tồn kho được làm mới. Nếu số lượng cập nhật > 0, hệ thống tự động hiển thị lại nút "Mua ngay" cho khách hàng (nếu trước đó đã bị ẩn do hết hàng). |
| Luồng sự kiện chính | 1. Quản lý tìm kiếm sản phẩm cần kiểm kê.<br>2. Quản lý nhập số lượng tồn kho mới vào hệ thống.<br>3. Quản lý nhấn xác nhận lưu thay đổi.<br>4. Hệ thống kiểm tra định dạng dữ liệu đầu vào.<br>5. Hệ thống cập nhật số lượng tồn kho vào cơ sở dữ liệu.<br>6. Hệ thống đánh giá lại trạng thái hiển thị của sản phẩm (Còn hàng / Hết hàng).<br>7. Hệ thống hiển thị thông báo cập nhật số lượng thành công. |
| Luồng sự kiện thay thế | 4a. Số lượng nhập vào là số âm hoặc chứa ký tự chữ: Hệ thống hiển thị cảnh báo lỗi định dạng và yêu cầu nhập lại. |
| Luồng sự kiện ngoại lệ | Không có. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR226 | Search & Input:<br>1. Hệ thống tiếp nhận mã sản phẩm [productId] qua thanh tìm kiếm. <br>2. Tiếp nhận giá trị số lượng mới [newStock] từ giao diện kiểm kê. |
| (4) | BR227 | Data Validation Logic:<br>1. if isEmpty([newStock]) then return 400-BAD_REQUEST with MSG1. <br>2. if [newStock] < 0 then return 400-BAD_REQUEST with MSG84. <br>3. if [newStock] không phải số nguyên then return 400-BAD_REQUEST với MSG29. |
| (5) | BR228 | Database Persistence:<br>Thực hiện ProductRepo.UpdateStock([productId], [newStock]) và cập nhật dấu thời gian updatedAt. |
| (6) | BR229 | Automatic Status Evaluation:<br>1. if [newStock] == 0 then set productStatus = 'OUT_OF_STOCK'. <br>2. if [newStock] > 0 and currentStatus == 'OUT_OF_STOCK' then set productStatus = 'ACTIVE'. |
| (7) | BR230 | Success Notification:<br>Return 200-OK kèm MSG83 và làm mới số lượng hiển thị trên Dashboard. |
| (8) | BR231 | Error Handling:<br>Hiển thị cảnh báo lỗi định dạng (MSG29, MSG84) ngay tại trường nhập liệu để nhân viên điều chỉnh. |

