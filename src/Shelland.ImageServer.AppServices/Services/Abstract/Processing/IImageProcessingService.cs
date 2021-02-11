// Created on 08/02/2021 18:27 by Andrey Laserson

using System.IO;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Other;
using SixLabors.ImageSharp;

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
        Image Process(ImageProcessingJob job);
        
        /// <summary>
        /// Loads an image from input stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<Image> Load(Stream stream);
    }
}