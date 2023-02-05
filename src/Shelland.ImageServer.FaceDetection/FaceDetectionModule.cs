// Created on 03/02/2023 16:08 by shell

using Microsoft.Extensions.DependencyInjection;
using Shelland.ImageServer.FaceDetection.Services;
using Shelland.ImageServer.FaceDetection.Services.Abstract;

namespace Shelland.ImageServer.FaceDetection;

public static class FaceDetectionModule
{
    public static IServiceCollection AddFaceDetectionModule(this IServiceCollection services)
    {
        services.AddSingleton<IFaceDetectionService, FaceDetectionService>();
        return services;
    }
}