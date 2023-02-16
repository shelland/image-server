// Created on 31/05/2021 19:44 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Data;
using Shelland.ImageServer.DataAccess.Models;

namespace Shelland.ImageServer.DataAccess.Abstract.Repository;

public interface IProcessingProfileRepository
{
    /// <summary>
    /// Returns a list of saved processing profiles
    /// </summary>
    Task<IReadOnlyCollection<ProcessingProfileDbModel>> GetProfiles();

    /// <summary>
    /// Returns a profile by profile ID
    /// </summary>
    Task<ProcessingProfileDbModel?> GetProfileById(Guid id);

    /// <summary>
    /// Adds a new profile
    /// </summary>
    Task<ProcessingProfileDbModel> Create(CreateProcessingProfileContext ctx);
}