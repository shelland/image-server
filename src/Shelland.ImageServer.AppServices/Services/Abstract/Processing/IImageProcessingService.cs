// Created on 08/02/2021 18:27 by Andrey Laserson

using System.IO;
using System.Threading.Tasks;
using ImageMagick;
using Shelland.ImageServer.Core.Models.Other;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Processing
{
    /// <summary>
    /// Image processing service performs a resizing of requested files
    /// </summary>
    public interface IImageProcessingService
    {
        /// <summary>
        /// Performs an image resizing
        /// </summary>
        /// <param name="job">Job description</param>
        /// <returns></returns>
        MagickImage Process(ImageProcessingJob job);

        /// <summary>
        /// Loads an image from input stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<MagickImage> Load(Stream stream);
    }
}