# UC-18: Nhập mã giảm giá

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Nhập mã giảm giá |
| Mã Use Case | UC-18 |
| Mô tả Use Case | Khách hàng áp dụng mã voucher hợp lệ để nhận được mức ưu đãi giảm giá cho đơn hàng chuẩn bị thanh toán. |
| Kích hoạt | Khách hàng nhập văn bản vào ô "Mã giảm giá" và nhấn nút "Áp dụng". |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Khách hàng đang ở màn hình Thanh toán và có sản phẩm hợp lệ để chốt đơn. |
| Hậu điều kiện | Mức giảm giá được trừ trực tiếp vào tổng số tiền khách hàng cần thanh toán. |
| Luồng sự kiện chính | 1. Khách hàng nhập mã giảm giá và nhấn nút áp dụng.<br>2. Hệ thống truy xuất dữ liệu để kiểm tra tính hợp lệ và các điều kiện đi kèm của mã voucher (thời hạn, ngân sách, giá trị đơn hàng tối thiểu).<br>3. Hệ thống tính toán số tiền được giảm và cập nhật lại tổng hóa đơn cuối cùng.<br>4. Hệ thống hiển thị thông báo áp dụng mã thành công. |
| Luồng sự kiện thay thế | 2a. Mã giảm giá sai, hết hạn, hoặc không thỏa mãn điều kiện đơn hàng: Hệ thống hiển thị cảnh báo lỗi và giữ nguyên tổng tiền ban đầu. |
| Luồng sự kiện ngoại lệ | - Lỗi kết nối truy xuất dữ liệu voucher: Hệ thống hiển thị thông báo lỗi hệ thống và yêu cầu thử lại sau. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR77 | Loading Rules:<br>1. Hệ thống tiếp nhận tương tác tại màn hình checkoutScreen. <br>2. Trường dữ liệu yêu cầu từ client: [voucherCode]. |
| (2) | BR78 | Voucher Validation Rules:<br>1. Hệ thống thực hiện truy xuất thông tin: [voucher] = VoucherRepository.findByCode([voucherCode]). <br>2. Kiểm tra tính hợp lệ: Nếu [voucher] == null HOẶC [voucher].status != 'ACTIVE' HOẶC isExpired([voucher]) then returns 400-BAD_REQUEST kèm MSG33. <br>3. Else chuyển sang Activity (3). |
| (3) & (4) | BR79 | Discount Calculation Rules:<br>1. Tính toán số tiền được giảm: [discountAmount] = calculateDiscountValue([voucher], [totalOrderAmount]). <br>2. Cập nhật lại tổng giá trị hóa đơn: [finalTotal] = [totalOrderAmount] - [discountAmount]. <br>3. Trả về phản hồi 200-OK kèm dữ liệu [discountAmount] và [finalTotal] đã cập nhật. |
| (5) | BR80 | Message Rules (Success):<br>Hiển thị thông báo áp dụng mã giảm giá thành công: MSG34. |
| (6) | BR81 | Message Rules (Invalid Voucher):<br>Hiển thị thông báo lỗi mã không hợp lệ hoặc đã hết hạn: MSG33. |

