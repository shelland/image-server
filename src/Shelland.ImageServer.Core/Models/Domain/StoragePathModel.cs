// Created on 08/02/2021 15:55 by Andrey Laserson

using System;

namespace Shelland.ImageServer.Core.Models.Domain
{
    /// <summary>
    /// Represents a single image thumbnail storage
    /// </summary>
    public record StoragePathModel(
        Guid Key,
        string FilePath,
        string UrlPath
    );
}