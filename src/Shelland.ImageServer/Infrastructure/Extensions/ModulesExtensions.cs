// Created on 08/02/2021 20:08 by Andrey Laserson

using Autofac;
using Shelland.ImageServer.AppServices;
using Shelland.ImageServer.DataAccess;

namespace Shelland.ImageServer.Infrastructure.Extensions
{
    public static class ModulesExtensions
    {
        public static void AddModules(this ContainerBuilder builder)
        {
            builder.RegisterModule<DataAccessModule>();
            builder.RegisterModule<AppServicesModule>();
        }
    }
}