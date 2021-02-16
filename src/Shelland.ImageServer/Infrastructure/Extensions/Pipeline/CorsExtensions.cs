// Created on 15/02/2021 22:00 by Andrey Laserson

using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Shelland.ImageServer.Infrastructure.Extensions.Pipeline
{
    public static class CorsExtensions
    {
        /// <summary>
        /// Add CORS headers (if enabled)
        /// </summary>
        /// <param name="application"></param>
        /// <param name="configuration"></param>
        public static void UseAppCors(this IApplicationBuilder application, IConfiguration configuration)
        {
            var isCorsEnabled = configuration.GetValue<bool>("Cors:IsEnabled");

            if (!isCorsEnabled)
            {
                return;
            }

            var allowedOrigins = configuration.GetValue<string[]>("Cors:AllowedOrigins");

            if (allowedOrigins?.Any() == true)
            {
                application.UseCors(opts => opts.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod());
            }
            else
            {
                application.UseCors(opts => opts.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            }
        }
    }
}