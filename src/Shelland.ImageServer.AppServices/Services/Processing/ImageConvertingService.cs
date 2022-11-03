// Created on 15/02/2021 20:32 by Andrey Laserson

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Processing;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using Shelland.ImageServer.Core.Models.Other;
using Shelland.ImageServer.Core.Other;

namespace Shelland.ImageServer.AppServices.Services.Processing
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public class ImageConvertingService : IImageConvertingService
    {
        private readonly IImageWritingService imageWritingService;
        private readonly IImageReadingService imageReadingService;

        public ImageConvertingService(
            IImageWritingService imageWritingService, 
            IImageReadingService imageReadingService)
        {
            this.imageWritingService = imageWritingService;
            this.imageReadingService = imageReadingService;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<string> ImageToBase64(Stream inputStream)
        {
            var streamBytes = await inputStream.ToByteArray();
            var imageBase64String = Convert.ToBase64String(streamBytes);

            return $"{Constants.Base64ImagePrefix}{imageBase64String}";
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task ImageToFormat(Stream inputStream, StreamImageSavingParamsModel savingParams, CancellationToken cancellationToken)
        {
            var inputImage = await this.imageReadingService.Read(inputStream);
            await this.imageWritingService.WriteToStream(inputImage, savingParams, cancellationToken);
        }
    }
}