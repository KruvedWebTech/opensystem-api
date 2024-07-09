using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace opensystem_api.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Middleware logic goes here

            await _next(context);
        }

        public static string GetClientIPAddress(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrEmpty(ipAddress) || ipAddress == "::1")
            {
                ipAddress = "127.0.0.1"; // Localhost IP
            }

            return ipAddress;
        }
    }
}
