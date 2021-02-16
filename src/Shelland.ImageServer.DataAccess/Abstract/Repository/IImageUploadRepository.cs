// Created on 08/02/2021 17:07 by Andrey Laserson

using System;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Data;

namespace Shelland.ImageServer.DataAccess.Abstract.Repository
{
    /// <summary>
    /// Image upload repository gets information about processed images
    /// </summary>
    public interface IImageUploadRepository
    {
        /// <summary>
        /// Returns an upload entity based on ID
        /// </summary>
        /// <param name="id">ID (Guid) of the entity</param>
        /// <returns></returns>
        Task<ImageUploadDbModel> GetById(Guid id);

        /// <summary>
        /// Saves a processing result into local database for further references
        /// </summary>
        /// <param name="dbModel"></param>
        /// <returns></returns>
        Task<ImageUploadDbModel> Create(ImageUploadDbModel dbModel);

        /// <summary>
        /// Deletes (physically) a selected record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);
    }
}