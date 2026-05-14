# UC-41: Sửa/ Xóa danh mục

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Sửa hoặc xóa danh mục |
| Mã Use Case | UC-41 |
| Mô tả Use Case | Quản lý tái cấu trúc phân loại sản phẩm bằng cách sửa tên/hình ảnh danh mục hiện tại, hoặc xóa bỏ các danh mục không còn cần thiết. Việc xóa bắt buộc phải chuyển dữ liệu sang danh mục khác. |
| Kích hoạt | Quản lý nhấn nút "Sửa" hoặc "Xóa" tại một danh mục cụ thể. |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Quản lý đã đăng nhập và đang ở giao diện danh sách danh mục. |
| Hậu điều kiện | Danh mục được cập nhật hoặc xóa thành công. Nếu xóa, toàn bộ sản phẩm thuộc danh mục cũ được chuyển an toàn sang danh mục mới. |
| Luồng sự kiện chính | 1. Quản lý chọn thao tác Xóa đối với một danh mục.<br>2. Hệ thống hiển thị hộp thoại cảnh báo và yêu cầu chọn một "Danh mục thay thế" (Migration category) từ danh sách hiện có.<br>3. Quản lý chọn danh mục thay thế và nhấn xác nhận.<br>4. Hệ thống bắt đầu một giao dịch nguyên tử (Atomic transaction).<br>5. Hệ thống chuyển đổi toàn bộ sản phẩm từ danh mục cũ sang danh mục mới.<br>6. Hệ thống tiến hành xóa danh mục cũ khỏi cơ sở dữ liệu.<br>7. Hệ thống hoàn tất (Commit) giao dịch và hiển thị thông báo thành công. |
| Luồng sự kiện thay thế | 1a. Nếu Quản lý chọn thao tác Sửa: Hệ thống hiển thị biểu mẫu chứa thông tin hiện tại. Quản lý cập nhật dữ liệu, nhấn lưu. Hệ thống kiểm tra và lưu lại, hiển thị thông báo cập nhật thành công.<br>3a. (Trong luồng Xóa) Quản lý không chọn danh mục thay thế: Hệ thống chặn thao tác và hiển thị cảnh báo lỗi bắt buộc chọn nơi chuyển dữ liệu. |
| Luồng sự kiện ngoại lệ | - Lỗi chuyển đổi dữ liệu hoặc lỗi cơ sở dữ liệu: Hệ thống tự động Rollback, không xóa danh mục, không chuyển sản phẩm và hiển thị thông báo lỗi hệ thống. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (2) | BR205 | Operation Branching:<br>1. If [actionType] == 'DELETE' then chuyển sang quy trình Xóa tại Activity (3). <br>2. If [actionType] == 'EDIT' then chuyển sang quy trình Sửa tại Activity (10). |
| (3) & (4) | BR206 | Migration Setup:<br>Hệ thống load danh sách danh mục thay thế: CategoryRepo.GetAll().Where(c => c.Id != targetId). |
| (5) | BR207 | Selection Validation:<br>If [replacementCategoryId] == null then return 400-BAD_REQUEST với MSG75 và chuyển đến Activity (9). |
| (6) & (7) | BR208 | Data Transaction:<br>Thực hiện Transaction: { ProductRepo.UpdateCategory(oldId, newId); CategoryRepo.Delete(oldId) } để đảm bảo tính toàn vẹn dữ liệu. |
| (8) | BR209 | Delete Success:<br>Return 200-OK kèm MSG74 và cập nhật lại Grid dữ liệu. |
| (10) & (11) | BR210 | Edit Validation:<br>1. Hiển thị thông tin danh mục hiện tại. <br>2. If isEmpty([categoryName]) then return 400-BAD_REQUEST với MSG1. |
| (12) | BR211 | Update Processing:<br>1. If CategoryRepo.Exists(newName, id != currentId) then return 409-CONFLICT với MSG73. <br>2. Else thực hiện CategoryRepo.Update([categoryData]). |
| (13) | BR212 | Update Success:<br>Return 200-OK kèm MSG76. |

