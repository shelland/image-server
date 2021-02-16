// Created on 13/02/2021 18:14 by Andrey Laserson

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shelland.ImageServer.Core.Other;
using Shelland.ImageServer.Infrastructure.Storage;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Providers;

namespace Shelland.ImageServer.Infrastructure.Extensions
{
    public static class ImageProcessingExtensions
    {
        public static IServiceCollection AddImageProcessing(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var isEnabled = configuration.GetValue<bool>("OnDemandProcessing:IsEnabled");
            var onDemandCacheDirectory = configuration.GetValue<string>("OnDemandProcessing:CacheFolderName");
            var workingDirectory = configuration.GetValue<string>("Directory:WorkingDirectory");
            var cacheTimeSeconds = configuration.GetValue<int?>("StaticCache:CacheTimeSeconds");

            if (!isEnabled)
            {
                return services;
            }

            environment.WebRootPath = workingDirectory;

            // Add image processing library services

            services.AddImageSharp(opts =>
            {
                opts.Configuration = Configuration.Default;
                opts.BrowserMaxAge = TimeSpan.FromSeconds(cacheTimeSeconds ?? Constants.DefaultCacheDuration);

                //opts.OnParseCommandsAsync = (cmd) =>
                //{
                //    return Task.CompletedTask; // todo
                //};
            }).Configure<PhysicalFileSystemCacheOptions>(cacheConfig =>
            {
                cacheConfig.CacheRoot = workingDirectory;
                cacheConfig.CacheFolder = onDemandCacheDirectory;
            }).RemoveProvider<PhysicalFileSystemProvider>().AddProvider<AppImageProvider>();

            return services;
        }
    }
}