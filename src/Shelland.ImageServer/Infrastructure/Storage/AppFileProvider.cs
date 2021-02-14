// Created on 14/02/2021 17:49 by Andrey Laserson

using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Shelland.ImageServer.Core.Models.Preferences;

namespace Shelland.ImageServer.Infrastructure.Storage
{
    /// <summary>
    /// Physical file provider
    /// </summary>
    public class AppFileProvider : IFileProvider
    {
        private readonly IOptions<AppSettingsModel> appSettings;
        private PhysicalFileProvider physicalFileProvider;

        public AppFileProvider(IOptions<AppSettingsModel> appSettings)
        {
            this.appSettings = appSettings;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return this.Provider.GetDirectoryContents(subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return this.Provider.GetFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            return this.Provider.Watch(filter);
        }

        private PhysicalFileProvider Provider
        {
            get { return this.physicalFileProvider ??= new PhysicalFileProvider(this.appSettings.Value.Directory.WorkingDirectory); }
        }
    }
}