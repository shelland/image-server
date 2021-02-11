// Created on 10/02/2021 11:16 by Andrey Laserson

using System.Collections.Generic;
using Shelland.ImageServer.Core.Models.Domain;

namespace Shelland.ImageServer.Core.Models.Other
{
    public class ImageUploadParamsModel
    {
        public List<ImageThumbnailParamsModel> Thumbnails { get; set; }
    }
}