# UC-35: Khởi tạo hoàn tiền

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Khởi tạo hoàn tiền |
| Mã Use Case | UC-35 |
| Mô tả Use Case | Xử lý lệnh hoàn tiền cho những đơn hàng đã được khách hàng thanh toán trước (qua ví điện tử, thẻ) nhưng sau đó bị hủy. |
| Kích hoạt | Nhân viên nhấn nút "Hoàn tiền" tại đơn hàng đã bị hủy. |
| Actors | Nhân viên Sales (Sales Staff) |
| Use Case liên quan | Usecase "Hủy đơn hàng" |
| Tiền điều kiện | Đơn hàng ở trạng thái "Đã hủy" và có ghi nhận giao dịch thanh toán thành công trước đó. |
| Hậu điều kiện | Lệnh hoàn tiền được gửi đi xử lý và trạng thái tài chính của đơn hàng được cập nhật. |
| Luồng sự kiện chính | 1. Nhân viên chọn chức năng hoàn tiền.<br>2. Hệ thống hiển thị thông tin số tiền cần hoàn và phương thức gốc khách đã dùng để thanh toán.<br>3. Nhân viên xác nhận gửi lệnh hoàn tiền.<br>3. Hệ thống kiểm tra đơn hàng có ở trạng thái “Đã hủy” hay không<br>4. Hệ thống kiểm tra đơn hàng đã thanh toán hay chưa<br>4. Hệ thống kết nối và gửi yêu cầu hoàn tiền qua API của đối tác cổng thanh toán (Bank API).<br>5. Hệ thống nhận lại phản hồi xác nhận từ đối tác.<br>6. Hệ thống cập nhật trạng thái tài chính của đơn thành "Đã hoàn tiền" và ghi Audit Log.<br>7. Hệ thống hiển thị thông báo hoàn tiền thành công. |
| Luồng sự kiện thay thế | 3a. Đơn hàng chưa hủy, vẫn ở trạng thái “Đã duyệt”: Thông báo đơn hàng chưa ở trạng thái hủy.<br>4a. Đơn hàng chưa thanh toán: Thông báo đơn hàng chưa thanh toán nên không thể hoàn tiền.<br>5a. API cổng thanh toán từ chối lệnh hoàn: Hệ thống hiển thị cảnh báo từ chối từ phía ngân hàng và giữ nguyên trạng thái tài chính để nhân viên xử lý thủ công. |
| Luồng sự kiện ngoại lệ | - 4a. Mất kết nối với API cổng thanh toán: Hệ thống hiển thị thông báo lỗi kết nối đối tác và yêu cầu thử lại sau. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR171 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu từ Nhân viên Sales tại màn hình chi tiết đơn hàng. <br>2. Các nguồn dữ liệu cần thiết: [orderId], [totalPaidAmount], [originalPaymentMethod]. <br>3. Hệ thống hiển thị thông tin tóm tắt khoản tiền cần hoàn và phương thức gốc để nhân viên rà soát. |
| (3) | BR172 | Interaction Rules:<br>Nhân viên nhấn nút xác nhận gửi lệnh. Hệ thống hiển thị hộp thoại yêu cầu xác nhận cuối cùng: MSG61. |
| (4) | BR173 | Refund Condition Rules:<br>1. Hệ thống kiểm tra điều kiện: [order.paymentStatus] == 'PAID' VÀ [order.status] == 'CANCELED'. <br>2. If điều kiện không thỏa mãn then returns 400-BAD_REQUEST kèm MSG64. <br>3. Else chuyển sang Activity (5). |
| (5) | BR174 | Bank API Integration Rules:<br>1. Hệ thống gọi phương thức hoàn tiền từ cổng thanh toán: BankAPI.refund([transactionId], [amount]). <br>2. If API trả về mã thành công then chuyển sang Activity (7). <br>3. Else (bị ngân hàng từ chối/lỗi kết nối) then chuyển sang Activity (10). |
| (7) & (8) | BR175 | Data Persistence & Audit Rules:<br>1. Cập nhật trạng thái thanh toán đơn hàng: [order.paymentStatus] = 'REFUNDED'. <br>2. Ghi nhận hành động vào nhật ký: AuditLogger.log([userId], 'INITIATE_REFUND', [orderId], 'SUCCESS'). |
| (9) | BR176 | Success Notification Rules:<br>Trả về phản hồi 200-OK kèm thông báo MSG62 và cập nhật giao diện hiển thị trạng thái hoàn tiền thành công. |
| (10) | BR177 | Bank Error Handling Rules:<br>Hiển thị cảnh báo lỗi chi tiết từ phía ngân hàng hoặc cổng thanh toán: MSG63. |
| (11) | BR178 | Validation Error Rules:<br>Hiển thị thông báo đơn hàng không đủ điều kiện để hoàn tiền (ví dụ: đơn hàng chưa thanh toán hoặc chưa bị hủy): MSG64. |

