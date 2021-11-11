// Created on 20/02/2021 14:50 by Andrey Laserson

using System.Collections.Generic;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shelland.ImageServer.Infrastructure.Extensions
{
    public static class RateLimitingExtensions
    {
        public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            var isEnabled = configuration.GetValue<bool>("RateLimiting:IsEnabled");
            var period = configuration.GetValue<string>("RateLimiting:Period");
            var requestLimit = configuration.GetValue<int>("RateLimiting:RequestLimit");
            var ipWhiteList = configuration.GetValue<List<string>>("RateLimiting:IpWhitelist");

            if (!isEnabled)
            {
                return services;
            }

            services.AddMemoryCache();

            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            services.Configure<IpRateLimitOptions>(opts =>
            {
                opts.IpWhitelist = ipWhiteList;
                opts.GeneralRules = new List<RateLimitRule>
                {
                    new()
                    {
                        Endpoint = "*",
                        Period = period,
                        Limit = requestLimit,
                    }
                };
            });

            return services;
        }

        public static void AddRateLimitingPipeline(this IApplicationBuilder appBuilder, IConfiguration configuration)
        {
            var isEnabled = configuration.GetValue<bool>("RateLimiting:IsEnabled");

            if (isEnabled)
            {
                appBuilder.UseIpRateLimiting();
            }
        }
    }
}