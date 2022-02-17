// Created on 08/02/2021 15:52 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.Core.Models.Other;
using Shelland.ImageServer.Core.Models.Preferences;

namespace Shelland.ImageServer.AppServices.Services.Storage
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public class FileService : IFileService
    {
        private readonly IOptions<AppSettingsModel> appSettings;
        private readonly ILogger<FileService> logger;

        public FileService(
            ILogger<FileService> logger,
            IOptions<AppSettingsModel> appSettings)
        {
            this.logger = logger;
            this.appSettings = appSettings;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public StoragePathModel PrepareStoragePath(OutputImageFormat format)
        {
            var uploadKeyGuid = Guid.NewGuid();
            var uploadKey = uploadKeyGuid.ToString("N");

            var basePath = this.appSettings.Value.Directory.WorkingDirectory;

            var segmentsPath = Path.Combine(
                uploadKey[0].ToString(),
                uploadKey[1].ToString(),
                uploadKey[2].ToString());

            var baseDirectoryPath = Path.Combine(basePath, segmentsPath);
            var finalPath = $"{Path.Combine(baseDirectoryPath, uploadKey)}.{format.GetImageFormat()}";

            var urlPath = $"/{segmentsPath}/{uploadKey}.{format.GetImageFormat()}";

            // If storage directory doesn't exists, create it
            if (!Directory.Exists(baseDirectoryPath))
            {
                Directory.CreateDirectory(baseDirectoryPath);
            }

            return new StoragePathModel
            {
                Key = uploadKeyGuid,
                FilePath = finalPath,
                UrlPath = urlPath
            };
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public ImageThumbPathsModel PrepareThumbFilePath(StoragePathModel originalPath, OutputImageFormat format, int width, int height)
        {
            // Prepare a disk path
            var diskPath = $"{Path.ChangeExtension(originalPath.FilePath, null)}_thumb_{width}x{height}.{format.GetImageFormat()}";

            // Prepare an URL path
            var url = this.NormalizeWebPath(Path.ChangeExtension(originalPath.UrlPath, null)) +
                      $"_thumb_{width}x{height}.{format.GetImageFormat()}";

            return new ImageThumbPathsModel
            {
                DiskPath = diskPath,
                Url = url
            };
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task WriteFile(Stream stream, string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    // It's okay if file exists, really.

                    this.logger.LogWarning($"File already exists {filePath}. Skipping...");
                    return;
                }

                await using var fileStream = new FileStream(filePath, FileMode.CreateNew);

                await stream.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "WriteFile failed for {0}", filePath);
                throw new AppFlowException(AppFlowExceptionType.DiskWriteFailed, filePath);
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public string NormalizeWebPath(string originalUrl)
        {
            var url = $"{this.appSettings.Value.Common.ServerUrl}" +
                      $"{this.appSettings.Value.Common.RoutePrefix}" + originalUrl;

            return url;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public void Delete(List<string> paths)
        {
            try
            {
                foreach (var path in paths.Where(File.Exists))
                {
                    File.Delete(path);
                    this.logger.LogInformation($"File {path} was deleted");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw new AppFlowException(AppFlowExceptionType.DiskWriteFailed);
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<Stream> ReadFile(string path)
        {
            var isFileExists = File.Exists(path);

            if (!isFileExists)
            {
                return null;
            }

            var fileStream = new FileStream(path, FileMode.Open);
            
            return await Task.FromResult(fileStream);
        }
    }
}