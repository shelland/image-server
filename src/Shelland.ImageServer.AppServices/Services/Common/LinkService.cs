// Created on 06/02/2023 18:28 by shell

using Microsoft.Extensions.Options;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.Core.Models.Preferences;

namespace Shelland.ImageServer.AppServices.Services.Common;

public class LinkService : ILinkService
{
    private readonly IOptions<AppSettingsModel> appSettings;

    public LinkService(IOptions<AppSettingsModel> appSettings)
    {
        this.appSettings = appSettings;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public string NormalizeWebPath(string originalUrl)
    {
        var url = $"{this.appSettings.Value.Common.ServerUrl}" +
                  $"{this.appSettings.Value.Common.RoutePrefix}" + originalUrl;

        return url;
    }
}