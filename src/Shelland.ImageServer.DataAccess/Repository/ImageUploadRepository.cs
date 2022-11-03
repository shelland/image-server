// Created on 08/02/2021 17:09 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Data;
using Shelland.ImageServer.Core.Models.Domain;
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
        public async Task<ImageUploadDbModel?> GetById(Guid id)
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

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<List<ImageUploadDbModel>> GetExpiredUploads()
        {
            var collection = this.context.Database.GetCollection<ImageUploadDbModel>();
            var expiredUploads = await collection.Query().Where(x => x.ExpiresAt != null && x.ExpiresAt <= DateTimeOffset.UtcNow).ToListAsync();

            return expiredUploads;
        }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public async Task<ImageUploadDbModel> Create(
            StoragePathModel storagePath, 
            List<ImageThumbnailResultModel> thumbnails, 
            string? ipAddress, int? lifetime)
        {
            DateTimeOffset? expirationDate = lifetime.HasValue ? DateTimeOffset.UtcNow.AddSeconds(lifetime.Value) : null;

            var dbModel = new ImageUploadDbModel
            {
                UploadId = storagePath.Key,
                OriginalFilePath = storagePath.FilePath,
                Thumbnails = thumbnails,
                IpAddress = ipAddress,
                ExpiresAt = expirationDate
            };

            await this.Create(dbModel);

            return dbModel;
        }
    }
}