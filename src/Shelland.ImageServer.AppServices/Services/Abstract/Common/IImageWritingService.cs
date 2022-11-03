// Created on 04/03/2021 12:37 by Andrey Laserson

using System.Threading;
using System.Threading.Tasks;
using ImageMagick;
using Shelland.ImageServer.Core.Models.Other;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Common
{
    public interface IImageWritingService
    {
        /// <summary>
        /// Writes an image to the destination paths using provided parameters
        /// </summary>
        /// <param name="image"></param>
        /// <param name="savingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task WriteToDisk(MagickImage image, DiskImageSavingParamsModel savingParams, CancellationToken cancellationToken);

        /// <summary>
        /// Writes an image to the provided stream
        /// </summary>
        /// <param name="image"></param>
        /// <param name="savingParams"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task WriteToStream(MagickImage image, StreamImageSavingParamsModel savingParams, CancellationToken cancellationToken);
    }
}