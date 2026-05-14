# UC-30: Xem chi tiết đơn hàng

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Xem chi tiết đơn hàng |
| Mã Use Case | UC-30 |
| Mô tả Use Case | Nhân viên bán hàng xem thông tin chi tiết của một đơn hàng (người nhận, danh sách mặt hàng, ghi chú) để xác minh và tiến hành đóng gói. |
| Kích hoạt | Nhân viên nhấn vào một mã đơn hàng cụ thể trong danh sách. |
| Actors | Nhân viên Sales (Sales Staff) |
| Use Case liên quan | Usecase "Xem danh sách đơn hàng Pending" |
| Tiền điều kiện | Nhân viên đang thao tác tại màn hình danh sách đơn hàng. |
| Hậu điều kiện | Toàn bộ chi tiết liên quan đến đơn hàng được hiển thị rõ ràng trên màn hình của nhân viên. |
| Luồng sự kiện chính | 1. Nhân viên nhấn xem chi tiết một đơn hàng.<br>2. Hệ thống truy vấn toàn bộ dữ liệu liên quan đến bản ghi đơn hàng đó từ cơ sở dữ liệu.<br>3. Hệ thống hiển thị màn hình chi tiết, phân chia rõ ràng các khu vực: Thông tin giao hàng, Danh sách sản phẩm kèm số lượng, Phương thức thanh toán và Trạng thái hiện tại. |
| Luồng sự kiện thay thế | Không có. |
| Luồng sự kiện ngoại lệ | - Lỗi truy xuất dữ liệu: Hệ thống hiển thị thông báo lỗi và yêu cầu tải lại trang. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) | BR145 | Loading Rules:<br>1. Hệ thống tiếp nhận yêu cầu xem chi tiết từ Nhân viên Sales tại danh sách quản lý đơn hàng. <br>2. Trường dữ liệu yêu cầu từ client: [orderId]. |
| (2) | BR146 | Detailed Query Rules:<br>1. Hệ thống thực hiện truy vấn thông tin toàn diện của đơn hàng: [orderFullDetails] = OrderRepository.findFullDetails([orderId]). <br>2. If [orderFullDetails] == null then returns 404-NOT_FOUND kèm MSG43. <br>3. Else chuyển sang Activity (3). |
| (3) | BR147 | Display Rules:<br>1. Trả về phản hồi 200-OK kèm toàn bộ dữ liệu đơn hàng [orderFullDetails]<br>2. Hiển thị màn hình chi tiết bao gồm các phân khu thông tin:<br>- Thông tin giao hàng: Full Name, Phone Number, Shipping Address. <br>- Danh sách sản phẩm: Tên sản phẩm, Số lượng, Đơn giá, Thành tiền. <br>- Thanh toán: Payment Method, Shipping Fee, Voucher, Total Amount. <br>- Trạng thái: Order Status |

