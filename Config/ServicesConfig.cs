using System;
using EskomCalendarApi.Services;
using EskomCalendarApi.Services.Calendar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Config
{
    public static class ServicesConfig
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            // Add Services
            services.AddSingleton<ICalendarService, CalendarService>();
            services.AddSingleton<IEskomService, EskomService>();
            services.AddHttpClient<CalendarHttpClient>();
            services.AddHttpClient<EskomHttpClient>();
        }
    }
}