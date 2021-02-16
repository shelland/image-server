// Created on 08/02/2021 17:09 by Andrey Laserson

using System;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Data;
using Shelland.ImageServer.DataAccess.Abstract.Repository;
using Shelland.ImageServer.DataAccess.Context;

namespace Shelland.ImageServer.DataAccess.Repository
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public class ImageUploadRepository : IImageUploadRepository
    {
        private readonly AppDbContext context;

        public ImageUploadRepository(AppDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<ImageUploadDbModel> GetById(Guid id)
        {
            var collection = this.context.Database.GetCollection<ImageUploadDbModel>();
            await collection.EnsureIndexAsync(x => x.UploadId);

            var upload = await collection.Query().Where(x => x.UploadId == id).FirstOrDefaultAsync();

            return upload;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<ImageUploadDbModel> Create(ImageUploadDbModel dbModel)
        {
            var collection = this.context.Database.GetCollection<ImageUploadDbModel>();

            await collection.EnsureIndexAsync(x => x.UploadId);
            await collection.InsertAsync(dbModel);

            return dbModel;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task Delete(int id)
        {
            var collection = this.context.Database.GetCollection<ImageUploadDbModel>();
            await collection.DeleteAsync(id);
        }
    }
}