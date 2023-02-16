// Created on 31/05/2021 19:44 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Models.Data;
using Shelland.ImageServer.DataAccess.Abstract.Repository;
using Shelland.ImageServer.DataAccess.Context;
using Shelland.ImageServer.DataAccess.Models;

namespace Shelland.ImageServer.DataAccess.Repository;

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
    public async Task<IReadOnlyCollection<ProcessingProfileDbModel>> GetProfiles()
    {
        var collection = this.context.Database.GetCollection<ProcessingProfileDbModel>();

        var profiles = await collection.Query()
            .OrderByDescending(x => x.CreateDateUtc)
            .ToListAsync();

        return profiles;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public async Task<ProcessingProfileDbModel?> GetProfileById(Guid id)
    {
        var collection = this.context.Database.GetCollection<ProcessingProfileDbModel>();
        var profile = await collection.Query().Where(x => x.Id == id).SingleOrDefaultAsync();

        return profile;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public async Task<ProcessingProfileDbModel> Create(CreateProcessingProfileContext ctx)
    {
        var collection = this.context.Database.GetCollection<ProcessingProfileDbModel>();
        await collection.EnsureIndexAsync(x => x.Id, unique: true);

        var dbModel = new ProcessingProfileDbModel
        {
            CreateDateUtc = ctx.Now,
            Id = ctx.Id,
            Name = ctx.Name,
            Parameters = ctx.Parameters
        };

        await collection.InsertAsync(dbModel);

        return dbModel;
    }
}