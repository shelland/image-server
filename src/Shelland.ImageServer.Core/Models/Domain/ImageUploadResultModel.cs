// Created on 10/02/2021 0:44 by Andrey Laserson

using System;
using System.Collections.Generic;

namespace Shelland.ImageServer.Core.Models.Domain
{
    public class ImageUploadResultModel
    {
        public Guid Id { get; set; }

        public List<ImageThumbnailResultModel> Thumbnails { get; set; } = new();
    }
}