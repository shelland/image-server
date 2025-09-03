// Created on 10/02/2021 0:45 by Andrey Laserson

using AutoMapper;
using JetBrains.Annotations;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Models.Dto.Response;
using System.Collections.Generic;

namespace Shelland.ImageServer.Infrastructure.Mapping;

[UsedImplicitly]
public class ImageUploadResultModelMapperProfile : Profile
{
    public ImageUploadResultModelMapperProfile()
    {
        CreateMap<ImageThumbnailResultModel, ImageThumbnailResultDto>();
        CreateMap<ImageUploadResultModel, ImageUploadResultDto>()
            .ForMember(x => x.Thumbnails, x => x.MapFrom((src, _, _, ctx) => ctx.Mapper.Map<List<ImageThumbnailResultDto>>(src.Thumbnails)));
    }
}