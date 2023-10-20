namespace IceSync
{
    public class CustomLoggingMiddleware
    {
        private readonly RequestDelegate _next; 
        private readonly ILogger<CustomLoggingMiddleware> _logger;

        public CustomLoggingMiddleware(RequestDelegate next, ILogger<CustomLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // Check if this is an API request
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                // Log only if it's an API request
                _logger.LogInformation($"Received API request: {context.Request.Method} {context.Request.Path}");
            }

            // Continue the request pipeline
            await _next(context);
        }
    }
}
