// Created on 10/02/2021 0:40 by Andrey Laserson

using AutoMapper;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Models.Dto.Request;
using Shelland.ImageServer.Models.Dto.Response;

namespace Shelland.ImageServer.Infrastructure.Mapping
{
    public class ThumbnailModelMapperProfile : Profile
    {
        public ThumbnailModelMapperProfile()
        {
            CreateMap<ImageThumbnailParamsDto, ImageThumbnailParamsModel>()
                .ForMember(x => x.Watermark, x => x.MapFrom((src, _, _, ctx) => ctx.Mapper.Map<WatermarkParams>(src.Watermark)));

            CreateMap<ImageThumbnailResultModel, ImageThumbnailResultDto>();
            CreateMap<WatermarkParamsDto, WatermarkParams>();
        }
    }
}