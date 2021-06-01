// Created on 31/05/2021 19:51 by Andrey Laserson

using AutoMapper;
using Shelland.ImageServer.Core.Models.Data;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Models.Dto.Request;

namespace Shelland.ImageServer.Infrastructure.Mapping
{
    public class ProcessingProfileModelMapperProfile : Profile
    {
        public ProcessingProfileModelMapperProfile()
        {
            CreateMap<ProcessingProfileDbModel, ProcessingProfileModel>().ReverseMap();
            CreateMap<ProcessingProfileDto, ProcessingProfileModel>()
                .ForMember(x => x.ProfileId, x => x.Ignore())
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.CreateDate, x => x.Ignore());
        }
    }
}