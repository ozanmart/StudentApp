namespace StudentApp.Models;

public class CountRequestsMiddleware
{
    readonly RequestDelegate _next;
    readonly ILogger<CountRequestsMiddleware> _logger;
    private static int _count;
    
    public CountRequestsMiddleware(RequestDelegate next, ILogger<CountRequestsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        // Increment the request count
        Interlocked.Increment(ref _count);

        // Log the request count
        _logger.LogInformation("Total requests so far: {Count}", _count);

        // Call the next middleware in the pipeline
        await _next(context);
    }
}