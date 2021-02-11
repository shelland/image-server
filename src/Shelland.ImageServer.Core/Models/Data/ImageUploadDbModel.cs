// Created on 08/02/2021 16:41 by Andrey Laserson

using System;
using System.Collections.Generic;
using Shelland.ImageServer.Core.Models.Base;
using Shelland.ImageServer.Core.Models.Domain;

namespace Shelland.ImageServer.Core.Models.Data
{
    public class ImageUploadDbModel : BaseDbModel
    {
        public ImageUploadDbModel()
        {
            IsActive = true;
            CreateDate = DateTimeOffset.UtcNow;
        }

        public Guid UploadId { get; set; }

        public string IpAddress { get; set; }

        public string OriginalFilePath { get; set; }

        public List<ImageThumbnailResultModel> Thumbnails { get; set; }
    }
}