// Created on 14/02/2021 17:53 by Andrey Laserson

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Shelland.ImageServer.Core.Models.Preferences;
using Shelland.ImageServer.Infrastructure.Extensions;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Providers;
using SixLabors.ImageSharp.Web.Resolvers;

namespace Shelland.ImageServer.Infrastructure.Storage
{
    /// <summary>
    /// Custom image provider based on custom file provider
    /// </summary>
    public class AppImageProvider : IImageProvider
    {
        private readonly IFileProvider fileProvider;
        private readonly FormatUtilities formatUtilities;
        private readonly PathString requestPath;
        private Func<HttpContext, bool>? match;

        public AppImageProvider(
            IOptions<AppSettingsModel> appSettings, 
            IFileProvider fileProvider, 
            FormatUtilities formatUtilities)
        {
            this.fileProvider = fileProvider;
            this.formatUtilities = formatUtilities;
            this.requestPath = appSettings.Value.Common.RoutePrefix;
        }

        public bool IsValidRequest(HttpContext context)
        {
            return this.formatUtilities.TryGetExtensionFromUri(context.Request.GetDisplayUrl(), out var url) && 
                   !string.IsNullOrEmpty(url);
        }

        public Task<IImageResolver?> GetAsync(HttpContext context)
        {
            var path = string.IsNullOrEmpty(requestPath) ? 
                context.Request.Path.Value! : 
                context.Request.Path.Value![this.requestPath.Value!.Length..];

            var fileInfo = this.fileProvider.GetFileInfo(path);

            if (!fileInfo.Exists)
            {
                return Task.FromResult<IImageResolver?>(null);
            }

            return Task.FromResult<IImageResolver?>(new FileProviderImageResolver(fileInfo));
        }

        public ProcessingBehavior ProcessingBehavior => ProcessingBehavior.CommandOnly;

        public Func<HttpContext, bool> Match
        {
            get => match ?? IsMatch;
            set => match = value;
        }

        private bool IsMatch(HttpContext context) => context.Request.Path.StartsWithNormalizedSegments(requestPath, StringComparison.OrdinalIgnoreCase);
    }
}