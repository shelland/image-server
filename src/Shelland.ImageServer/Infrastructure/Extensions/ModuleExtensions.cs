// Created on 17/02/2022 11:57 by shell

using Microsoft.Extensions.DependencyInjection;
using Shelland.ImageServer.AppServices;
using Shelland.ImageServer.DataAccess;
using Shelland.ImageServer.FaceDetection;

namespace Shelland.ImageServer.Infrastructure.Extensions;

public static class ModuleExtensions
{
    public static IServiceCollection AddModules(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddFaceDetectionModule();
        serviceCollection.AddDataAccessModule();
        serviceCollection.AddAppServicesModule();
        serviceCollection.AddWebModule();

        return serviceCollection;
    }
}