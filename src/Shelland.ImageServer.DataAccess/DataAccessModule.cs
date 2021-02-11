// Created on 08/02/2021 16:08 by Andrey Laserson

using Autofac;
using Shelland.ImageServer.DataAccess.Context;

namespace Shelland.ImageServer.DataAccess
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppDbContext>().SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.Namespace == "Shelland.ImageServer.DataAccess.Repository")
                .AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}