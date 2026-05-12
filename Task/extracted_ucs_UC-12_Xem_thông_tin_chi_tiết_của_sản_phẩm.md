# UC-12: Xem thông tin chi tiết của sản phẩm

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Xem thông tin chi tiết của sản phẩm |
| Mã Use Case | UC-12 |
| Mô tả Use Case | Khách hàng xem các thông số kỹ thuật chi tiết và hình ảnh độ phân giải cao để hiểu rõ về sản phẩm trước khi đưa ra quyết định mua hàng. |
| Kích hoạt | Khách hàng nhấn vào tên hoặc hình ảnh của một sản phẩm bất kỳ từ danh sách. |
| Actors | Khách hàng (Customer) |
| Use Case liên quan | Usecase "Kiểm tra trạng thái tồn kho" |
| Tiền điều kiện | Khách hàng đang thao tác trên giao diện cửa hàng. |
| Hậu điều kiện | Trang chi tiết sản phẩm được hiển thị đầy đủ thông tin cấu hình và hình ảnh. |
| Luồng sự kiện chính | 1. Khách hàng nhấn vào một sản phẩm cụ thể.<br>2. Hệ thống truy xuất thông tin chi tiết của sản phẩm từ cơ sở dữ liệu bao gồm hình ảnh.<br>3. Hệ thống hiển thị các thông số kỹ thuật một cách rõ ràng và tương thích với kích thước màn hình.<br>4. Hệ thống kích hoạt Usecase "Kiểm tra trạng thái tồn kho" để hiển thị tình trạng hàng hóa hiện tại. |
| Luồng sự kiện thay thế | Không có. |
| Luồng sự kiện ngoại lệ | - Sản phẩm không còn tồn tại trong hệ thống (đã bị xóa hoàn toàn): Hệ thống hiển thị trang không tìm thấy sản phẩm |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (2) | BR50 | Check Product Existence Rules:<br>1. Hệ thống tiếp nhận yêu cầu xem chi tiết với tham số [productId]. <br>2. [product] = ProductRepository.findById([productId])<br>3. If [product] != null thì chuyển sang Activity (3). <br>4. Else chuyển sang Activity (5). |
| (3) | BR51 | Check Stock Rules:<br>1. [stockStatus] = InventoryRepository.getStockStatus([productId])<br>2. Gắn thông tin tồn kho vào dữ liệu sản phẩm: [product.stockStatus] = [stockStatus]. |
| (4) | BR52 | Display Product Details Rules:<br>1. Trả về phản hồi 200-OK.<br>2. Tải màn hình productDetailsScreen và hiển thị toàn bộ thông tin [product] (bao gồm tên, hình ảnh, giá, mô tả, và [stockStatus]). |
| (5) | BR53 | Message Rules (Not Found):<br>1. Trả về lỗi 404-NOT_FOUND.<br>2. Hiển thị thông báo không tìm thấy sản phẩm trên giao diện: MSG25. |

