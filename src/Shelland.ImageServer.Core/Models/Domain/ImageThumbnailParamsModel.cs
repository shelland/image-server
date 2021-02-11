// Created on 10/02/2021 0:40 by Andrey Laserson

using Shelland.ImageServer.Core.Models.Enums;

namespace Shelland.ImageServer.Core.Models.Domain
{
    public class ImageThumbnailParamsModel
    {
        public int? Width { get; set; }

        public int? Height { get; set; }

        public ThumbnailEffectType? Effect { get; set; }
    }
}