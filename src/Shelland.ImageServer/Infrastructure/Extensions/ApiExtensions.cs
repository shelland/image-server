// Created on 10/02/2021 17:54 by Andrey Laserson

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shelland.ImageServer.Infrastructure.Filters;
using Shelland.ImageServer.Infrastructure.ModelBinding;

namespace Shelland.ImageServer.Infrastructure.Extensions
{
    public static class ApiExtensions
    {
        public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
        {
            var isCorsEnabled = configuration.GetValue<bool>("Cors:IsEnabled");

            // Add controllers and JSON support
            services.AddControllers(opts =>
            {
                opts.Filters.Add<ExceptionFilter>();
                opts.Filters.Add<ServerEnabledFilter>();
            }).AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.PropertyNameCaseInsensitive = JsonCommonOptions.Default.JsonSerializerOptions.PropertyNameCaseInsensitive;
                opts.JsonSerializerOptions.PropertyNamingPolicy = JsonCommonOptions.Default.JsonSerializerOptions.PropertyNamingPolicy;
                opts.JsonSerializerOptions.NumberHandling = JsonCommonOptions.Default.JsonSerializerOptions.NumberHandling;
            });

            services.AddHttpContextAccessor();

            // Check if CORS settings were enable
            if (isCorsEnabled)
            {
                services.AddCors();
            }

            return services;
        }
    }
}