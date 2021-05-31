// Created on 31/05/2021 19:51 by Andrey Laserson

using AutoMapper;
using Shelland.ImageServer.Core.Models.Data;
using Shelland.ImageServer.Core.Models.Domain;

namespace Shelland.ImageServer.Infrastructure.Mapping
{
    public class ProcessingProfileModelMapperProfile : Profile
    {
        public ProcessingProfileModelMapperProfile()
        {
            CreateMap<ProcessingProfileDbModel, ProcessingProfileModel>().ReverseMap();
        }
    }
}