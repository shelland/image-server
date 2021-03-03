// Created on 09/02/2021 21:36 by Andrey Laserson

using System.IO;
using System.Threading.Tasks;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Networking
{
    /// <summary>
    /// Extremely simple HTTP client wrapper to send a webhook request
    /// </summary>
    public interface INetworkService
    {
        /// <summary>
        /// Send a web request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        Task MakeRequest(string url, object payload);

        /// <summary>
        /// Download a file to stream
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<Stream> DownloadAsStream(string url);
    }
}