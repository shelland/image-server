// Created on 08/02/2021 17:07 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Data;
using Shelland.ImageServer.DataAccess.Models;

namespace Shelland.ImageServer.DataAccess.Abstract.Repository;

/// <summary>
/// Image upload repository gets information about processed images
/// </summary>
public interface IImageUploadRepository
{
    /// <summary>
    /// Returns an upload entity based on ID
    /// </summary>
    Task<ImageUploadDbModel?> GetById(Guid id);

    /// <summary>
    /// Deletes (physically) a selected record
    /// </summary>
    Task Delete(Guid id);

    /// <summary>
    /// Returns a list of uploads that were expired
    /// </summary>
    Task<IReadOnlyCollection<ImageUploadDbModel>> GetExpiredUploads(DateTime utcNow);

    /// <summary>
    /// Saves a processing result into local database for further references
    /// </summary>
    Task<ImageUploadDbModel> Create(CreateImageUploadContext ctx);
}