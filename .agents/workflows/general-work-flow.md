---
description: Work flow này cho phép agent có thể làm các UC đúng ý người dùng, sử dụng khi người dùng yêu cầu thực hiện hóa 1 cái UC nào đó  hơn
---

1 Phân tích tài liệu: Research các UC liên quan qua UC, Diagram sẵn có.
2 Áp dụng các skill liên quan trong project (SKILL.md).
3 Đề xuất các design pattern và giải thích lý do cụ thể.
4 Viết kế hoạch thực hiện (implementation_plan.md) và đợi duyệt.
5 Chia nhỏ task vào task.md.
6 Code và tự kiểm lỗi bằng compiler (dotnet build).
7 Tổng hợp báo cáo (walkthrough.md).

### Constraints:
+Bảo tồn mã nguồn: Nếu có lỗi compiler, chỉ được sửa các file đã tạo hoặc đã sửa trong phiên làm việc hiện tại. Giữ nguyên các file cũ "đã ổn" trừ trường hợp đặc biệt được cho phép.
+Message Constants: Tuyệt đối không hard-code string. Mọi thông báo phải được định nghĩa trong `Common/MessageConstants`.
+Minh bạch: Luôn hỏi và giải trình lý do trước khi sửa file. Liệt kê rõ danh sách file thay đổi.
+User Only File: Bạn không thể sửa file này chỉ có user mới có quyền sửa