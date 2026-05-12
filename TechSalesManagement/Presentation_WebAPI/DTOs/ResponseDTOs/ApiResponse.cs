namespace TechSalesManagement.Presentation_WebAPI.DTOs.ResponseDTOs;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ApiResponse<T> SuccessResult(T data, string message = "Operation successful")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> FailureResult(string message, T? data = default)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Data = data
        };
    }
}

public class ApiErrorResponse : ApiResponse<Dictionary<string, List<string>>>
{
    public ApiErrorResponse(string message, Dictionary<string, List<string>>? errors = null)
    {
        Success = false;
        Message = message;
        Data = errors;
    }
}
