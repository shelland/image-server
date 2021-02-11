// Created on 10/02/2021 17:54 by Andrey Laserson

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Shelland.ImageServer.Infrastructure.Filters;

namespace Shelland.ImageServer.Infrastructure.Extensions
{
    public static class ApiExtensions
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddControllers(opts =>
            {
                opts.Filters.Add<ExceptionFilter>();
            }).AddNewtonsoftJson(opts =>
            {
                opts.SerializerSettings.Converters.Add(new StringEnumConverter());
                opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddHttpContextAccessor();

            return services;
        }
    }
}