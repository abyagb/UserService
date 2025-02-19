using System.Text.Json;

namespace UserService.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var statusCode = exception switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            response.StatusCode = statusCode;

            var errorResponse = new
            {
                StatusCode = statusCode,
                Message = exception.Message,
                Details = exception.InnerException?.Message
            };

            return response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}
