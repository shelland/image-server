// Created on 08/02/2021 17:09 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Data;
using Shelland.ImageServer.DataAccess.Abstract.Repository;
using Shelland.ImageServer.DataAccess.Context;
using Shelland.ImageServer.DataAccess.Models;

namespace Shelland.ImageServer.DataAccess.Repository;

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
        var upload = await collection.Query().Where(x => x.Id == id).SingleOrDefaultAsync();

        return upload;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public async Task Delete(Guid id)
    {
        var collection = this.context.Database.GetCollection<ImageUploadDbModel>();
        await collection.DeleteAsync(id);
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public async Task<IReadOnlyCollection<ImageUploadDbModel>> GetExpiredUploads(DateTime utcNow)
    {
        var collection = this.context.Database.GetCollection<ImageUploadDbModel>();
        var expiredUploads = await collection.Query()
            .Where(x => x.ExpiresAtUtc != null && x.ExpiresAtUtc <= utcNow)
            .ToListAsync();

        return expiredUploads;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public async Task<ImageUploadDbModel> Create(CreateImageUploadContext ctx)
    {
        var collection = this.context.Database.GetCollection<ImageUploadDbModel>();

        await collection.EnsureIndexAsync(x => x.Id, unique: true);
        await collection.EnsureIndexAsync(x => x.ExpiresAtUtc);

        var dbModel = new ImageUploadDbModel
        {
            Id = ctx.Id,
            OriginalFilePath = ctx.OriginalFilePath,
            Thumbnails = ctx.Thumbnails,
            IpAddress = ctx.IpAddress,
            ExpiresAtUtc = ctx.ExpirationDate,
            CreateDateUtc = ctx.Now
        };

        await collection.InsertAsync(dbModel);

        return dbModel;
    }
}