// Created on 11/02/2021 20:22 by Andrey Laserson

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shelland.ImageServer.AppServices.Services.Abstract.Networking;
using Shelland.ImageServer.AppServices.Services.Messaging.Payload;
using Shelland.ImageServer.Core.Models.Preferences;

namespace Shelland.ImageServer.AppServices.Services.Messaging.Handlers
{
    public class ImageProcessingFinishedHandler : AsyncRequestHandler<ImageProcessingFinishedPayload>
    {
        private readonly IOptions<WebHooksSettingsModel> webHooksOptions;
        private readonly INetworkService networkService;
        private readonly ILogger<ImageProcessingFinishedHandler> logger;

        public ImageProcessingFinishedHandler(
            IOptions<WebHooksSettingsModel> webHooksOptions, 
            INetworkService networkService, 
            ILogger<ImageProcessingFinishedHandler> logger)
        {
            this.webHooksOptions = webHooksOptions;
            this.networkService = networkService;
            this.logger = logger;
        }

        protected override async Task Handle(ImageProcessingFinishedPayload request, CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"Entering an image processing finished handler");

            if (this.webHooksOptions.Value.IsEnabled)
            {
                this.logger.LogInformation($"Sending a web hook to {this.webHooksOptions.Value.PostUrl}");
                await this.networkService.MakeRequest(this.webHooksOptions.Value.PostUrl, request.Result);
            }
        }
    }
}