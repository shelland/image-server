// Created on 08/02/2021 20:04 by Andrey Laserson

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shelland.ImageServer.Core.Models.Preferences;

namespace Shelland.ImageServer.Infrastructure.Extensions
{
    public static class OptionsExtensions
    {
        public static IServiceCollection AddConfigOptions(this IServiceCollection collection, IConfiguration configuration)
        {
            collection.Configure<CorsSettingsModel>(configuration.GetSection("Cors"));
            collection.Configure<DirectorySettingsModel>(configuration.GetSection("Directory"));
            collection.Configure<RateLimitingSettingsModel>(configuration.GetSection("RateLimiting"));
            collection.Configure<WebHooksSettingsModel>(configuration.GetSection("WebHooks"));
            collection.Configure<ImageProcessingSettingsModel>(configuration.GetSection("ImageProcessing"));
            collection.Configure<StaticCacheSettingsModel>(configuration.GetSection("StaticCache"));
            collection.Configure<AppSettingsModel>(configuration.GetSection("AppSettings"));

            return collection;
        }
    }
}