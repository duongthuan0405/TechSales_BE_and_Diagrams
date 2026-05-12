---
name: logging-skill
description: Quy chuẩn về ghi nhật ký hệ thống (System Log) và nhật ký nghiệp vụ (Audit Log) trong dự án TechSales Management.
---

# Logging Strategy

Dự án TechSales Management sử dụng hai hệ thống nhật ký song song để đảm bảo tính minh bạch và khả năng truy vết.

---

# 1. System Logging (Nhật ký hệ thống)

Sử dụng **Serilog** để ghi lại các sự kiện kỹ thuật và lỗi phát sinh.

### 1.1. Log Levels (Mức độ Log)
- **Error**: Sử dụng cho các Exception, lỗi logic nghiêm trọng khiến luồng xử lý bị dừng.
- **Warning**: Sử dụng khi có sự cố không mong muốn nhưng chưa gây chết chương trình (ví dụ: gọi API thất bại nhưng có cơ chế retry).
- **Information**: Ghi lại các cột mốc quan trọng (ví dụ: "Đã tạo đơn hàng thành công", "Bắt đầu tiến trình đồng bộ dữ liệu").

### 1.2. Strategic Placement (Vị trí đặt Log)
- **Controllers**: KHÔNG cần đặt log thủ công (đã có Middleware tự động log request/error).
- **Business Services**: Chỉ log các "cột mốc" quan trọng của quy trình nghiệp vụ.
- **External Services**: **BẮT BUỘC** log mọi giao tiếp ra bên ngoài (URL, Status, Errors).
- **Repositories**: Hạn chế log, trừ khi xử lý các câu lệnh SQL phức tạp.

---

# 2. Audit Logging (Nhật ký nghiệp vụ)

Sử dụng cơ chế tự động trong **EF Core** để ghi lại các thay đổi dữ liệu vào bảng `AuditLogs`.

### 2.1. Quy tắc ghi Audit Log
- **CHỈ ghi nhận** các hành động thay đổi dữ liệu: **CREATE, UPDATE, DELETE**.
- **KHÔNG ghi nhận** hành động đọc dữ liệu (**GET**).

### 2.2. Dữ liệu cần lưu trữ
Mỗi bản ghi Audit Log phải chứa:
- `UserId`: Ai thực hiện?
- `Action`: Hành động gì? (Create/Update/Delete).
- `TableName`: Bảng nào bị tác động?
- `PrimaryKey`: ID của bản ghi bị tác động.
- `OldValues`: Dữ liệu cũ (JSON).
- `NewValues`: Dữ liệu mới (JSON).
- `AffectedColumns`: Danh sách các cột bị thay đổi (text, phân cách bởi dấu phẩy).
- `CreatedAt`: Thời điểm thực hiện.

---

# 3. Best Practices
- **Structured Logging**: Luôn sử dụng message template (ví dụ: `Log.Info("User {UserId} logged in", userId)`) thay vì cộng chuỗi.
- **Privacy**: Tuyệt đối **KHÔNG** ghi log các thông tin nhạy cảm như Mật khẩu (Password), số thẻ tín dụng (Credit Card) vào System Log.
- **Correlation**: Sử dụng `TraceId` để liên kết các log thuộc cùng một Request.
