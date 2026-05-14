# UC-21: Nhận email xác nhận đơn hàng

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Nhận email xác nhận đơn hàng |
| Mã Use Case | UC-21 |
| Mô tả Use Case | Hệ thống tự động gửi một email chứa biên lai mua hàng cho khách hàng ngay sau khi đơn hàng được tạo thành công để xác nhận hệ thống đã ghi nhận yêu cầu. |
| Kích hoạt | Tự động kích hoạt ngay sau khi Usecase "Xác nhận đơn hàng" hoàn tất thành công. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase "Xác nhận đơn hàng" |
| Tiền điều kiện | Đơn hàng đã được tạo thành công và lưu vào cơ sở dữ liệu. Khách hàng có địa chỉ email hợp lệ. |
| Hậu điều kiện | Email xác nhận được gửi đi thành công đến hộp thư của khách hàng. |
| Luồng sự kiện chính | 1. Hệ thống tổng hợp các thông tin của đơn hàng vừa tạo sau khi xác nhận đặt hàng (danh sách sản phẩm, tổng tiền, địa chỉ giao hàng).<br>2. Hệ thống biên dịch thông tin thành một biểu mẫu email biên lai chuẩn.<br>3. Hệ thống kết nối với dịch vụ gửi email (SMTP/Email Service).<br>4. Hệ thống thực hiện gửi email đến địa chỉ đã đăng ký của khách hàng.<br>5. Khách hàng mở hộp thư cá nhân và nhận được email xác nhận. |
| Luồng sự kiện thay thế | Không có. |
| Luồng sự kiện ngoại lệ | - Lỗi kết nối dịch vụ email bên thứ ba: Hệ thống ghi nhận lỗi vào file nhật ký (log) và đưa email này vào hàng đợi để tự động gửi lại sau. Trạng thái của đơn hàng trên web không bị ảnh hưởng. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (2) | BR101 | Loading Rules:<br>1. Hệ thống tiếp nhận sự kiện hoàn tất từ UC-20 hoặc UC-61. <br>2. Các nguồn dữ liệu cần thiết để tổng hợp: [orderId], [userEmail], [orderItems], [totalAmount], [shippingAddress], [paymentMethod]. |
| (3) | BR102 | Email Compilation Rules:<br>Hệ thống thực hiện gửi email theo mẫu:<br>Ví dụ<br>[fullName] = UserProfileRepository.getFullNameByUserId([userId])<br><br> |
| (4) | BR103 | SMTP Connection Rules:<br>1. Hệ thống thực hiện kết nối tới dịch vụ gửi email: [connectionStatus] = SmtpService.connect(). <br>2. If [connectionStatus] == SUCCESS then chuyển sang Activity (5). <br>3. Else chuyển sang Activity (6). |
| (5) | BR104 | Email Dispatch Rules:<br>1. Thực hiện gửi email: SmtpService.send([userEmail], [finalEmailBody]). <br>2. Ghi nhận trạng thái gửi thành công vào nhật ký đơn hàng: OrderLog.info([orderId], 'Confirmation email sent'). |
| (6) | BR105 | Failure & Queue Rules:<br>1. Ghi nhận mã lỗi kết nối: SystemLog.error('SMTP_CONNECTION_FAILED', [orderId]). |

