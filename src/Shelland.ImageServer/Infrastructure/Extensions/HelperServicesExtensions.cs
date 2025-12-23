// Created on 11/02/2021 21:13 by Andrey Laserson

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Shelland.ImageServer.AppServices;
using Shelland.ImageServer.AppServices.Services.Abstract.Networking;
using Shelland.ImageServer.AppServices.Services.Networking;
using Shelland.ImageServer.Infrastructure.Other;

namespace Shelland.ImageServer.Infrastructure.Extensions;

public static class HelperServicesExtensions
{
    public static IServiceCollection AddHelperServices(this IServiceCollection services)
    {
        services.AddHttpClient<INetworkService, NetworkService>();
        services.AddAutoMapper(x => x.AddMaps(Assembly.GetExecutingAssembly()));

        services.AddMediatR(opts =>
        {
            opts.RegisterServicesFromAssemblyContaining(typeof(Startup))
                .RegisterServicesFromAssemblyContaining(typeof(AppServicesModule));
        });

        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }
}