// Created on 08/02/2021 15:52 by Andrey Laserson

using Autofac;

namespace Shelland.ImageServer.AppServices
{
    public class AppServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(x => x.Namespace?.StartsWith("Shelland.ImageServer.AppServices.Services") == true)
                .AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}