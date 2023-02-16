// Created on 09/02/2021 21:37 by Andrey Laserson

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Shelland.ImageServer.AppServices.Services.Abstract.Networking;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
using Shelland.ImageServer.Core.Models.Enums;

namespace Shelland.ImageServer.AppServices.Services.Networking;

/// <summary>
/// <inheritdoc />
/// </summary>
public class NetworkService : INetworkService
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly ILogger<NetworkService> logger;

    public NetworkService(IHttpClientFactory httpClientFactory, ILogger<NetworkService> logger)
    {
        this.httpClientFactory = httpClientFactory;
        this.logger = logger;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public async Task MakeRequest(string url, object payload, CancellationToken cancellationToken)
    {
        try
        {
            using var client = this.httpClientFactory.CreateClient();
            await client.PostAsJsonAsync(url, payload, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, ex.Message);
            throw new AppFlowException(AppFlowExceptionType.NetworkCallFailed);
        }
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public async Task<Stream> DownloadAsStream(string url, CancellationToken cancellationToken)
    {
        try
        {
            using var client = this.httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(2);

            return await client.GetStreamAsync(url, cancellationToken);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, ex.Message);
            throw new AppFlowException(AppFlowExceptionType.NetworkCallFailed);
        }
    }
}