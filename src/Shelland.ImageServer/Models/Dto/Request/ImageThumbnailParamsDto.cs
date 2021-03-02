// Created on 08/02/2021 16:01 by Andrey Laserson

using Shelland.ImageServer.Core.Models.Enums;

namespace Shelland.ImageServer.Models.Dto.Request
{
    public class ImageThumbnailParamsDto
    {
        public int? Width { get; set; }

        public int? Height { get; set; }

        public ThumbnailEffectType? Effect { get; set; }

        public int? Quality { get; set; }

        public WatermarkParamsDto Watermark { get; set; }
    }

    public class WatermarkParamsDto
    {
        public string Url { get; set; }

        public double? Opacity { get; set; }
    }
}