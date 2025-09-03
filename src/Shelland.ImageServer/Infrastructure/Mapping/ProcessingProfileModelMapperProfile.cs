// Created on 31/05/2021 19:51 by Andrey Laserson

using AutoMapper;
using JetBrains.Annotations;
using Shelland.ImageServer.Core.Models.Data;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Models.Dto.Request;

namespace Shelland.ImageServer.Infrastructure.Mapping;

[UsedImplicitly]
public class ProcessingProfileModelMapperProfile : Profile
{
    public ProcessingProfileModelMapperProfile()
    {
        CreateMap<ProcessingProfileDbModel, ProcessingProfileModel>().ReverseMap();
        CreateMap<CreateProcessingProfileRequestDto, ProcessingProfileModel>()
            .ForMember(x => x.Id, x => x.Ignore())
            .ForMember(x => x.CreateDateUtc, x => x.Ignore());
    }
}