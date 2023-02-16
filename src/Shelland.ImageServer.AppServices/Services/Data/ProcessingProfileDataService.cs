// Created on 14/02/2023 13:28 by shell

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Data;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.DataAccess.Abstract.Repository;
using Shelland.ImageServer.DataAccess.Models;

namespace Shelland.ImageServer.AppServices.Services.Data;

public class ProcessingProfileDataService : IProcessingProfileDataService
{
    private readonly IProcessingProfileRepository repository;
    private readonly IMapper mapper;
    private readonly IDateService dateService;
    private readonly IIdGenerator idGenerator;

    public ProcessingProfileDataService(
        IProcessingProfileRepository repository,
        IMapper mapper,
        IDateService dateService,
        IIdGenerator idGenerator
    )
    {
        this.repository = repository;
        this.mapper = mapper;
        this.dateService = dateService;
        this.idGenerator = idGenerator;
    }

    public async Task<IReadOnlyCollection<ProcessingProfileModel>> GetProfiles()
    {
        var profiles = await this.repository.GetProfiles();
        return this.mapper.Map<IReadOnlyCollection<ProcessingProfileModel>>(profiles);
    }

    public async Task<ProcessingProfileModel?> GetById(Guid id)
    {
        var profile = await this.repository.GetProfileById(id);
        return this.mapper.Map<ProcessingProfileModel>(profile);
    }

    public async Task<ProcessingProfileModel> Create(string name, IReadOnlyCollection<ImageThumbnailParamsModel> thumbnailParams)
    {
        var dbEntity = await this.repository.Create(new CreateProcessingProfileContext(
            Id: this.idGenerator.Id(),
            Name: name,
            Now: this.dateService.NowUtc,
            Parameters: thumbnailParams)
        );

        return this.mapper.Map<ProcessingProfileModel>(dbEntity);
    }
}