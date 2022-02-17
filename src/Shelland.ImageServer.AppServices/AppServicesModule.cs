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
            serviceCollection.AddTransient<IDiskCacheService, DiskCacheService>();
            serviceCollection.AddTransient<IImageReadingService, ImageReadingService>();
            serviceCollection.AddTransient<IImageThumbnailService, ImageThumbnailService>();
            serviceCollection.AddTransient<IImageWritingService, ImageWritingService>();
            serviceCollection.AddTransient<INetworkService, NetworkService>();
            serviceCollection.AddTransient<IImageConvertingService, ImageConvertingService>();
            serviceCollection.AddTransient<IImageProcessingService, ImageProcessingService>();
            serviceCollection.AddTransient<IFileService, FileService>();

            return serviceCollection;
        }
    }
}