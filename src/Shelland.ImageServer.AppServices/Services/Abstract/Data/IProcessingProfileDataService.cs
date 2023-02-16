// Created on 14/02/2023 13:27 by shell

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Domain;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Data;

public interface IProcessingProfileDataService
{
    Task<IReadOnlyCollection<ProcessingProfileModel>> GetProfiles();

    Task<ProcessingProfileModel?> GetById(Guid id);

    Task<ProcessingProfileModel> Create(string name, IReadOnlyCollection<ImageThumbnailParamsModel> thumbnailParams);
}