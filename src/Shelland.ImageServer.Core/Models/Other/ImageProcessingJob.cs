// Created on 11/02/2021 14:06 by Andrey Laserson

using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Preferences;
using SixLabors.ImageSharp;

namespace Shelland.ImageServer.Core.Models.Other
{
    /// <summary>
    /// Contains information to process a requested image file
    /// </summary>
    public class ImageProcessingJob
    {
        public Image Image { get; set; }

        public ImageProcessingSettingsModel Settings { get; set; }

        public ImageThumbnailParamsModel ThumbnailParams { get; set; }
    }
}