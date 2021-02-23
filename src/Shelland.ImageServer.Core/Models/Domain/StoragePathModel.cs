// Created on 08/02/2021 15:55 by Andrey Laserson

using System;

namespace Shelland.ImageServer.Core.Models.Domain
{
    /// <summary>
    /// Represents a single image thumbnail storage
    /// </summary>
    public class StoragePathModel
    {
        public Guid Key { get; set; }

        public string FilePath { get; set; }

        public string UrlPath { get; set; }
    }
}