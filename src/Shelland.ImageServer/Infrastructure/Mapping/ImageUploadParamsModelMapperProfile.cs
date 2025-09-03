// Created on 10/02/2021 11:17 by Andrey Laserson

using System.Collections.Generic;
using AutoMapper;
using JetBrains.Annotations;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Other;
using Shelland.ImageServer.Models.Dto.Request;

namespace Shelland.ImageServer.Infrastructure.Mapping;

[UsedImplicitly]
public class ImageUploadParamsModelMapperProfile : Profile
{
    public ImageUploadParamsModelMapperProfile()
    {
        CreateMap<ImageUploadParamsDto, ImageUploadParamsModel>()
            .ForMember(x => x.Thumbnails, x => x.MapFrom((src, _, _, ctx) => ctx.Mapper.Map<List<ImageThumbnailParamsModel>>(src.Thumbnails)));
    }
}