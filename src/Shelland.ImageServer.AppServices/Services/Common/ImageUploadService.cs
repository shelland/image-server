// Created on 08/02/2021 17:05 by Andrey Laserson

#region Usings

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
    public class ImageUploadService : IImageUploadService
    {
        private readonly IImageProcessingService imageProcessingService;
        private readonly IFileService fileService;

        private readonly IImageUploadRepository imageUploadRepository;

        private readonly IOptions<AppSettingsModel> appOptions;
        private readonly IOptions<ImageProcessingSettingsModel> imageProcessingOptions;

        private readonly ILogger<ImageUploadService> logger;
        private readonly IMediator mediator;

        public ImageUploadService(
            IImageProcessingService imageProcessingService,
            IFileService fileService,
            IImageUploadRepository imageUploadRepository,
            IOptions<AppSettingsModel> appOptions,
            IOptions<ImageProcessingSettingsModel> imageProcessingOptions,
            ILogger<ImageUploadService> logger, 
            IMediator mediator)
        {
            this.imageProcessingService = imageProcessingService;
            this.fileService = fileService;
            this.imageUploadRepository = imageUploadRepository;
            this.appOptions = appOptions;
            this.imageProcessingOptions = imageProcessingOptions;
            this.logger = logger;
            this.mediator = mediator;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<ImageUploadResultModel> Process(ImageUploadJob uploadJob)
        {
            var storagePath = this.fileService.PrepareStoragePath();

            var result = new ImageUploadResultModel
            {
                Id = storagePath.Key
            };

            this.logger.LogInformation($"A new upload with id {result.Id} was created");

            if (this.appOptions.Value.SaveOriginalFile)
            {
                await this.fileService.WriteFile(uploadJob.Stream, storagePath.FilePath);
                uploadJob.Stream.Reset();
            }

            // Load a new image and save it since loading it each time is extremely expensive
            // All image processing routines will clone it and use - it is about 10x times faster
            using var sourceImage = await this.imageProcessingService.Load(uploadJob.Stream);

            // Process requested thumbnails
            foreach (var thumbParam in uploadJob.Params.Thumbnails)
            {
                await HandleImage(thumbParam, sourceImage, storagePath, result);
            }
            
            await this.mediator.Send(new ImageProcessingFinishedPayload
            {
                Result = result
            });

            await this.AddDbEntry(storagePath, result.Thumbnails, uploadJob.IpAddress);
            
            return result;
        }

        #region Private methods

        /// <summary>
        /// Performs an image handling (resizing, effects, etc) and flushing to disk
        /// </summary>
        /// <param name="thumpParam"></param>
        /// <param name="sourceImage"></param>
        /// <param name="storagePath"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task HandleImage(ImageThumbnailParamsModel thumpParam, Image sourceImage, StoragePathModel storagePath, ImageUploadResultModel result)
        {
            this.logger.LogInformation($"Begin a thumbnail processing with params ({thumpParam.Width}, {thumpParam.Height})");

            var job = new ImageProcessingJob
            {
                Image = sourceImage,
                Settings = this.imageProcessingOptions.Value,
                ThumbnailParams = thumpParam
            };

            var processedImage = this.imageProcessingService.Process(job);
            var paths = this.fileService.PrepareThumbFilePath(storagePath, processedImage.Width, processedImage.Height);

            await this.SaveImage(processedImage, paths.DiskPath);

            result.Thumbnails.Add(new ImageThumbnailResultModel
            {
                Width = processedImage.Width,
                Height = processedImage.Height,
                Url = paths.Url,
                DiskPath = paths.DiskPath
            });

            this.logger.LogInformation($"Thumbnail processing finished. File saved as {paths.DiskPath}");
        }

        /// <summary>
        /// Write an image to the disk
        /// </summary>
        /// <param name="image"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private async Task SaveImage(Image image, string path)
        {
            await using var imageStream = new MemoryStream();

            await image.SaveAsJpegAsync(imageStream, new JpegEncoder
            {
                Quality = this.imageProcessingOptions.Value.JpegQuality ?? Constants.DefaultJpegQuality
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
        /// <returns></returns>
        private async Task AddDbEntry(StoragePathModel storagePath, List<ImageThumbnailResultModel> thumbnails, string ipAddress)
        {
            var dbModel = new ImageUploadDbModel
            {
                UploadId = storagePath.Key,
                OriginalFilePath = storagePath.FilePath,
                Thumbnails = thumbnails,
                IpAddress = ipAddress
            };

            await this.imageUploadRepository.Create(dbModel);
        }
        
        #endregion
    }
}