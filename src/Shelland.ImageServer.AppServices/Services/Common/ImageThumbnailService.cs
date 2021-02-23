﻿// Created on 08/02/2021 17:05 by Andrey Laserson

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Processing;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.AppServices.Services.Messaging.Payload;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using Shelland.ImageServer.Core.Models.Data;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Other;
using Shelland.ImageServer.Core.Models.Preferences;
using Shelland.ImageServer.Core.Other;
using Shelland.ImageServer.DataAccess.Abstract.Repository;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

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

        public ImageThumbnailService(
            IImageProcessingService imageProcessingService,
            IFileService fileService,
            IImageUploadRepository imageUploadRepository,
            ILogger<ImageThumbnailService> logger,
            IMediator mediator,
            IOptions<AppSettingsModel> appSettings)
        {
            this.imageProcessingService = imageProcessingService;
            this.fileService = fileService;
            this.imageUploadRepository = imageUploadRepository;
            this.logger = logger;
            this.mediator = mediator;
            this.appSettings = appSettings;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<ImageUploadResultModel> ProcessThumbnails(ImageUploadJob uploadJob)
        {
            var storagePath = this.fileService.PrepareStoragePath();

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
            using var sourceImage = await this.imageProcessingService.Load(uploadJob.Stream);

            // Process requested thumbnails
            foreach (var thumbParam in uploadJob.Params.Thumbnails)
            {
                var thumbnail = await GenerateThumbnails(thumbParam, sourceImage, storagePath);
                result.Thumbnails.Add(thumbnail);
            }

            // Send a notification about finished job to all listeners
            await this.mediator.Send(new ImageProcessingFinishedPayload
            {
                Result = result
            });

            await this.AddDbEntry(storagePath, result.Thumbnails, uploadJob.IpAddress, uploadJob.ExpirationDate);

            return result;
        }

        #region Private methods

        /// <summary>
        /// Performs an image handling (resizing, effects, etc) and flushing to disk
        /// </summary>
        /// <param name="thumbParam"></param>
        /// <param name="sourceImage"></param>
        /// <param name="storagePath"></param>
        /// <returns></returns>
        private async Task<ImageThumbnailResultModel> GenerateThumbnails(
            ImageThumbnailParamsModel thumbParam, 
            Image sourceImage, 
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

            // Prepare disk paths to be used to save images
            var paths = this.fileService.PrepareThumbFilePath(storagePath, processedImage.Width, processedImage.Height);

            // Quality can be defined for each thumbnail individually
            // Otherwise a JPEG quality value will be loaded from the application settings or app default value
            var quality = thumbParam.Quality ??
                          this.appSettings.Value.ImageProcessing.JpegQuality ??
                          Constants.DefaultJpegQuality;

            // Save a processed image to the disk
            await this.SaveImage(processedImage, paths.DiskPath, quality);
            
            this.logger.LogInformation($"Thumbnail processing finished. File saved as {paths.DiskPath}");

            var thumbnailResult = new ImageThumbnailResultModel
            {
                Width = processedImage.Width,
                Height = processedImage.Height,
                Url = paths.Url,
                DiskPath = paths.DiskPath
            };

            return thumbnailResult;
        }

        /// <summary>
        /// Write an image to the disk
        /// </summary>
        /// <param name="image"></param>
        /// <param name="path"></param>
        /// <param name="quality"></param>
        /// <returns></returns>
        private async Task SaveImage(Image image, string path, int quality)
        {
            await using var imageStream = new MemoryStream();

            await image.SaveAsJpegAsync(imageStream, new JpegEncoder
            {
                Quality = quality
            });

            imageStream.Reset();

            await this.fileService.WriteFile(imageStream, path);
        }

        /// <summary>
        /// Create a database entry for this upload
        /// </summary>
        /// <param name="storagePath"></param>
        /// <param name="thumbnails"></param>
        /// <param name="ipAddress"></param>
        /// <param name="expirationDate"></param>
        /// <returns></returns>
        private async Task AddDbEntry(
            StoragePathModel storagePath,
            List<ImageThumbnailResultModel> thumbnails,
            string ipAddress,
            DateTimeOffset? expirationDate)
        {
            var dbModel = new ImageUploadDbModel
            {
                UploadId = storagePath.Key,
                OriginalFilePath = storagePath.FilePath,
                Thumbnails = thumbnails,
                IpAddress = ipAddress,
                ExpiresAt = expirationDate
            };

            await this.imageUploadRepository.Create(dbModel);
        }

        #endregion
    }
}