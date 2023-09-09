// Created on 02/03/2021 22:10 by Andrey Laserson

#region Usings

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetVips;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Networking;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
using Shelland.ImageServer.Core.Models.Enums;

#endregion

namespace Shelland.ImageServer.AppServices.Services.Common;

/// <summary>
/// <inheritdoc />
/// </summary>
public class ImageReadingService : IImageReadingService
{
    private readonly INetworkService networkService;
    private readonly ILogger<ImageReadingService> logger;
    private readonly IDiskCacheService diskCacheService;

    public ImageReadingService(
        INetworkService networkService,
        ILogger<ImageReadingService> logger,
        IDiskCacheService diskCacheService)
    {
        this.networkService = networkService;
        this.logger = logger;
        this.diskCacheService = diskCacheService;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public async Task<Image> Read(Stream stream)
    {
        try
        {
            return await Task.Run(() => Image.NewFromStream(stream));
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, ex.Message);
            throw new AppFlowException(AppFlowExceptionType.InvalidImageFormat);
        }
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public async Task<Image> Read(string url, CancellationToken cancellationToken)
    {
        try
        {
            await using var imageStream = await this.diskCacheService.GetOrAdd(url, async () => 
                await this.networkService.DownloadAsStream(url, cancellationToken), cancellationToken);

            return await this.Read(imageStream);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, ex.Message);
            throw new AppFlowException(AppFlowExceptionType.InvalidImageFormat);
        }
    }
}