// Created on 23/02/2021 11:28 by Andrey Laserson

using Microsoft.Extensions.DependencyInjection;
using Shelland.ImageServer.Infrastructure.HostedServices;

namespace Shelland.ImageServer.Infrastructure.Extensions;

public static class HostedServicesExtensions
{
    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<UploadExpirationHandlingService>();
        return services;
    }
}