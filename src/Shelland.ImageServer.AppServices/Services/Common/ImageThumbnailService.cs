// Created on 08/02/2021 17:05 by Andrey Laserson

#region Usings

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Processing;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.AppServices.Services.Messaging.Payload;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
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
        private readonly ILinkService linkService;

        private readonly IImageUploadRepository imageUploadRepository;
        private readonly IProcessingProfileRepository processingProfileRepository;
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
            IImageWritingService imageWritingService,
            IProcessingProfileRepository processingProfileRepository,
            ILinkService linkService)
        {
            this.imageProcessingService = imageProcessingService;
            this.fileService = fileService;
            this.imageUploadRepository = imageUploadRepository;
            this.logger = logger;
            this.mediator = mediator;
            this.appSettings = appSettings;
            this.imageReadingService = imageReadingService;
            this.imageWritingService = imageWritingService;
            this.processingProfileRepository = processingProfileRepository;
            this.linkService = linkService;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<ImageUploadResultModel> ProcessThumbnails(ImageUploadJob uploadJob, CancellationToken cancellationToken)
        {
            var storagePath = this.fileService.PrepareStoragePath(uploadJob.Params.OutputFormat);

            var result = new ImageUploadResultModel
            {
                Id = storagePath.Key
            };

            this.logger.LogInformation("A new upload with id {Id} was created", result.Id);

            // Save original file depending on the app settings
            if (this.appSettings.Value.Common.SaveOriginalFile)
            {
                await this.fileService.WriteFile(uploadJob.Stream, storagePath.FilePath, cancellationToken);
                uploadJob.Stream.Reset();

                result.OriginalFileUrl = this.linkService.PrepareWebPath(storagePath.UrlPath);
            }

            // Load a new image and save it since loading it each time is extremely expensive
            // All image processing routines will clone it and use - it is about 10x times faster
            using var sourceImage = await this.imageReadingService.Read(uploadJob.Stream);

            // Process requested thumbnails
            result.Thumbnails = await this.GenerateThumbnails(uploadJob, sourceImage, storagePath, cancellationToken);

            // Send a notification about finished job to all listeners
            await this.mediator.Send(new ImageProcessingFinishedPayload
            {
                Result = result
            }, cancellationToken);

            // Create a database record
            await this.imageUploadRepository.Create(storagePath, result.Thumbnails, uploadJob.IpAddress, uploadJob.Params.Lifetime);

            return result;
        }

        #region Private methods

        /// <summary>
        /// Performs an image handling (resizing, effects, etc) and flushing to disk
        /// </summary>
        /// <param name="uploadJob"></param>
        /// <param name="sourceImage"></param>
        /// <param name="storagePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<List<ImageThumbnailResultModel>> GenerateThumbnails(
            ImageUploadJob uploadJob,
            MagickImage sourceImage,
            StoragePathModel storagePath,
            CancellationToken cancellationToken)
        {
            var resultsList = new List<ImageThumbnailResultModel>();
            var thumbnailParams = await GetThumbnailParams(uploadJob);

            foreach (var thumbParam in thumbnailParams)
            {
                this.logger.LogInformation("Begin a thumbnail processing with params ({Width}, {Height})", thumbParam.Width, thumbParam.Height);

                // Prepare an image processing job
                var job = new ImageProcessingJob
                (
                    Image: sourceImage,
                    Settings: this.appSettings.Value.ImageProcessing,
                    ThumbnailParams: thumbParam
                );

                // Run an image processing job
                using var processedImage = this.imageProcessingService.Process(job);

                // If watermark parameters were provided then apply a watermark
                if (thumbParam.Watermark != null)
                {
                    using var watermarkImage = await this.imageReadingService.Read(thumbParam.Watermark!.Url, cancellationToken);
                    this.imageProcessingService.AddWatermark(processedImage, watermarkImage);
                }

                // Prepare disk paths to be used to save images
                var paths = this.fileService.PrepareThumbFilePath(storagePath, uploadJob.Params.OutputFormat, processedImage.Width, processedImage.Height);

                // Quality can be defined for each thumbnail individually
                // Otherwise a JPEG quality value will be loaded from the application settings or app default value
                var quality = thumbParam.Quality ??
                              this.appSettings.Value.ImageProcessing.JpegQuality ??
                              Constants.DefaultJpegQuality;

                // Save a processed image to the disk
                await this.imageWritingService.WriteToDisk(processedImage, new DiskImageSavingParamsModel
                {
                    Format = uploadJob.Params.OutputFormat,
                    Path = paths.DiskPath,
                    Quality = quality
                }, cancellationToken);

                this.logger.LogInformation("Thumbnail processing finished. File saved as {DiskPath}", paths.DiskPath);

                var thumbnailResult = new ImageThumbnailResultModel
                (
                    Width: processedImage.Width,
                    Height: processedImage.Height,
                    Url: paths.Url,
                    DiskPath: paths.DiskPath
                );

                resultsList.Add(thumbnailResult);
            }

            return resultsList;
        }

        /// <summary>
        /// Returns a thumbnails processing params depending on the provided processing profile
        /// </summary>
        /// <param name="uploadJob"></param>
        /// <returns></returns>
        private async Task<List<ImageThumbnailParamsModel>> GetThumbnailParams(ImageUploadJob uploadJob)
        {
            if (!uploadJob.Params.ProfileId.HasValue)
            {
                return uploadJob.Params.Thumbnails;
            }

            var profile = await this.processingProfileRepository.GetProfileById(uploadJob.Params.ProfileId.Value);

            if (profile != null)
            {
                return profile.Parameters;
            }

            throw new AppFlowException(AppFlowExceptionType.InvalidParameters, uploadJob.Params.ProfileId.Value.ToString());
        }

        #endregion
    }
}