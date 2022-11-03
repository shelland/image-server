// Created on 10/02/2021 11:16 by Andrey Laserson

using System;
using System.Collections.Generic;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Enums;

namespace Shelland.ImageServer.Core.Models.Other
{
    public class ImageUploadParamsModel
    {
        /// <summary>
        /// Input thumbnails
        /// </summary>
        public List<ImageThumbnailParamsModel> Thumbnails { get; set; } = new();

        /// <summary>
        /// Image lifetime
        /// </summary>
        public int? Lifetime { get; set; }

        /// <summary>
        /// Output image format. JPEG is used by default
        /// </summary>
        public OutputImageFormat OutputFormat { get; set; } = OutputImageFormat.Jpeg;

        /// <summary>
        /// Profile identifier
        /// </summary>
        public Guid? ProfileId { get; set; }
    }
}