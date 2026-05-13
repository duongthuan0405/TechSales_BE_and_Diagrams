# UC-51: Chặn tài khoản đặt hàng giả mạo

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Chặn tài khoản đặt hàng giả mạo |
| Mã Use Case | UC-51 |
| Mô tả Use Case | Quản lý vô hiệu hóa các tài khoản có hành vi đặt hàng ảo nhằm phá hoại. Quá trình khóa tài khoản và hủy các đơn ảo của họ phải diễn ra đồng thời để giải phóng hàng hóa về kho. |
| Kích hoạt | Quản lý nhấn nút "Chặn tài khoản" tại hồ sơ của một khách hàng có dấu hiệu vi phạm. |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Tài khoản khách hàng vi phạm đang tồn tại trên hệ thống. |
| Hậu điều kiện | Tài khoản bị khóa, mọi đơn hàng đang chờ xử lý của người này bị hủy và số lượng sản phẩm được hoàn trả lại kho. |
| Luồng sự kiện chính | 1. Quản lý chọn tài khoản vi phạm và nhấn nút chặn.<br>2. Hệ thống hiển thị hộp thoại cảnh báo về việc sẽ hủy toàn bộ các đơn "Pending" của tài khoản này.<br>3. Quản lý nhấn xác nhận đồng ý.<br>4. Hệ thống bắt đầu một giao dịch nguyên tử (Atomic transaction):<br>- Thay đổi trạng thái tài khoản thành "Bị khóa".<br>- Chuyển trạng thái toàn bộ đơn hàng đang chờ xử lý của người này thành "Đã hủy".<br>- Hoàn trả số lượng sản phẩm từ các đơn vừa hủy vào tổng tồn kho.<br>5. Hệ thống hoàn tất (Commit) giao dịch.<br>6. Hệ thống hiển thị thông báo xử lý thành công. |
| Luồng sự kiện thay thế | 3a. Quản lý hủy bỏ tại hộp thoại xác nhận: Hệ thống đóng hộp thoại và không thực hiện thay đổi nào. |
| Luồng sự kiện ngoại lệ | - Lỗi cập nhật cơ sở dữ liệu: Hệ thống tự động hủy bỏ giao dịch (Rollback), không khóa tài khoản hay hủy đơn, và hiển thị thông báo lỗi hệ thống. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR265 | Selection Rules:<br>1. Hệ thống tiếp nhận [customerId] từ yêu cầu chặn tài khoản của Business Admin. <br>2. [account] = CustomerRepository.findById([customerId]). |
| (2) | BR266 | Warning Rules:<br>1. Hệ thống hiển thị hộp thoại FraudMitigation_Dialog kèm thông báo MSG99. <br>2. Cảnh báo nhấn mạnh việc tất cả đơn hàng đang ở trạng thái PENDING sẽ bị hủy tự động. |
| (3) | BR267 | Confirmation Rules:<br>1. if phản hồi là "Đồng ý" then chuyển sang Activity (4). <br>2. if phản hồi là "Từ chối" then chuyển sang Activity (8). |
| (4), (5) & (6) | BR268 | Fraud Processing Transaction:<br>Thực hiện một Transaction duy nhất để đảm bảo tính toàn vẹn dữ liệu:<br>1. [account.status] = 'LOCKED'. <br>2. [pendingOrders] = OrderRepository.findPendingByCustomer([customerId]).<br>3. foreach order in [pendingOrders]:<br>begin<br>a. order.status = 'CANCELLED'. <br>b. InventoryService.restock(order.items) (Hoàn trả số lượng sản phẩm vào kho theo từng mã hàng). <br>4. AccountRepository.save([account]) và OrderRepository.saveAll([pendingOrders]). |
| (7) | BR269 | Success Notification:<br>1. returns 200-OK kèm MSG100. <br>2. Làm mới giao diện và cập nhật số lượng tồn kho trên Dashboard. |
| (8) | BR270 | Cancellation Rules:<br>1. Đóng hộp thoại, giữ nguyên trạng thái tài khoản và đơn hàng, hủy bỏ mọi xử lý logic. |

