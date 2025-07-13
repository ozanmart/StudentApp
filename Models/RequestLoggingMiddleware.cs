namespace StudentApp.Models;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
        var path = context.Request.Path; 
        var user = context.User.Identity?.Name ?? "Anonymous";
        
        // Log the request details
        _logger.LogInformation("MyCustomMiddleware: Request from IP {IpAddress}, Path: {Path}, User: {User}",
            ipAddress, path, user);
        await _next(context);
    }
}