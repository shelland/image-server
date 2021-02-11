// Created on 10/02/2021 0:40 by Andrey Laserson

using AutoMapper;
using JetBrains.Annotations;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Models.Dto.Request;
using Shelland.ImageServer.Models.Dto.Response;

namespace Shelland.ImageServer.Infrastructure.Mapping
{
    [UsedImplicitly]
    public class ThumbnailModelMapperProfile : Profile
    {
        public ThumbnailModelMapperProfile()
        {
            CreateMap<ImageThumbnailParamsDto, ImageThumbnailParamsModel>();
            CreateMap<ImageThumbnailResultModel, ImageThumbnailResultDto>();
        }
    }
}