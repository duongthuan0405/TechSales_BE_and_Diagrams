# UC-34: Hủy đơn hàng

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Hủy đơn hàng |
| Mã Use Case | UC-34 |
| Mô tả Use Case | Nhân viên Sales chủ động hủy đơn hàng (do khách hàng yêu cầu qua điện thoại, hoặc do cửa hàng hết hàng đột xuất) và hoàn trả số lượng hàng về kho. |
| Kích hoạt | Nhân viên nhấn nút "Hủy đơn" tại giao diện chi tiết đơn hàng. |
| Actors | Nhân viên Sales (Sales Staff) |
| Use Case liên quan | Usecase "Xem chi tiết đơn hàng" |
| Tiền điều kiện | Đơn hàng chưa chuyển sang trạng thái "Delivered". |
| Hậu điều kiện | Đơn hàng bị hủy, số lượng tồn kho được phục hồi và lưu Audit Log. |
| Luồng sự kiện chính | 1. Nhân viên chọn chức năng hủy đơn.<br>2. Hệ thống hiển thị hộp thoại yêu cầu chọn lý do hủy từ danh sách được chuẩn hóa.<br>3. Nhân viên xác nhận hủy.<br>4. Hệ thống bắt đầu giao dịch:<br>- Chuyển trạng thái đơn sang "Canceled" (Đã hủy).<br>- Hoàn trả lại số lượng sản phẩm vào kho.<br>- Ghi nhận Audit Log bao gồm lý do hủy.<br>5. Hệ thống hoàn tất giao dịch.<br>6. Hệ thống hiển thị thông báo hủy đơn thành công. |
| Luồng sự kiện thay thế | 3a. Nhân viên không chọn lý do hủy: Hệ thống chặn thao tác và hiển thị cảnh báo yêu cầu phải cung cấp lý do. |
| Luồng sự kiện ngoại lệ | - Lỗi cập nhật dữ liệu tồn kho: Hệ thống tự động Rollback, không hủy đơn và hiển thị thông báo lỗi hệ thống. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR162 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu hủy đơn và hiển thị popup CancelOrder_Popup. |
| (3) | BR163 |  |
| (4) | BR164 | Input Submission:<br>Hệ thống truyền bộ tham số (orderID) xuống tầng xử lý Order_Controller. |
| (13) | BR165 |  |
| (5) & (6) | BR166 | Status Update Rules:<br>Hệ thống cập nhật trạng thái đơn hàng thành "Canceled" trong CSDL. |
| (7) & (8) | BR167 | Inventory Rules:<br>Hoàn trả số lượng sản phẩm vào tồn kho thực tế: InventoryRepository.restoreStock(orderID). |
| (9) & (10) | BR168 | Audit Log Rules:<br>Ghi nhật ký hành động: AuditLog.write(staffID, "CANCEL_ORDER", orderID). Nội dung mô tả sẽ được lưu cùng để phục vụ hậu kiểm. |
| (11) & (12) | BR169 | Success Message Rules:<br>Trả về phản hồi 200-OK kèm MSG58 và đóng popup. |
| (14) | BR170 | Error Display Rules:<br>Hiển thị cảnh báo lỗi tương ứng MSG59 hoặc MSG60 ngay trên popup để nhân viên bổ sung thông tin. |

