# UC-17: Chọn mặt hàng để thanh toán từ giỏ hàng

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Chọn mặt hàng để thanh toán từ giỏ hàng |
| Mã Use Case | UC-17 |
| Mô tả Use Case | Khách hàng tích chọn các mặt hàng cụ thể từ danh sách trong giỏ để chỉ thanh toán cho những món đồ cần thiết trước. |
| Kích hoạt | Khách hàng đánh dấu (tích chọn) vào ô kiểm (checkbox) bên cạnh các sản phẩm và nhấn nút "Thanh toán". |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Khách hàng đang ở trang giỏ hàng. |
| Hậu điều kiện | Khách hàng được chuyển sang màn hình Thanh toán (Checkout) với danh sách các sản phẩm đã chọn. |
| Luồng sự kiện chính | 1. Khách hàng đánh dấu chọn một hoặc nhiều sản phẩm trong giỏ.<br>2. Hệ thống tính toán và hiển thị tổng tiền dự kiến dựa trên các mục đã chọn.<br>3. Khách hàng nhấn nút tiến hành thanh toán.<br>4. Hệ thống ghi nhận danh sách hàng hóa được chọn và chuyển hướng sang màn hình Thanh toán. |
| Luồng sự kiện thay thế | 3a. Khách hàng không chọn bất kỳ sản phẩm nào nhưng vẫn nhấn nút thanh toán: Hệ thống chặn thao tác và hiển thị cảnh báo lỗi yêu cầu chọn ít nhất một sản phẩm. |
| Luồng sự kiện ngoại lệ | Không có. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR72 | Loading Rules:<br>1. Hệ thống tiếp nhận tương tác tích chọn sản phẩm tại cartScreen. <br>2. Trường dữ liệu yêu cầu từ client: [selectedProductIds] (mảng chứa mã các sản phẩm được chọn). |
| (2) | BR73 | Calculation Rules:<br>1. Hệ thống thực hiện tính toán tổng tiền dựa trên danh sách sản phẩm được chọn: [tempTotalPrice] = CartRepository.calculateSelectedItems([selectedProductIds]). <br>2. Trả về phản hồi 200-OK kèm giá trị [tempTotalPrice] để hiển thị thời gian thực trên giao diện. |
| (4) | BR74 | Selection Validation Rules:<br><br>1. Khi khách hàng nhấn "Thanh toán", hệ thống kiểm tra danh sách đã chọn. <br>2. If [selectedProductIds].isEmpty() then returns 400-BAD_REQUEST kèm MSG32. <br>3. Else chuyển sang Activity (6). |
| (5) | BR75 | Message Rules (Empty Selection):<br>Hiển thị thông báo lỗi yêu cầu chọn sản phẩm: MSG32. |
| (6) & (7) | BR76 | Redirect Rules:<br>1. Ghi nhận danh sách sản phẩm vào phiên làm việc thanh toán: CheckoutSession.saveSelection([selectedProductIds]). <br>2. Trả về phản hồi 302-FOUND để chuyển hướng người dùng sang màn hình checkoutScreen. |

