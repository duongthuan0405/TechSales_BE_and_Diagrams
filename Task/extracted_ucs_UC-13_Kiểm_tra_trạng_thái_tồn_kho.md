# UC-13: Kiểm tra trạng thái tồn kho

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Kiểm tra trạng thái tồn kho |
| Mã Use Case | UC-13 |
| Mô tả Use Case | Hệ thống tự động kiểm tra số lượng tồn kho thực tế của sản phẩm để khách hàng biết mặt hàng này còn có sẵn để mua hay không. |
| Kích hoạt | Tự động kích hoạt khi khách hàng mở trang chi tiết sản phẩm. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase "Xem thông tin chi tiết" |
| Tiền điều kiện | Hệ thống hoạt động bình thường và kết nối được với CSDL tồn kho. |
| Hậu điều kiện | Tình trạng tồn kho được phản ánh chính xác trên giao diện người dùng. |
| Luồng sự kiện chính | 1. Hệ thống truy vấn dữ liệu tồn kho theo thời gian thực đối với sản phẩm đang xem.<br>2. Hệ thống tính toán số lượng hàng có sẵn để bán.<br>3. Nếu số lượng lớn hơn 0, hệ thống hiển thị trạng thái còn hàng và kích hoạt các nút mua sắm. |
| Luồng sự kiện thay thế | 3a. Nếu số lượng bằng 0: Hệ thống hiển thị trạng thái Hết hàng (Out of Stock) và tự động ẩn nút "Mua ngay" |
| Luồng sự kiện ngoại lệ | - Lỗi đồng bộ tồn kho: Hệ thống tạm thời ngăn chặn việc thêm sản phẩm vào giỏ hàng để tránh tình trạng bán vượt mức cho phép. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (2) & (3) | BR54 | Stock Calculation Rules:<br>1. Hệ thống nhận yêu cầu kiểm tra tồn kho cho sản phẩm hiện tại với mã sản phẩm [productId].<br>2. [availableQuantity] = InventoryRepository.getRealTimeStock([productId]). <br>3. If [availableQuantity] > 0 thì chuyển sang Activity (4). <br>4. Else chuyển sang Activity (6). |
| (4) & (5) | BR55 | In-Stock UI Rules:<br>1. Cập nhật trạng thái sản phẩm: [product.stockStatus] = 'IN_STOCK'. <br>2. Hiển thị nhãn trạng thái còn hàng trên giao diện. <br>3. Kích hoạt (enable) các nút mua sắm: [addToCartButton].setEnabled(true) và [buyNowButton].setEnabled(true). |
| (6) & (7) | BR56 | Out-Of-Stock UI Rules:<br>1. Cập nhật trạng thái sản phẩm: [product.stockStatus] = 'OUT_OF_STOCK'. <br>2. Hiển thị thông báo/nhãn hết hàng trên giao diện: MSG26. <br>3. Vô hiệu hóa (disable) hoặc ẩn các nút mua sắm: [addToCartButton].setEnabled(false) và [buyNowButton].setEnabled(false). |

