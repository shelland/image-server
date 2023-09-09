// Created on 02/03/2021 22:09 by Andrey Laserson

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetVips;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Common;

public interface IImageReadingService
{
    /// <summary>
    /// Loads an image from input stream
    /// </summary>
    Task<Image> Read(Stream stream);

    /// <summary>
    /// Loads an image from URL
    /// </summary>
    Task<Image> Read(string url, CancellationToken cancellationToken);
}