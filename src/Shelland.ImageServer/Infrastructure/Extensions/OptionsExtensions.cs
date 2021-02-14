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
            collection.Configure<AppSettingsModel>(configuration);
            return collection;
        }
    }
}