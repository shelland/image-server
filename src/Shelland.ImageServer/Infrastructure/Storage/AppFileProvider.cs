// Created on 14/02/2021 17:49 by Andrey Laserson

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Shelland.ImageServer.Core.Models.Preferences;

namespace Shelland.ImageServer.Infrastructure.Storage;

/// <summary>
/// Physical file provider
/// </summary>
public class AppFileProvider : IFileProvider
{
    private readonly PhysicalFileProvider provider;

    public AppFileProvider(IOptions<AppSettingsModel> appSettings)
    {
        this.provider = new PhysicalFileProvider(appSettings.Value.Directory.WorkingDirectory);
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        return this.provider.GetDirectoryContents(subpath);
    }

    public IFileInfo GetFileInfo(string subpath)
    {
        return this.provider.GetFileInfo(subpath);
    }

    public IChangeToken Watch(string filter)
    {
        return this.provider.Watch(filter);
    }
}