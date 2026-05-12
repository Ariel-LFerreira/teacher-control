namespace TeacherControl.Middlewares;

public class LoggingMiddleware : IMiddleware
{
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(ILogger<LoggingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = context.Request;

        var start = DateTime.UtcNow;

        _logger.LogInformation("HTTP {Method} {Path} iniciado",
                                request.Method,
                                request.Path
        );

        try
        {
            await next(context);

            var duration = DateTime.UtcNow - start;
			
            // Log da resposta (após sucesso)
            _logger.LogInformation("HTTP {Method} {Path} respondeu {StatusCode} em {Duration} ms",
                                    request.Method,
                                    request.Path,
                                    context.Response.StatusCode,
                                    duration.TotalMilliseconds
            );
        }
        catch (Exception ex)
        {
            var duration = DateTime.UtcNow - start;
			
            // Log de erro (não trato, apenas registro em log)
            _logger.LogError(ex,
                            "Erro HTTP {Method} {Path} após {Duration} ms",
                            request.Method,
                            request.Path,
                            duration.TotalMilliseconds
            );

            throw; // deixo o ExceptionMiddleware tratar
        }
    }
}