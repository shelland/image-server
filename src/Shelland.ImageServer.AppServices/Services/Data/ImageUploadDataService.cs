// Created on 14/02/2023 13:28 by shell

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Data;
using Shelland.ImageServer.Core.Models.Data;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Other;
using Shelland.ImageServer.DataAccess.Abstract.Repository;
using Shelland.ImageServer.DataAccess.Models;

namespace Shelland.ImageServer.AppServices.Services.Data;

public class ImageUploadDataService : IImageUploadDataService
{
    private readonly IImageUploadRepository repository;
    private readonly IDateService dateService;
    private readonly IMapper mapper;
    private readonly IIdGenerator idGenerator;

    public ImageUploadDataService(
        IImageUploadRepository repository,
        IDateService dateService,
        IMapper mapper,
        IIdGenerator idGenerator
    )
    {
        this.repository = repository;
        this.dateService = dateService;
        this.mapper = mapper;
        this.idGenerator = idGenerator;
    }

    public async Task<ImageUploadModel> Create(StoragePathModel storagePath, ImageUploadJob job, IReadOnlyCollection<ImageThumbnailResultModel> thumbnails)
    {
        var now = this.dateService.NowUtc;

        var expirationDate = job.Params.Lifetime != null ? 
            now.AddSeconds(job.Params.Lifetime.Value) : 
            (DateTime?)null;

        var dbEntity = await this.repository.Create(new CreateImageUploadContext(
            Id: this.idGenerator.Id(),
            OriginalFilePath: storagePath.UrlPath,
            Now: now,
            Thumbnails: thumbnails,
            IpAddress: job.IpAddress,
            ExpirationDate: expirationDate)
        );

        return this.mapper.Map<ImageUploadDbModel, ImageUploadModel>(dbEntity);
    }

    public async Task<ImageUploadModel?> GetById(Guid id)
    {
        var upload = await this.repository.GetById(id);
        return this.mapper.Map<ImageUploadDbModel?, ImageUploadModel>(upload);
    }

    public async Task Delete(Guid id)
    {
        await this.repository.Delete(id);
    }

    public async Task<IReadOnlyCollection<ImageUploadModel>> GetExpiredUploads()
    {
        var uploads = await this.repository.GetExpiredUploads(this.dateService.NowUtc);
        return this.mapper.Map<IReadOnlyCollection<ImageUploadDbModel>, IReadOnlyCollection<ImageUploadModel>>(uploads);
    }
}