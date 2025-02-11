namespace UserService.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;
        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("X-Correlation-ID"))
            {
                var correlationId = Guid.NewGuid().ToString();
                context.Request.Headers.Add("X-Correlation-ID", correlationId);
            }
            await _next(context);
        }
    }
}
