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
        private readonly IOptions<AppSettingsModel> appSettings;
        private readonly IFileProvider fileProvider;
        private readonly FormatUtilities formatUtilities;
        private readonly PathString requestPath;
        private Func<HttpContext, bool> match;

        public AppImageProvider(IOptions<AppSettingsModel> appSettings, IFileProvider fileProvider, FormatUtilities formatUtilities)
        {
            this.appSettings = appSettings;
            this.fileProvider = fileProvider;
            this.formatUtilities = formatUtilities;
            this.requestPath = this.appSettings.Value.Common.RoutePrefix;
        }

        public bool IsValidRequest(HttpContext context)
        {
            return this.formatUtilities.GetExtensionFromUri(context.Request.GetDisplayUrl()) != null;
        }

        public Task<IImageResolver> GetAsync(HttpContext context)
        {
            var path = string.IsNullOrEmpty(requestPath) ? 
                context.Request.Path.Value : 
                context.Request.Path.Value.Substring(this.requestPath.Value.Length);

            var fileInfo = this.fileProvider.GetFileInfo(path);

            if (!fileInfo.Exists)
            {
                return Task.FromResult<IImageResolver>(null);
            }

            var metadata = new ImageMetadata(fileInfo.LastModified.UtcDateTime, fileInfo.Length);
            return Task.FromResult<IImageResolver>(new PhysicalFileSystemResolver(fileInfo, metadata));
        }

        public ProcessingBehavior ProcessingBehavior { get; } = ProcessingBehavior.CommandOnly;

        public Func<HttpContext, bool> Match
        {
            get => match ?? IsMatch;
            set => match = value;
        }

        private bool IsMatch(HttpContext context)
        {
            return context.Request.Path.StartsWithNormalizedSegments(requestPath, StringComparison.OrdinalIgnoreCase);
        }
    }
}