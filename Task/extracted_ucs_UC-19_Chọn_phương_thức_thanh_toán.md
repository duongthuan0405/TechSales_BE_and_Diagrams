# UC-19: Chọn phương thức thanh toán

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Chọn phương thức thanh toán |
| Mã Use Case | UC-19 |
| Mô tả Use Case | Khách hàng linh hoạt lựa chọn phương thức thanh toán phù hợp (ví dụ: chuyển khoản ngân hàng, ví điện tử hoặc thanh toán khi nhận hàng) để hoàn tất giao dịch. |
| Kích hoạt | Khách hàng nhấn chọn một trong các tùy chọn thanh toán được cung cấp trên màn hình. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Khách hàng đang ở bước cuối cùng của màn hình Thanh toán. |
| Hậu điều kiện | Phương thức thanh toán được hệ thống ghi nhận để chuẩn bị cho bước tạo đơn hàng. |
| Luồng sự kiện chính | 1. Hệ thống hiển thị danh sách các phương thức thanh toán khả dụng.<br>2. Khách hàng chọn một phương thức mong muốn.<br>3. Hệ thống cập nhật giao diện, hiển thị thêm các hướng dẫn hoặc thông tin liên quan đến phương thức vừa chọn (nếu có).<br>4. Hệ thống lưu tạm lựa chọn này vào bộ nhớ phiên làm việc. |
| Luồng sự kiện thay thế | Không có. |
| Luồng sự kiện ngoại lệ | Không có. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR82 | Loading Rules:<br>1. Hệ thống tiếp nhận tương tác tại phần chọn phương thức thanh toán trên màn hình checkoutScreen. <br>2. Trường dữ liệu yêu cầu từ client: [paymentMethodId]. |
| (2) | BR83 | Instruction Retrieval Rules:<br>1. Hệ thống truy xuất thông tin: [paymentMethod] = PaymentRepository.findById([paymentMethodId]). <br>2. If [paymentMethod] == null then returns 400-BAD_REQUEST kèm MSG35. <br>3. Else return 200-OK, hiện hướng dẫn thanh toán lên màn hình |
| (3) | BR84 | Session Persistence & Display Rules:<br>1. Lưu phương thức đã chọn vào phiên làm việc: CheckoutSession.savePaymentMethod([paymentMethod]). <br>2. Returns 200-OK <br> |

