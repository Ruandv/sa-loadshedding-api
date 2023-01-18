using EskomCalendarApi.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EskomCalendarApi.Middleware
{
    public class HeaderValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private string allowedHosts = "default";

        public HeaderValidationMiddleware(RequestDelegate next)
        {
            if (allowedHosts == "default")
            {
                allowedHosts = Environment.GetEnvironmentVariable(EnvironmentVariableNames.ALLOWINGHOSTS.ToString());
            }
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            if (allowedHosts == "*" || allowedHosts.Split(";").IndexOf(httpContext.Request.Host.ToString()) >= 0 || httpContext.Request.Headers.ContainsKey("TEST") == true)
            {
                await _next(httpContext); // calling next middleware
            }

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HeaderValidationMiddleware>();
        }
    }
}
