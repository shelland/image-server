// Created on 04/03/2021 12:36 by Andrey Laserson

#region Usings

using System;
using System.IO;
using System.Threading.Tasks;
using ImageMagick;
using Microsoft.Extensions.Logging;
using Shelland.ImageServer.AppServices.Logic;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.Core.Models.Other;

#endregion

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

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task WriteToDisk(MagickImage image, DiskImageSavingParamsModel savingParams)
        {
            try
            {
                await using var imageStream = await GetOutputStream(image, savingParams);
                await this.fileService.WriteFile(imageStream, savingParams.Path);

                this.logger.LogInformation($"Image was saved to {savingParams.Path}");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw new AppFlowException(AppFlowExceptionType.DiskWriteFailed, savingParams.Path);
            }
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task WriteToStream(MagickImage image, StreamImageSavingParamsModel savingParams)
        {
            try
            {
                await using var imageStream = await GetOutputStream(image, savingParams);
                await imageStream.CopyToAsync(savingParams.OutputStream);

                savingParams.OutputStream.Reset();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw new AppFlowException(AppFlowExceptionType.DiskWriteFailed);
            }
        }

        #region Private methods

        private static async Task<MemoryStream> GetOutputStream(MagickImage image, BaseImageSavingParamsModel savingParams)
        {
            var imageStream = new MemoryStream();
            var outputFormat = savingParams.Format ?? OutputImageFormat.Jpeg;

            image.Format = ToMagickFormat(outputFormat);

            var imageWritingContext = new ImageWritingContext(image.Format == MagickFormat.Jpeg ? 
                new JpegWritingStrategy(savingParams.Quality) : 
                new GenericWritingStrategy());

            await imageWritingContext.Write(image, imageStream);

            imageStream.Reset();

            return imageStream;
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

        #endregion
    }
}