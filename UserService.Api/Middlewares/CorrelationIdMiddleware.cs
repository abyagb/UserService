namespace UserService.Middlewares
{
    public class CorrelationIdMiddleware
    {
        //do you understand what this class is supposed to do?
        //because you have this and you are also creating a new correlation id in the controller
        //you should only have one place where you create the correlation id
        //and that should be in the middleware
        //try to understand what the class is doing instead of just copy and paste
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
