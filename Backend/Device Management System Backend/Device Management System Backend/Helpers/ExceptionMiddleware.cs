using System.Net;
using System.Text.Json;

namespace Device_Management_System_Backend.Helpers
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                //fwd request
                await _next(context); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message); 
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = ex switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound,     
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized, 
                _ => (int)HttpStatusCode.InternalServerError              
            };

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message,
                //only show the stack trace if we are in dev mode
                Details = _env.IsDevelopment() ? ex.StackTrace?.ToString() : "An internal error occurred."
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }

}
