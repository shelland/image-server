﻿// Created on 09/02/2021 21:36 by Andrey Laserson

using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Networking;

/// <summary>
/// Extremely simple HTTP client wrapper to send a webhook request
/// </summary>
public interface INetworkService
{
    /// <summary>
    /// Send a web request
    /// </summary>
    Task MakeRequest(string url, object payload, CancellationToken cancellationToken);

    /// <summary>
    /// Download a file to stream
    /// </summary>
    Task<Stream> DownloadAsStream(string url, CancellationToken cancellationToken);
}