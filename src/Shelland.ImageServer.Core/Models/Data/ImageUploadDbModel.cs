// Created on 08/02/2021 16:41 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.Linq;
using Shelland.ImageServer.Core.Models.Base;
using Shelland.ImageServer.Core.Models.Domain;

namespace Shelland.ImageServer.Core.Models.Data
{
    /// <summary>
    /// Database entity that represents a thumbnail processing result
    /// </summary>
    public class ImageUploadDbModel : BaseDbModel
    {
        public ImageUploadDbModel()
        {
            CreateDate = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Unique upload ID
        /// </summary>
        public Guid UploadId { get; set; }

        /// <summary>
        /// User IP address
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// Original file path
        /// </summary>
        public string OriginalFilePath { get; set; }

        /// <summary>
        /// Generated thumbnails
        /// </summary>
        public List<ImageThumbnailResultModel> Thumbnails { get; set; }

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