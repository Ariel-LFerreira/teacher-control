using System.Net;
using System.Text.Json;
using TeacherControl.Exceptions;
using TeacherControl.Extensions;

namespace TeacherControl.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    
    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain validation error");

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                Success = false,
                Message = ex.Message,
                Errors = ex.Errors.ToList()
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (ApiException ex)
        {
            /*
            _logger.LogError($"[{context.Response.StatusCode}] {ex.Message} ");
            
            context.Response.ContentType = "application/json";
            
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(new
            {
                sucess  = false, 
                message = ex.Message,
                status  = ex.StatusCode
            }, options);
            
            await context.Response.WriteAsync(json);
            */
            
            _logger.LogWarning(ex, "API error");

            context.Response.StatusCode = ex.StatusCode;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                Success = false,
                Message = ex.Message,
                Errors = ex.Details != null
                                    ? new List<string> { ex.Details }
                                    : null
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse
            {
                Success = false,
                Message = $"Internal server error: {ex.Message}"
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}