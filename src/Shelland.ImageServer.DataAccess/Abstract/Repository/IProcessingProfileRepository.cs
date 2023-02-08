// Created on 31/05/2021 19:44 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Data;
using Shelland.ImageServer.Core.Models.Domain;

namespace Shelland.ImageServer.DataAccess.Abstract.Repository
{
    public interface IProcessingProfileRepository
    {
        /// <summary>
        /// Returns a list of saved processing profiles
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyList<ProcessingProfileDbModel>> GetProfiles();

        /// <summary>
        /// Returns a profile by profile ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProcessingProfileDbModel?> GetProfileById(Guid id);

        /// <summary>
        /// Adds a new profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ProcessingProfileDbModel> Create(ProcessingProfileModel model);
    }
}