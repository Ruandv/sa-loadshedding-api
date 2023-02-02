using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EskomCalendarApi;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Config
{
    public static class MvcConfig
    {
        public static void ConfigureMvc(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.MimeTypes = GetSupportedCompressionMimeTypes();
            });

            services.AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddApplicationPart(typeof(Startup).Assembly);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials());
            });
        }

        public static void UseMvcConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Handles exceptions and generates a custom response body
                app.UseExceptionHandler("/errors/500");

                // Handles non-success status codes with empty body
                app.UseStatusCodePagesWithReExecute("/errors/{0}");

                // Global exception catch not to expose data about the exception.
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "text/plain";

                        await context.Response.WriteAsync(string.Empty);
                    });
                });
                app.UseHsts();
            }

            app.UseHttpsRedirection();
        }

        private static IEnumerable<string> GetSupportedCompressionMimeTypes()
        {
            var result = ResponseCompressionDefaults.MimeTypes.ToList();

            result.Add(MediaTypeNames.Application.Pdf);

            // Older MS format that would benefit from compression.
            result.Add("application/msword");
            result.Add("application/vnd.ms-excel");
            result.Add("application/vnd.ms-powerpoint");
            result.Add("application/vnd.ms-outlook");

            return result;
        }
    }
}