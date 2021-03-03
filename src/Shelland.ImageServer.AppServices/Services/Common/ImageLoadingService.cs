// Created on 02/03/2021 22:10 by Andrey Laserson

using System;
using System.IO;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.Extensions.Logging;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Networking;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
using Shelland.ImageServer.Core.Models.Enums;

namespace Shelland.ImageServer.AppServices.Services.Common
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public class ImageLoadingService : IImageLoadingService
    {
        private readonly INetworkService networkService;
        private readonly ILogger<ImageLoadingService> logger;
        private readonly IDiskCacheService diskCacheService;

        public ImageLoadingService(
            INetworkService networkService,
            ILogger<ImageLoadingService> logger,
            IDiskCacheService diskCacheService)
        {
            this.networkService = networkService;
            this.logger = logger;
            this.diskCacheService = diskCacheService;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<MagickImage> Load(Stream stream)
        {
            try
            {
                var image = new MagickImage();
                await image.ReadAsync(stream);

                return image;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw new AppFlowException(AppFlowExceptionType.InvalidImageFormat);
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<MagickImage> Load(string url)
        {
            try
            {
                await using var imageStream = await this.diskCacheService.GetOrAdd(url, async () => 
                    await this.networkService.DownloadAsStream(url));

                return await this.Load(imageStream);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw new AppFlowException(AppFlowExceptionType.InvalidImageFormat);
            }
        }
    }
}