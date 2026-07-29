using System.Net;
using System.Text.Json;
using Common_Module.src.BusinessExceptions;
using Common_Module.src.Presentation.ApiResponseModels;
using Microsoft.AspNetCore.Http;

namespace Common_Module.src.Middlewares
{
    public class GlobalExceptionCatchingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch(BusinessException businessException)
            {
                await HandleExceptionAsync(context, businessException);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var statusCode = HttpStatusCode.InternalServerError;
            var message = "";
            Dictionary<string, List<string>>? errors = null;

            // Map custom business exceptions to HTTP status codes
            switch (exception)
            {
                case BadRequestException badRequestEx:
                    statusCode = HttpStatusCode.BadRequest;
                    message = badRequestEx.Message;
                    errors = badRequestEx.Errors;
                    break;

                case UnauthorizedException unauthorizedEx:
                    statusCode = HttpStatusCode.Unauthorized;
                    message = unauthorizedEx.Message;
                    errors = unauthorizedEx.Errors;
                    break;

                case ForbiddenException forbiddenEx:
                    statusCode = HttpStatusCode.Forbidden;
                    message = forbiddenEx.Message;
                    errors = forbiddenEx.Errors;
                    break;

                case NotFoundException notFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    message = notFoundEx.Message;
                    errors = notFoundEx.Errors;
                    break;

                case ConflictUniqueValueException conflictEx:
                    statusCode = HttpStatusCode.Conflict;
                    message = conflictEx.Message;
                    errors = conflictEx.Errors;
                    break;

                default:
                    errors = new Dictionary<string, List<string>>
                    {
                        { "server_errors", new List<string> { exception.Message } }
                    };
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var response = new ApiErrorResponse(message, errors);
            
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var result = JsonSerializer.Serialize(response, options);

            return context.Response.WriteAsync(result);
        }
    }
}