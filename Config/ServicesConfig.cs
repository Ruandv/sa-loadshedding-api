using System;
using EskomCalendarApi.Mappings;
using EskomCalendarApi.Services;
using HttpClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Eskom;

namespace Config
{
    public static class ServicesConfig
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
            // Add Services
            services.AddSingleton<IEskomService, EskomService>();
            services.AddHttpClient<EskomHttpClient2>();
            services.AddHttpClient<EspHttpClient>();
            services.AddSingleton<LoggingService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles));
        }
    }
}