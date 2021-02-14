// Created on 08/02/2021 15:42 by Andrey Laserson

using System.IO;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Other;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Storage
{
    /// <summary>
    /// File service
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Prepares a paths object to be used to save an image
        /// </summary>
        /// <returns></returns>
        StoragePathModel PrepareStoragePath();

        /// <summary>
        /// Prepares a path object for thumbnails
        /// </summary>
        /// <param name="originalPath"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        ImageThumbPathsModel PrepareThumbFilePath(StoragePathModel originalPath, int width, int height);

        /// <summary>
        /// Write a memory stream to the disk
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<bool> WriteFile(Stream stream, string filePath);

        string NormalizeUrl(string originalUrl);
    }
}