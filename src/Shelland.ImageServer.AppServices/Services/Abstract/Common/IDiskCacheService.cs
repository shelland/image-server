// Created on 03/03/2021 18:37 by Andrey Laserson

using System;
using System.IO;
using System.Threading.Tasks;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Common
{
    /// <summary>
    /// Disk cache service
    /// </summary>
    public interface IDiskCacheService
    {
        /// <summary>
        /// Checks if file was locally cached. If not, execute a func and save a result stream
        /// </summary>
        /// <param name="url"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        Task<Stream> GetOrAdd(string url, Func<Task<Stream>> func);
    }
}