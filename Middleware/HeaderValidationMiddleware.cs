﻿using Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware
{
  public class HeaderValidationMiddleware
  {
    private readonly RequestDelegate _next;
    private string allowedHosts = "default";
    private string allowedKeys = "default";

    public HeaderValidationMiddleware(RequestDelegate next)
    {
      if (allowedHosts == "default")
      {
        allowedHosts = Environment.GetEnvironmentVariable(EnvironmentVariableNames.ALLOWINGHOSTS.ToString());
        allowedKeys = Environment.GetEnvironmentVariable(EnvironmentVariableNames.ALLOWEDKEYS.ToString());
      }
      _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
      //if ((allowedHosts == "*" || (allowedKeys=="*" && allowedHosts.Split(";").IndexOf(httpContext.Request.Host.ToString()) >= 0)) || allowedKeys.Split(",").IndexOf(httpContext.Request.Headers["key"].ToString()) >= 0)
      var headerValue = httpContext.Request.Headers["Token"];
      
      if (httpContext.Request.GetDisplayUrl().Contains("localhost")|| httpContext.Request.GetDisplayUrl().Contains("api/Logging") || (!string.IsNullOrEmpty(headerValue) && headerValue.FirstOrDefault() == Environment.GetEnvironmentVariable(EnvironmentVariableNames.TOKEN.ToString())))
      {
        httpContext.Response.Headers.Add("x-myHost", httpContext.Request.Host.ToString());
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
