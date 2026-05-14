# UC-61: Thực hiện thanh toán online

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Thanh toán trực tuyến |
| Mã Use Case | UC-61 |
| Mô tả Use Case | Khách hàng hoàn tất việc thanh toán đơn hàng thông qua các cổng thanh toán trực tuyến bên thứ ba (như VNPay, MoMo, ZaloPay) để đảm bảo giao dịch an toàn và nhanh chóng. |
| Kích hoạt | Khách hàng chọn phương thức thanh toán trực tuyến và nhấn nút "Thanh toán" tại màn hình Check out. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Use Case “Xác nhận mua hàng” |
| Tiền điều kiện | Khách hàng đã đăng nhập vào hệ thống, có đơn hàng hợp lệ và đang ở màn hình thanh toán. |
| Hậu điều kiện | Trạng thái đơn hàng được cập nhật thành "Đã thanh toán", thông tin giao dịch được lưu trữ và hệ thống gửi email xác nhận cho khách hàng. |
| Luồng sự kiện chính | 1. Khách hàng chọn phương thức thanh toán trực tuyến và nhấn "Thanh toán".<br>2. Hệ thống khởi tạo yêu cầu giao dịch, mã hóa dữ liệu đơn hàng và tạo mã băm bảo mật (Checksum) để đảm bảo an toàn dữ liệu.<br>3. Hệ thống gửi yêu cầu thanh toán sang Cổng thanh toán và điều hướng khách hàng sang trang web hoặc ứng dụng của đối tác.<br>4. Khách hàng nhập thông tin thanh toán, thực hiện xác thực OTP hoặc quét mã QR trên giao diện của Cổng thanh toán.<br>5. Cổng thanh toán xử lý giao dịch thành công và trả kết quả về hệ thống thông qua URL phản hồi.<br>6. Hệ thống tiếp nhận kết quả, kiểm tra tính hợp lệ của chữ ký số và lưu trữ thông tin giao dịch vào cơ sở dữ liệu.<br>7. Hệ thống cập nhật trạng thái đơn hàng thành "Đã thanh toán" và tự động gửi email thông báo xác nhận cho khách hàng.<br>8. Hệ thống điều hướng khách hàng quay lại website và hiển thị thông báo thanh toán thành công. |
| Luồng sự kiện thay thế | 3a. Thất bại khi kết nối đến Cổng thanh toán: Hệ thống hiển thị thông báo lỗi kỹ thuật và yêu cầu khách hàng thử lại.<br>5a. Khách hàng quyết định hủy giao dịch: Cổng thanh toán trả kết quả hủy về hệ thống, hệ thống cập nhật trạng thái "Đã hủy" và đưa người dùng quay lại trang thanh toán.<br>6a. Giao dịch thanh toán thất bại (sai mã OTP, không đủ số dư...): Cổng thanh toán trả kết quả thất bại, hệ thống cập nhật trạng thái "Thất bại" và hiển thị thông báo cho phép khách hàng thực hiện lại. |
| Luồng sự kiện ngoại lệ | - Khách hàng nhấn "Cancel" hoặc đóng trình duyệt khi đang trong quá trình thanh toán: Use case dừng, hệ thống sẽ đợi cập nhật trạng thái từ Webhook hoặc IPN của cổng thanh toán sau đó. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR92 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu thanh toán trực tuyến từ checkoutScreen. <br>2. Các trường dữ liệu yêu cầu từ client: [orderId], [paymentMethodId], [totalAmount]. |
| (2) | BR93 | Encryption & Checksum Rules:<br>1. Hệ thống khởi tạo yêu cầu giao dịch: [paymentRequest] = PaymentService.initialize([orderId], [totalAmount]). <br>2. Thực hiện mã hóa dữ liệu đơn hàng và tạo mã băm bảo mật: [checksum] = SHA512([paymentRequest.data] + [secretKey]). |
| (3) & (4) | BR94 | Gateway Connection Rules:<br>1. Kiểm tra kết nối đến cổng thanh toán. <br>2. If kết nối thất bại then returns 503-SERVICE_UNAVAILABLE kèm MSG38. <br>3. Else chuyển sang Activity (5). |
| (5) & (6) | BR95 | Redirection Rules:<br>1. Gửi yêu cầu kèm [checksum] sang cổng thanh toán. <br>2. Trả về phản hồi 302-FOUND kèm [paymentUrl] để điều hướng khách hàng sang giao diện của bên thứ ba. |
| (9) & (10) | BR96 | Cancellation Handling Rules:<br>1. Tiếp nhận kết quả từ URL phản hồi (callback URL): [gatewayResponse.status] == 'CANCEL'. <br>2. Cập nhật trạng thái giao dịch trong CSDL: [transaction.status] = 'CANCELLED'. <br>3. Returns 200-OK kèm MSG39. |
| (14) & (16) | BR97 | Failure Handling Rules:<br>1. Tiếp nhận kết quả từ URL phản hồi: [gatewayResponse.status] == 'FAILED'. <br>2. Cập nhật trạng thái giao dịch: [transaction.status] = 'FAILED'. <br>3. Returns 400-BAD_REQUEST kèm MSG40. |
| (18) & (19) | BR98 | Success Verification Rules:<br>1. Tiếp nhận kết quả thành công từ URL phản hồi. <br>2. Hệ thống kiểm tra tính hợp lệ của chữ ký số (Digital Signature): [isValid] = SecurityService.verifySignature([gatewayResponse], [checksum]). <br>3. If [isValid] == false then returns 403-FORBIDDEN và ghi nhật ký cảnh báo bảo mật. |
| (20) & (21) | BR99 | Data Persistence Rules:<br>1. Lưu trữ thông tin giao dịch thành công vào CSDL: TransactionRepository.save([gatewayResponse]). <br>2. Cập nhật trạng thái đơn hàng: [order.paymentStatus] = 'PAID' và [order.status] = 'CONFIRMED'. |
| (22) & (23) | BR100 | Finalization Rules:<br>1. Kích hoạt quy trình gửi email thông báo xác nhận (UC-21). <br>2. Returns 200-OK kèm MSG41. <br>3. Điều hướng khách hàng về website và hiển thị thông báo thành công. |

