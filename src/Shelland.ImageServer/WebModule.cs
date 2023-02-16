// Created on 14/02/2021 17:50 by Andrey Laserson

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Shelland.ImageServer.Infrastructure.Storage;

namespace Shelland.ImageServer;

public static class WebModule
{
    public static IServiceCollection AddWebModule(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IFileProvider, AppFileProvider>();
        return serviceCollection;
    }
}