using System.Diagnostics;

namespace Restaurants.API.Middlewares;

public class TimeLoggingMiddleware(ILogger<TimeLoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        await next.Invoke(context);
        stopwatch.Stop();
        
        if(stopwatch.ElapsedMilliseconds > 4000)
            logger.LogWarning("Request [{Verb}] at {Path} took {Time} ms", context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds);
    }
}