using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using DOCOsoft.UserManagement.Application.Users.Commands.UpdateUser;
using DOCOsoft.UserManagement.Domain.Common;

namespace DOCOsoft.UserManagement.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var errorResponse = new
            {
                Message = "An error occurred while processing your request.",
                Details = exception.Message
            };

            response.StatusCode = exception switch
            {
                DomainValidationException => (int)HttpStatusCode.BadRequest,
                UserNotFoundException => (int)HttpStatusCode.NotFound,
                DuplicateEmailException => (int)HttpStatusCode.Conflict,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var result = JsonSerializer.Serialize(errorResponse);
            return response.WriteAsync(result);
        }
    }
}
