// Created on 04/03/2021 12:36 by Andrey Laserson

using System;
using System.IO;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.Extensions.Logging;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.Core.Models.Other;

namespace Shelland.ImageServer.AppServices.Services.Common
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public class ImageWritingService : IImageWritingService
    {
        private readonly IFileService fileService;
        private readonly ILogger<ImageWritingService> logger;

        public ImageWritingService(IFileService fileService, ILogger<ImageWritingService> logger)
        {
            this.fileService = fileService;
            this.logger = logger;
        }

        public async Task Write(ImageSavingParamsModel savingParams)
        {
            try
            {
                await using var imageStream = new MemoryStream();
                var outputFormat = savingParams.Format ?? OutputImageFormat.Jpeg;
                
                savingParams.Image.Format = ToMagickFormat(outputFormat);
                savingParams.Image.Quality = savingParams.Quality;

                await savingParams.Image.WriteAsync(imageStream);
                imageStream.Reset();

                await this.fileService.WriteFile(imageStream, savingParams.Path);

                this.logger.LogInformation($"Image was saved to {savingParams.Path}");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex,ex.Message);
                throw new AppFlowException(AppFlowExceptionType.DiskWriteFailed, savingParams.Path);
            }
        }

        private static MagickFormat ToMagickFormat(OutputImageFormat outputFormat)
        {
            var magickFormat = outputFormat switch
            {
                OutputImageFormat.Gif => MagickFormat.Gif,
                OutputImageFormat.Png => MagickFormat.Png,
                _ => MagickFormat.Jpeg
            };

            return magickFormat;
        }
    }
}