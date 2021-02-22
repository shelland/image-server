// Created on 08/02/2021 22:34 by Andrey Laserson

using System;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Shelland.ImageServer.Core.Other;
using SixLabors.ImageSharp.Web.DependencyInjection;

namespace Shelland.ImageServer.Infrastructure.Extensions.Pipeline
{
    public static class StaticFileExtensions
    {
        /// <summary>
        /// Add static content middleware
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        public static void UseAppCachedStaticFiles(this IApplicationBuilder app, IConfiguration configuration)
        {
            var workingDirectory = configuration.GetValue<string>("Directory:WorkingDirectory");
            var cacheTimeSeconds = configuration.GetValue<int?>("StaticCache:CacheTimeSeconds");
            var routePrefix = configuration.GetValue<string>("Common:RoutePrefix");
            var isOnDemandProcessingEnabled = configuration.GetValue<bool>("OnDemandProcessing:IsEnabled");

            if (isOnDemandProcessingEnabled)
            {
                app.UseImageSharp();
            }

            // Check if working directory is defined. Otherwise, fail fast
            Guard.Against.NullOrEmpty(workingDirectory, nameof(workingDirectory));
            
            // Add static files middleware
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(workingDirectory),
                RequestPath = routePrefix,
                OnPrepareResponse = ctx =>
                {
                    var headers = ctx.Context.Response.GetTypedHeaders();

                    // Add caching headers

                    headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(cacheTimeSeconds ?? Constants.DefaultCacheDuration)
                    };
                }
            });
        }
    }
}