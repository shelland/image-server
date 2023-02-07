// Created on 08/02/2021 15:52 by Andrey Laserson

using Microsoft.Extensions.DependencyInjection;
using Shelland.ImageServer.AppServices.Services.Abstract.Common;
using Shelland.ImageServer.AppServices.Services.Abstract.Networking;
using Shelland.ImageServer.AppServices.Services.Abstract.Processing;
using Shelland.ImageServer.AppServices.Services.Abstract.Storage;
using Shelland.ImageServer.AppServices.Services.Common;
using Shelland.ImageServer.AppServices.Services.Networking;
using Shelland.ImageServer.AppServices.Services.Processing;
using Shelland.ImageServer.AppServices.Services.Storage;

namespace Shelland.ImageServer.AppServices
{
    public static class AppServicesModule
    {
        public static IServiceCollection AddAppServicesModule(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDiskCacheService, DiskCacheService>();
            serviceCollection.AddSingleton<IImageReadingService, ImageReadingService>();
            serviceCollection.AddSingleton<IImageThumbnailService, ImageThumbnailService>();
            serviceCollection.AddSingleton<IImageWritingService, ImageWritingService>();
            serviceCollection.AddSingleton<INetworkService, NetworkService>();
            serviceCollection.AddSingleton<IImageConvertingService, ImageConvertingService>();
            serviceCollection.AddSingleton<IImageProcessingService, ImageProcessingService>();
            serviceCollection.AddSingleton<IFileService, FileService>();
            serviceCollection.AddSingleton<ILinkService, LinkService>();

            return serviceCollection;
        }
    }
}