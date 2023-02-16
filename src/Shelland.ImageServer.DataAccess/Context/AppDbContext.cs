// Created on 08/02/2021 16:49 by Andrey Laserson

using System.IO;
using LiteDB.Async;
using Microsoft.Extensions.Options;
using Shelland.ImageServer.Core.Models.Preferences;
using Shelland.ImageServer.Core.Other;
using Shelland.ImageServer.DataAccess.Abstract.Context;

namespace Shelland.ImageServer.DataAccess.Context;

/// <summary>
/// <inheritdoc />
/// </summary>
public class AppDbContext : IAppDbContext<LiteDatabaseAsync>
{
    public AppDbContext(IOptions<AppSettingsModel> options)
    {
        this.Database = new LiteDatabaseAsync(Path.Combine(options.Value.Directory.AppDataDirectory, Constants.AppDatabaseName))
        {
            UtcDate = true
        };
    }

    public LiteDatabaseAsync Database { get; }
}