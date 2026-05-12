namespace TechSalesManagement.Application.Common;

public static class ErrorDetailMessages
{
    public static class Auth
    {
        public const string InvalidCredentials = "Email hoặc mật khẩu không chính xác.";
        public const string UserAlreadyExists = "Người dùng với email này đã tồn tại.";
        public const string AccountLocked = "Tài khoản của bạn đã bị khóa. Vui lòng thử lại sau.";
        public const string EmailNotVerified = "Vui lòng xác thực email trước khi đăng nhập.";
    }

    public static class Validation
    {
        public const string FieldRequired = "Trường {0} là bắt buộc.";
        public const string FieldInvalid = "Trường {0} không đúng định dạng.";
    }
}
