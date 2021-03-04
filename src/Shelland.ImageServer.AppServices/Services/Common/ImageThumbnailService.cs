// Created on 08/02/2021 17:05 by Andrey Laserson

#region Usings

using System.Threading.Tasks;
using ImageMagick;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Processing;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.AppServices.Services.Messaging.Payload;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.Core.Models.Other;
using Shelland.ImageServer.Core.Models.Preferences;
using Shelland.ImageServer.Core.Other;
using Shelland.ImageServer.DataAccess.Abstract.Repository;

#endregion

namespace Shelland.ImageServer.AppServices.Services.Common
{
    public class ImageThumbnailService : IImageThumbnailService
    {
        private readonly IImageProcessingService imageProcessingService;
        private readonly IFileService fileService;

        private readonly IImageUploadRepository imageUploadRepository;
        private readonly IOptions<AppSettingsModel> appSettings;

        private readonly ILogger<ImageThumbnailService> logger;
        private readonly IMediator mediator;

        private readonly IImageReadingService imageReadingService;
        private readonly IImageWritingService imageWritingService;

        public ImageThumbnailService(
            IImageProcessingService imageProcessingService,
            IFileService fileService,
            IImageUploadRepository imageUploadRepository,
            ILogger<ImageThumbnailService> logger,
            IMediator mediator,
            IOptions<AppSettingsModel> appSettings,
            IImageReadingService imageReadingService,
            IImageWritingService imageWritingService)
        {
            this.imageProcessingService = imageProcessingService;
            this.fileService = fileService;
            this.imageUploadRepository = imageUploadRepository;
            this.logger = logger;
            this.mediator = mediator;
            this.appSettings = appSettings;
            this.imageReadingService = imageReadingService;
            this.imageWritingService = imageWritingService;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<ImageUploadResultModel> ProcessThumbnails(ImageUploadJob uploadJob)
        {
            var storagePath = this.fileService.PrepareStoragePath(uploadJob.Params.OutputFormat);

            var result = new ImageUploadResultModel
            {
                Id = storagePath.Key
            };

            this.logger.LogInformation($"A new upload with id {result.Id} was created");

            // Save original file depending on the app settings
            if (this.appSettings.Value.Common.SaveOriginalFile)
            {
                await this.fileService.WriteFile(uploadJob.Stream, storagePath.FilePath);
                uploadJob.Stream.Reset();

                result.OriginalFileUrl = this.fileService.NormalizeWebPath(storagePath.UrlPath);
            }

            // Load a new image and save it since loading it each time is extremely expensive
            // All image processing routines will clone it and use - it is about 10x times faster
            using var sourceImage = await this.imageReadingService.Read(uploadJob.Stream);

            // Process requested thumbnails
            foreach (var thumbParam in uploadJob.Params.Thumbnails)
            {
                var thumbnail = await GenerateThumbnails(thumbParam, uploadJob.Params.OutputFormat, sourceImage, storagePath);
                result.Thumbnails.Add(thumbnail);
            }

            // Send a notification about finished job to all listeners
            await this.mediator.Send(new ImageProcessingFinishedPayload
            {
                Result = result
            });

            // Create a database record
            await this.imageUploadRepository.Create(storagePath, result.Thumbnails, uploadJob.IpAddress, uploadJob.Params.Lifetime);

            return result;
        }

        #region Private methods

        /// <summary>
        /// Performs an image handling (resizing, effects, etc) and flushing to disk
        /// </summary>
        /// <param name="thumbParam"></param>
        /// <param name="outputImageFormat"></param>
        /// <param name="sourceImage"></param>
        /// <param name="storagePath"></param>
        /// <returns></returns>
        private async Task<ImageThumbnailResultModel> GenerateThumbnails(
            ImageThumbnailParamsModel thumbParam,
            OutputImageFormat outputImageFormat,
            MagickImage sourceImage,
            StoragePathModel storagePath)
        {
            this.logger.LogInformation($"Begin a thumbnail processing with params ({thumbParam.Width}, {thumbParam.Height})");

            // Prepare an image processing job
            var job = new ImageProcessingJob
            {
                Image = sourceImage,
                Settings = this.appSettings.Value.ImageProcessing,
                ThumbnailParams = thumbParam
            };

            // Run an image processing job
            var processedImage = this.imageProcessingService.Process(job);

            // If watermark parameters were provided then apply a watermark
            if (thumbParam.Watermark != null)
            {
                using var watermarkImage = await this.imageReadingService.Read(thumbParam.Watermark.Url);
                processedImage = this.imageProcessingService.AddWatermark(processedImage, watermarkImage);
            }

            // Prepare disk paths to be used to save images
            var paths = this.fileService.PrepareThumbFilePath(storagePath, outputImageFormat, processedImage.Width, processedImage.Height);

            // Quality can be defined for each thumbnail individually
            // Otherwise a JPEG quality value will be loaded from the application settings or app default value
            var quality = thumbParam.Quality ??
                          this.appSettings.Value.ImageProcessing.JpegQuality ??
                          Constants.DefaultJpegQuality;

            // Save a processed image to the disk
            await this.imageWritingService.Write(new ImageSavingParamsModel
            {
                Format = outputImageFormat,
                Image = processedImage,
                Path = paths.DiskPath,
                Quality = quality
            });

            this.logger.LogInformation($"Thumbnail processing finished. File saved as {paths.DiskPath}");

            var thumbnailResult = new ImageThumbnailResultModel
            {
                Width = processedImage.Width,
                Height = processedImage.Height,
                Url = paths.Url,
                DiskPath = paths.DiskPath
            };

            processedImage.Dispose();

            return thumbnailResult;
        }

        #endregion
    }
}