// Created on 08/02/2021 16:01 by Andrey Laserson

using Shelland.ImageServer.Core.Models.Enums;

namespace Shelland.ImageServer.Models.Dto.Request
{
    public record ImageThumbnailParamsDto(
        int? Width,
        int? Height,
        ThumbnailEffectType? Effect,
        int? Quality,
        WatermarkParamsDto? Watermark
    );

    public record WatermarkParamsDto(
        string Url,
        double? Opacity
    );
}