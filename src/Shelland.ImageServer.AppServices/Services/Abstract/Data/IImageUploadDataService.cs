// Created on 14/02/2023 13:27 by shell

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Other;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Data;

public interface IImageUploadDataService
{
    Task<ImageUploadModel> Create(
        StoragePathModel storagePath,
        ImageUploadJob job,
        IReadOnlyCollection<ImageThumbnailResultModel> thumbnails
    );

    Task<ImageUploadModel?> GetById(Guid id);

    Task Delete(Guid id);

    Task<IReadOnlyCollection<ImageUploadModel>> GetExpiredUploads();
}