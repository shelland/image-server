// Created on 10/02/2021 0:45 by Andrey Laserson

using System.Collections.Generic;
using AutoMapper;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Models.Dto.Response;

namespace Shelland.ImageServer.Infrastructure.Mapping
{
    public class ImageUploadResultModelMapperProfile : Profile
    {
        public ImageUploadResultModelMapperProfile()
        {
            CreateMap<ImageUploadResultModel, ImageUploadResultDto>()
                .ForMember(x => x.Thumbnails, x => x.MapFrom((src, _, _, ctx) => ctx.Mapper.Map<List<ImageThumbnailResultDto>>(src.Thumbnails)));
        }
    }
}