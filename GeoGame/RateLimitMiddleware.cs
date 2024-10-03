using System.Collections.Concurrent;

public class RateLimitMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ConcurrentDictionary<string, (DateTime, int)> _requestTracker = new ConcurrentDictionary<string, (DateTime, int)>();

    private readonly int _limit;
    private readonly TimeSpan _interval;

    public RateLimitMiddleware(RequestDelegate next, int limit, TimeSpan interval)
    {
        _next = next;
        _limit = limit;
        _interval = interval;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Path.ToString().Contains("/Game"))
        {
            await _next(context);
            return;
        }

        string ipAddress = context.Connection.RemoteIpAddress.ToString();
        (DateTime lastRequest, int requestCount) tracker;
        _requestTracker.TryGetValue(ipAddress, out tracker);

        if (tracker.lastRequest != default(DateTime) && (DateTime.Now - tracker.lastRequest) < _interval)
        {
            if (tracker.requestCount >= _limit)
            {

                // Rate limit exceeded
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Rate limit exceeded. Please try again later.");
                return;
            }
        }
        else
        {
            tracker.requestCount = 0;
        }

        // Update tracker
        tracker.lastRequest = DateTime.Now;
        tracker.requestCount++;

        _requestTracker[ipAddress] = tracker;

        await _next(context);
    }
}
