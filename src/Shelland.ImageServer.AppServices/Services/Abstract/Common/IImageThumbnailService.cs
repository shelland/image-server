// Created on 08/02/2021 17:05 by Andrey Laserson

using System.Threading;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Other;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Common
{
    public interface IImageThumbnailService
    {
        /// <summary>
        /// Starts a processing of uploaded image
        /// </summary>
        /// <param name="uploadJob"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ImageUploadResultModel> ProcessThumbnails(ImageUploadJob uploadJob, CancellationToken cancellationToken);
    }
}