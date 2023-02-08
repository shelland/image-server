// Created on 31/05/2021 19:44 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Data;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.DataAccess.Abstract.Repository;
using Shelland.ImageServer.DataAccess.Context;

namespace Shelland.ImageServer.DataAccess.Repository
{
    public class ProcessingProfileRepository : IProcessingProfileRepository
    {
        private readonly AppDbContext context;

        public ProcessingProfileRepository(AppDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<IReadOnlyList<ProcessingProfileDbModel>> GetProfiles()
        {
            var collection = this.context.Database.GetCollection<ProcessingProfileDbModel>();
            await collection.EnsureIndexAsync(x => x.ProfileId);

            var profiles = await collection.Query()
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();

            return profiles;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<ProcessingProfileDbModel?> GetProfileById(Guid id)
        {
            var collection = this.context.Database.GetCollection<ProcessingProfileDbModel>();
            await collection.EnsureIndexAsync(x => x.ProfileId);

            var profile = await collection.Query().Where(x => x.ProfileId == id).SingleOrDefaultAsync();
            return profile;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<ProcessingProfileDbModel> Create(ProcessingProfileModel model)
        {
            var collection = this.context.Database.GetCollection<ProcessingProfileDbModel>();
            
            var dbModel = new ProcessingProfileDbModel
            {
                CreateDate = DateTimeOffset.UtcNow,
                Id = model.Id,
                ProfileId = model.ProfileId,
                Name = model.Name,
                Parameters = model.Parameters
            };

            await collection.EnsureIndexAsync(x => x.ProfileId);
            await collection.InsertAsync(dbModel);

            return dbModel;
        }
    }
}