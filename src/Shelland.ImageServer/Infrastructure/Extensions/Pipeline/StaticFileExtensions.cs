// Created on 08/02/2021 22:34 by Andrey Laserson

using System;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Shelland.ImageServer.Core.Other;

namespace Shelland.ImageServer.Infrastructure.Extensions.Pipeline
{
    public static class StaticFileExtensions
    {
        /// <summary>
        /// Add static content middleware
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        public static void AddCachedStaticFiles(this IApplicationBuilder app, IConfiguration configuration)
        {
            var workingDirectory = configuration["Directory:WorkingDirectory"];
            var cacheTimeSeconds = configuration["StaticCache:CacheTimeSeconds"];
            var routePrefix = configuration["AppSettings:RoutePrefix"];

            // Check if working directory is defined. Otherwise, fail fast
            Guard.Against.NullOrEmpty(workingDirectory, nameof(workingDirectory));
            
            var cacheDuration = int.TryParse(cacheTimeSeconds, out var value) ? value : default(int?);

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(workingDirectory),
                RequestPath = routePrefix,
                OnPrepareResponse = ctx =>
                {
                    var headers = ctx.Context.Response.GetTypedHeaders();

                    headers.CacheControl = new Microsoft.Net.Http.Headers.CacheControlHeaderValue
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(cacheDuration ?? Constants.DefaultCacheDuration)
                    };
                }
            });
        }
    }
}