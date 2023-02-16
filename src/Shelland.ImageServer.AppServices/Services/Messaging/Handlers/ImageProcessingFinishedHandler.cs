// Created on 11/02/2021 20:22 by Andrey Laserson

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shelland.ImageServer.AppServices.Services.Abstract.Networking;
using Shelland.ImageServer.AppServices.Services.Messaging.Payload;
using Shelland.ImageServer.Core.Models.Preferences;

namespace Shelland.ImageServer.AppServices.Services.Messaging.Handlers;

public class ImageProcessingFinishedHandler : IRequestHandler<ImageProcessingFinishedPayload>
{
    private readonly IOptions<AppSettingsModel> appSettings;
    private readonly INetworkService networkService;
    private readonly ILogger<ImageProcessingFinishedHandler> logger;

    public ImageProcessingFinishedHandler(
        INetworkService networkService,
        ILogger<ImageProcessingFinishedHandler> logger,
        IOptions<AppSettingsModel> appSettings)
    {
        this.networkService = networkService;
        this.logger = logger;
        this.appSettings = appSettings;
    }

    public async Task Handle(ImageProcessingFinishedPayload request, CancellationToken cancellationToken)
    {
        this.logger.LogInformation("Entering an image processing finished handler for {UploadId}", request.Result.Id);

        var webHooksOptions = this.appSettings.Value.WebHooks;

        if (webHooksOptions.IsEnabled)
        {
            this.logger.LogInformation("Sending a web hook to {PostUrl}", webHooksOptions.PostUrl);
            await this.networkService.MakeRequest(webHooksOptions.PostUrl, request.Result, cancellationToken);
        }
    }
}