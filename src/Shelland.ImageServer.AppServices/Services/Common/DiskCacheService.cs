// Created on 03/03/2021 18:36 by Andrey Laserson

#region Usings

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using Shelland.ImageServer.Core.Models.Preferences;

#endregion

namespace Shelland.ImageServer.AppServices.Services.Common
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public class DiskCacheService : IDiskCacheService
    {
        private readonly IFileService fileService;
        private readonly IOptions<AppSettingsModel> options;
        private readonly ILogger<DiskCacheService> logger;

        public DiskCacheService(
            IFileService fileService,
            IOptions<AppSettingsModel> options,
            ILogger<DiskCacheService> logger)
        {
            this.fileService = fileService;
            this.options = options;
            this.logger = logger;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<Stream> GetOrAdd(string url, Func<Task<Stream>> func, CancellationToken cancellationToken)
        {
            var cacheDirectory = this.options.Value.Directory.CacheDirectory;

            // Check if cache directory exists.
            if (!Directory.Exists(cacheDirectory))
            {
                Directory.CreateDirectory(cacheDirectory);
            }

            // Calculate a hash from the URL
            var urlHash = url.GetMD5Hash();
            var cachedFilePath = Path.Combine(cacheDirectory, urlHash);

            this.logger.LogInformation("Looking for file {CachedFilePath}", cachedFilePath);

            var fileStream = await Task.Run(() => this.fileService.ReadFile(cachedFilePath), cancellationToken);

            // If cached file exists, return it
            if (fileStream != null)
            {
                this.logger.LogInformation("File was cached locally ({CachedFilePath}). Read it.", cachedFilePath);
                return fileStream;
            }

            this.logger.LogInformation("No cached file was found for url {Url}. Save it.", url);

            // If there's no cached file, execute a caching func to read a stream
            var newStream = await func();

            // Write a new stream to the disk
            await this.fileService.WriteFile(newStream, cachedFilePath, cancellationToken);

            this.logger.LogInformation("Disk cache file was written to {CachedFilePath}", cachedFilePath);

            newStream.Reset();
            return newStream;
        }
    }
}