// Created on 13/02/2021 18:14 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
using Shelland.ImageServer.Core.Models.Enums;
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
            var onDemandCacheDirectory = configuration.GetValue<string>("Directory:CacheDirectory");
            var workingDirectory = configuration.GetValue<string>("Directory:WorkingDirectory");
            var cacheTimeSeconds = configuration.GetValue<int?>("StaticCache:CacheTimeSeconds");
            var allowedOnDemandImageSizes = configuration.GetSection("OnDemandProcessing:AllowedDimensions")?.GetChildren()?.Select(x => x.Value).ToHashSet();
            
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

                opts.OnParseCommandsAsync = cmd =>
                {
                    if (cmd.Commands.Any())
                    {
                        ValidateParams(cmd.Commands, allowedOnDemandImageSizes);
                    }

                    return Task.CompletedTask;
                };
            }).Configure<PhysicalFileSystemCacheOptions>(cacheConfig =>
            {
                cacheConfig.CacheRoot = workingDirectory;
                cacheConfig.CacheFolder = onDemandCacheDirectory;
            }).RemoveProvider<PhysicalFileSystemProvider>().AddProvider<AppImageProvider>();

            return services;
        }

        private static void ValidateParams(IDictionary<string, string> imgParams, IReadOnlySet<string> allowedParams)
        {
            if (!allowedParams.Any())
            {
                return;
            }

            // Image params query for on-demand processing should be as following:
            // http://.../myimg.jpg?width=w&height=h (width: required, height: optional)

            imgParams.TryGetValue("width", out var imgWidth);
            imgParams.TryGetValue("height", out var imgHeight);

            var paramKey = $"{imgWidth ?? string.Empty}{(string.IsNullOrEmpty(imgHeight) ? string.Empty : "x")}{imgHeight ?? string.Empty}";

            if (!allowedParams.Contains(paramKey))
            {
                throw new AppFlowException(AppFlowExceptionType.InvalidParameters);
            }
        }
    }
}