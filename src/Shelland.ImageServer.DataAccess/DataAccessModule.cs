// Created on 08/02/2021 16:08 by Andrey Laserson

using Microsoft.Extensions.DependencyInjection;
using Shelland.ImageServer.DataAccess.Abstract.Repository;
using Shelland.ImageServer.DataAccess.Context;
using Shelland.ImageServer.DataAccess.Repository;

namespace Shelland.ImageServer.DataAccess
{
    public static class DataAccessModule
    {
        public static IServiceCollection AddDataAccessModule(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<AppDbContext>();

            serviceCollection.AddTransient<IImageUploadRepository, ImageUploadRepository>();
            serviceCollection.AddTransient<IProcessingProfileRepository, ProcessingProfileRepository>();

            return serviceCollection;
        }
    }
}