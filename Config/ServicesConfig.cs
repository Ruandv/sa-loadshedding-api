using System;
using HttpClients;
using Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;

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
      services.AddSingleton<ILoggingService, LoggingService>();
      services.AddSingleton<ICacheService, CacheService>();
      services.AddHttpClient<EskomHttpClient2>();
      services.AddHttpClient<EspHttpClient>();
      services.AddAutoMapper(typeof(AutoMapperProfiles));
    }
  }
}