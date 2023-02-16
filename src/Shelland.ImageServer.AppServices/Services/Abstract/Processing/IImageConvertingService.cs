// Created on 15/02/2021 20:31 by Andrey Laserson

using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Other;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Processing;

/// <summary>
/// Image converting service
/// </summary>
public interface IImageConvertingService
{
    /// <summary>
    /// Converts an input stream to a base64 representation that is usable to display
    /// </summary>
    Task<string> ImageToBase64(Stream inputStream);

    /// <summary>
    /// Converts an input image to the output format
    /// </summary>
    Task ImageToFormat(Stream inputStream, StreamImageSavingParamsModel savingParams, CancellationToken cancellationToken);
}