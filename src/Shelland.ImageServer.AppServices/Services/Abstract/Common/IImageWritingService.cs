// Created on 04/03/2021 12:37 by Andrey Laserson

using System.Threading;
using System.Threading.Tasks;
using NetVips;
using Shelland.ImageServer.Core.Models.Other;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Common;

public interface IImageWritingService
{
    /// <summary>
    /// Writes an image to the destination paths using provided parameters
    /// </summary>
    Task WriteToDisk(Image image, DiskImageSavingParamsModel savingParams, CancellationToken cancellationToken);

    /// <summary>
    /// Writes an image to the provided stream
    /// </summary>
    Task WriteToStream(Image image, StreamImageSavingParamsModel savingParams, CancellationToken cancellationToken);
}