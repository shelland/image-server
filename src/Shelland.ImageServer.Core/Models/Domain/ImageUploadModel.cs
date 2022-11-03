// Created on 31/05/2021 19:50 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.Linq;
using Shelland.ImageServer.Core.Models.Base;

namespace Shelland.ImageServer.Core.Models.Domain
{
    public class ImageUploadModel : BaseModel
    {
        /// <summary>
        /// Unique upload ID
        /// </summary>
        public Guid UploadId { get; set; }

        /// <summary>
        /// User IP address
        /// </summary>
        public string IpAddress { get; set; } = string.Empty;

        /// <summary>
        /// Original file path
        /// </summary>
        public string OriginalFilePath { get; set; } = string.Empty;

        /// <summary>
        /// Generated thumbnails
        /// </summary>
        public List<ImageThumbnailResultModel> Thumbnails { get; set; } = new();

        /// <summary>
        /// Expiration date
        /// </summary>
        public DateTimeOffset? ExpiresAt { get; set; }

        /// <summary>
        /// Returns a list of disk paths for original image (if exists) and generated thumbnails
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllFilePaths()
        {
            var paths = new List<string>();

            if (!string.IsNullOrEmpty(OriginalFilePath))
            {
                paths.Add(OriginalFilePath);
            }

            paths.AddRange(Thumbnails.Select(x => x.DiskPath));

            return paths;
        }
    }
}