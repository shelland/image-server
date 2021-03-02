// Created on 02/03/2021 22:09 by Andrey Laserson

using System.IO;
using System.Threading.Tasks;
using ImageMagick;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Common
{
    public interface IImageLoadingService
    {
        /// <summary>
        /// Loads an image from input stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<MagickImage> Load(Stream stream);

        /// <summary>
        /// Loads an image from URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<MagickImage> Load(string url);
    }
}