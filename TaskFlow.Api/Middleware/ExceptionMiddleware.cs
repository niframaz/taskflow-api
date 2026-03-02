using System.Net;
using System.Text.Json;

namespace TaskFlow.Api.Middleware
{
    public class ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Resource not found.");
                await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized.");
                await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Service unavailable.");
                await HandleExceptionAsync(context, HttpStatusCode.ServiceUnavailable, "External service unavailable.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Internal server error.");
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Internal server error.");
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context,
                                                       HttpStatusCode statusCode,
                                                       string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var result = JsonSerializer.Serialize(new
            {
                status = context.Response.StatusCode,
                error = message
            });

            await context.Response.WriteAsync(result);
        }
    }
}
