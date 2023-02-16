// Created on 02/03/2021 22:09 by Andrey Laserson

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ImageMagick;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Common;

public interface IImageReadingService
{
    /// <summary>
    /// Loads an image from input stream
    /// </summary>
    Task<MagickImage> Read(Stream stream);

    /// <summary>
    /// Loads an image from URL
    /// </summary>
    Task<MagickImage> Read(string url, CancellationToken cancellationToken);
}