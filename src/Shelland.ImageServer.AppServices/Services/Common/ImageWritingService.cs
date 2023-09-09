// Created on 04/03/2021 12:36 by Andrey Laserson

#region Usings

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using NetVips;
using Shelland.ImageServer.AppServices.Logic;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.Core.Models.Other;

#endregion

namespace Shelland.ImageServer.AppServices.Services.Common;

/// <summary>
/// <inheritdoc />
/// </summary>
public class ImageWritingService : IImageWritingService
{
    private readonly IFileService fileService;
    private readonly RecyclableMemoryStreamManager recyclableMemoryStreamManager;
    private readonly ILogger<ImageWritingService> logger;

    public ImageWritingService(
        IFileService fileService, 
        RecyclableMemoryStreamManager recyclableMemoryStreamManager, 
        ILogger<ImageWritingService> logger)
    {
        this.fileService = fileService;
        this.recyclableMemoryStreamManager = recyclableMemoryStreamManager;
        this.logger = logger;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public async Task WriteToDisk(Image image, DiskImageSavingParamsModel savingParams, CancellationToken cancellationToken)
    {
        try
        {
            await using var imageStream = await GetOutputStream(image, savingParams, cancellationToken);
            await this.fileService.WriteFile(imageStream, savingParams.Path, cancellationToken);

            this.logger.LogInformation("Image was saved to {Path}", savingParams.Path);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, ex.Message);
            throw new AppFlowException(AppFlowExceptionType.DiskWriteFailed, savingParams.Path);
        }
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public async Task WriteToStream(Image image, StreamImageSavingParamsModel savingParams, CancellationToken cancellationToken)
    {
        try
        {
            await using var imageStream = await GetOutputStream(image, savingParams, cancellationToken);
            await imageStream.CopyToAsync(savingParams.OutputStream, cancellationToken);

            savingParams.OutputStream.Reset();
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, ex.Message);
            throw new AppFlowException(AppFlowExceptionType.DiskWriteFailed);
        }
    }

    #region Private methods

    private async Task<RecyclableMemoryStream> GetOutputStream(Image image, BaseImageSavingParamsModel savingParams, CancellationToken cancellationToken)
    {
        var imageStream = new RecyclableMemoryStream(this.recyclableMemoryStreamManager);
        var outputFormat = savingParams.Format ?? OutputImageFormat.Jpeg;

        image.Format = ToMagickFormat(outputFormat);

        var imageWritingContext = new ImageWritingContext(image.Format == MagickFormat.Jpeg ? 
            new JpegWritingStrategy(savingParams.Quality) : 
            new GenericWritingStrategy()
        );

        await imageWritingContext.Write(image, imageStream, cancellationToken);
        imageStream.Reset();

        return imageStream;
    }

    private static OutputImageFormat ToMagickFormat(OutputImageFormat outputFormat)
    {
        return outputFormat switch
        {
            OutputImageFormat.Gif => MagickFormat.Gif,
            OutputImageFormat.Png => MagickFormat.Png,
            _ => MagickFormat.Jpeg
        };
    }

    #endregion
}