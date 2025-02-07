using Microsoft.OpenApi.Validations;
using System.Text;

namespace CargoPay.Presentation.Middlewares
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public BasicAuthMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("Missing Authorization header");
                return;
            }

            var authHeader = httpContext.Request.Headers["Authorization"].ToString();

            if (!authHeader.StartsWith("Basic "))
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("Invalid Authorization Scheme");
                return;
            }

            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials)).Split(':');
            var username = credentials[0];
            var password = credentials[1];

            if (username != "admin" || password != "password")
            {
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsync("Invalid Credentials");
                return;
            }

            await _requestDelegate(httpContext);
        }
    }
}
