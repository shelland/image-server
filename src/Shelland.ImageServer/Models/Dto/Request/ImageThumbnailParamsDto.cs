// Created on 08/02/2021 16:01 by Andrey Laserson

using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Enums;

namespace Shelland.ImageServer.Models.Dto.Request;

public record ImageThumbnailParamsDto(
    uint? Width,
    uint? Height,
    ThumbnailEffectType? Effect,
    uint? Quality,
    WatermarkParamsDto? Watermark
)
{
    public ImageThumbnailParamsModel ToDomain() => new()
    {
        Width = Width,
        Height = Height,
        Effect = Effect,
        Quality = Quality,
        Watermark = Watermark?.ToDomain()
    };
};

public record WatermarkParamsDto(
    string Url,
    double? Opacity
)
{
    public WatermarkParamsModel ToDomain() => new()
    {
        Opacity = Opacity ?? 100,
        Url = Url
    };
};