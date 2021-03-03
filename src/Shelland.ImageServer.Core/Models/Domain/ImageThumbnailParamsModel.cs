// Created on 10/02/2021 0:40 by Andrey Laserson

using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.Core.Other;

namespace Shelland.ImageServer.Core.Models.Domain
{
    public class ImageThumbnailParamsModel
    {
        public int? Width { get; set; }

        public int? Height { get; set; }

        public ThumbnailEffectType? Effect { get; set; }

        public int? Quality { get; set; }

        public bool IsFixedSize => Width.HasValue && Height.HasValue;

        public WatermarkParams Watermark { get; set; }
    }

    public class WatermarkParams
    {
        public string Url { get; set; }

        public double Opacity { get; set; } = Constants.DefaultWatermarkOpacity;
    }
}