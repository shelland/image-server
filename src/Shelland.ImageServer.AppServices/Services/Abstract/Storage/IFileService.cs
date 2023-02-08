// Created on 08/02/2021 15:42 by Andrey Laserson

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Enums;
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
        StoragePathModel PrepareStoragePath(OutputImageFormat format);

        /// <summary>
        /// Prepares a path object for thumbnails
        /// </summary>
        /// <param name="originalPath"></param>
        /// <param name="format"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        ImageThumbPathsModel PrepareThumbFilePath(StoragePathModel originalPath, OutputImageFormat format, int width, int height);

        /// <summary>
        /// Write a memory stream to the disk
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="filePath"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task WriteFile(Stream stream, string filePath, CancellationToken cancellationToken);

        /// <summary>
        /// Delete selected file from the disk
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        void Delete(IReadOnlyList<string> paths);

        /// <summary>
        /// Read a file from the local file system
        /// </summary>
        /// <param name="path"></param>
        /// <returns>File stream if file exists, null otherwise</returns>
        Stream? ReadFile(string path);
    }
}