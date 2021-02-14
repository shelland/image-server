// Created on 14/02/2021 17:50 by Andrey Laserson

using Autofac;
using Microsoft.Extensions.FileProviders;
using Shelland.ImageServer.Infrastructure.Storage;

namespace Shelland.ImageServer
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AppFileProvider>().As<IFileProvider>().SingleInstance();
            base.Load(builder);
        }
    }
}