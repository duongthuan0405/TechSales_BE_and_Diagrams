# UC-50: Quản lý danh sách khách hàng

## 1. Mô tả chi tiết & Logic nghiệp vụ

| Mục | Nội dung |
| --- | --- |
| Tên Use Case | Quản lý danh sách khách hàng |
| Mã Use Case | UC-50 |
| Mô tả Use Case | Quản trị kinh doanh theo dõi hồ sơ của những người dùng đã đăng ký tài khoản, phục vụ cho mục đích chăm sóc khách hàng và phân tích dữ liệu mua sắm. |
| Kích hoạt | Quản lý nhấn vào mục "Khách hàng" trên thanh điều hướng. |
| Actors | Quản trị Kinh doanh (Business Admin) |
| Use Case liên quan | Không có. |
| Tiền điều kiện | Quản lý đã đăng nhập thành công vào phân hệ nội bộ. |
| Hậu điều kiện | Thông tin tổng quan của khách hàng được hiển thị rõ ràng trên màn hình quản lý. |
| Luồng sự kiện chính | 1. Quản lý truy cập vào màn hình quản lý khách hàng.<br>2. Hệ thống truy vấn cơ sở dữ liệu để lấy danh sách toàn bộ các tài khoản khách hàng.<br>3. Hệ thống sắp xếp danh sách (ví dụ: theo thời gian đăng ký mới nhất) và tiến hành phân trang.<br>4. Hệ thống hiển thị dữ liệu (bao gồm tên, số điện thoại, tổng chi tiêu) trên giao diện bảng. |
| Luồng sự kiện thay thế | 2a. Hệ thống chưa có khách hàng nào đăng ký: Hệ thống hiển thị bảng danh sách trống kèm thông báo tương ứng. |
| Luồng sự kiện ngoại lệ | - Lỗi truy xuất cơ sở dữ liệu: Hệ thống hiển thị thông báo sự cố máy chủ và yêu cầu tải lại giao diện. |

## 2. Business Rules

| Activity | BR Code | Description |
| --- | --- | --- |
| (1) & (2) | BR261 | Data Retrieval Rules:<br>1. Tải màn hình CustomerManagement_Screen. <br>2. Thực hiện truy vấn: [customerList] = CustomerRepository.findAll(). |
| (3) | BR262 | Processing Rules:<br>1. Hệ thống thực hiện sắp xếp danh sách: [customerList].OrderByDescending(c => c.CreatedAt). <br>2. Áp dụng phân trang (Pagination): [pagedList] = [customerList].ToPagedList(pageIndex, pageSize) (mặc định 10 bản ghi/trang). |
| (4) | BR263 | Display Rules (Has Data):<br>1. if [pagedList].Count > 0 then render dữ liệu lên giao diện Grid kèm các cột thông tin: fullName, email, phoneNumber, status. |
| (5) | BR264 | Empty State Rules:<br>1. if [pagedList].Count == 0 then hiển thị bảng trống kèm thông báo MSG98. |

