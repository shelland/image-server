// Created on 23/11/2022 9:46 by shell

using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;

namespace Shelland.ImageServer.Infrastructure.Extensions;

public static class LoggingExtensions
{
    public static IServiceCollection AddLogs(this IServiceCollection services)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console().CreateLogger();

        services.AddLogging(e => e.AddSerilog(logger));

        return services;
    }
}