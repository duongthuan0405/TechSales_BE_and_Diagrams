# UC-20: Xác nhận đơn hàng

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Xác nhận đơn hàng |
| Mã Use Case | UC-20 |
| Mô tả Use Case | Khách hàng hoàn tất quá trình kiểm tra, chốt đơn và hệ thống tạo lập một đơn hàng chính thức. Toàn bộ quá trình tạo đơn phải tuân thủ nguyên tắc giao dịch nguyên tử (Atomic). |
| Kích hoạt | Khách hàng nhấn nút "Xác nhận đặt hàng" ở cuối màn hình thanh toán. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase "Chọn phương thức thanh toán", Usecase "Nhập mã giảm giá" |
| Tiền điều kiện | Khách hàng đã cung cấp đầy đủ thông tin giao hàng và chọn phương thức thanh toán. |
| Hậu điều kiện | Đơn hàng được khởi tạo thành công, số lượng hàng tồn kho bị trừ và các sản phẩm tương ứng bị xóa khỏi giỏ hàng. |
| Luồng sự kiện chính | 1. Khách hàng nhấn nút xác nhận đặt hàng.<br>2. Hệ thống kiểm tra đối chiếu lại số lượng tồn kho thực tế của toàn bộ sản phẩm trong đơn.<br>3. Hệ thống bắt đầu một giao dịch nguyên tử để thực hiện chuỗi hành động:<br>- Trừ số lượng tồn kho.<br>- Khởi tạo bản ghi đơn hàng mới với trạng thái "Pending".<br>- Xóa các sản phẩm đã chốt khỏi giỏ hàng của người dùng.<br>4. Hệ thống hoàn tất (Commit) giao dịch để lưu vĩnh viễn các thay đổi.<br>5. Hệ thống hiển thị thông báo đặt hàng thành công, gửi mail thông báo và điều hướng khách hàng sang trang chi tiết đơn vừa tạo. |
| Luồng sự kiện thay thế | 2a. Một hoặc nhiều sản phẩm không đủ tồn kho ở thời điểm bấm xác nhận: Hệ thống dừng quá trình tạo đơn, hiển thị cảnh báo lỗi thiếu hàng hóa và yêu cầu khách hàng điều chỉnh lại số lượng. |
| Luồng sự kiện ngoại lệ | 3a. Có lỗi bất ngờ xảy ra ở bất kỳ bước nào trong chuỗi hành động tạo đơn: Hệ thống tự động hủy bỏ (Rollback) toàn bộ giao dịch, không trừ kho, không tạo đơn. Hệ thống hiển thị thông báo lỗi máy chủ. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR85 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu chốt đơn tại checkoutScreen. <br>2. Các trường dữ liệu yêu cầu từ client: [selectedProductIds], [shippingAddressId], [paymentMethodId], [voucherCode] (nếu có). |
| (2) | BR86 | Real-time Stock Check Rules:<br>1. Hệ thống duyệt qua danh sách [selectedProductIds] để kiểm tra tồn kho thực tế từng mặt hàng: [isAvailable] = InventoryRepository.checkBatchStock([selectedProductIds]). <br>2. Nếu có bất kỳ sản phẩm nào không đủ số lượng, returns 400-BAD_REQUEST kèm MSG36. <br>3. Else chuyển sang Activity (4). |
| (3) | BR87 | Insufficient Stock Message Rules:<br>Hiển thị thông báo lỗi thiếu hàng và yêu cầu khách hàng kiểm tra lại giỏ hàng: MSG36. |
| (4) & (5) | BR88 | Order Creation & Inventory Deduction Rules:<br>1. Thực hiện trừ số lượng trong kho: InventoryRepository.deductStock([selectedProductIds]). <br>2. Khởi tạo đơn hàng mới: [newOrder] = OrderRepository.createOrder([orderData]). <br>3. Trạng thái đơn hàng mặc định: [newOrder.status] = 'PENDING'. |
| (6) & (7) | BR89 | Payment Processing Rules:<br>1. Kiểm tra [paymentMethodId].<br>2. Nếu phương thức là 'Online Payment' then gọi dịch vụ thanh toán: PaymentGateway.process([newOrder.totalAmount]). <br>3. Nếu thanh toán thất bại, trả về lỗi và dừng quy trình. |
| (8) | BR90 | Cart Clearing Rules:<br>Sau khi tạo đơn hàng thành công, hệ thống thực hiện xóa các sản phẩm đã mua khỏi giỏ hàng của khách hàng: CartRepository.removeItems([selectedProductIds]). |
| (9) | BR91 | Success Notification & Email Rules:<br>1. Trả về phản hồi 200-OK kèm MSG37. <br>2. Gửi email xác nhận đơn hàng (Chi tiết ở Usecase 21) |

