using System.Net;
using System.Text.Json;

public class ErrorHandlingMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context) {
        try {
            await _next(context); // 다음 미들웨어로 요청 전달
        }
        catch (Exception ex) {
            _logger.LogError(ex, "An unhandled exception occurred.");
            if (!context.Response.HasStarted) {
                await HandleExceptionAsync(context, ex);
            }
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception) {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = new {
            error = "An unexpected error occurred.",
            details = exception.Message
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

public class LoggingMiddleware {
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger) {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context) {
        // 요청 로깅
        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        _logger.LogInformation($"[REQUEST] {context.Request.Method} {context.Request.Path} \nBody: {requestBody}");

        // 원래 Response.Body를 저장
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try {
            await _next(context); // 다음 미들웨어 실행
        }
        finally {
            // 응답을 로그에 기록한 후 원래 스트림으로 복구
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            _logger.LogInformation($"[RESPONSE] {context.Response.StatusCode} \nBody: {responseBodyText}");

            await responseBody.CopyToAsync(originalBodyStream); // 응답을 원래 스트림으로 복구
            context.Response.Body = originalBodyStream; // 원래 Response.Body로 되돌림
        }
    }
}
