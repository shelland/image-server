// Created on 10/02/2021 0:40 by Andrey Laserson

using AutoMapper;
using JetBrains.Annotations;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Models.Dto.Request;

namespace Shelland.ImageServer.Infrastructure.Mapping;

[UsedImplicitly]
public class ThumbnailModelMapperProfile : Profile
{
    public ThumbnailModelMapperProfile()
    {
        CreateMap<ImageThumbnailParamsDto, ImageThumbnailParamsModel>()
            .ForMember(x => x.Watermark, x => x.MapFrom((src, _, _, ctx) => ctx.Mapper.Map<WatermarkParamsModel>(src.Watermark)));

        CreateMap<WatermarkParamsDto, WatermarkParamsModel>();
    }
}