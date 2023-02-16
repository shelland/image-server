// Created on 22/02/2021 20:10 by Andrey Laserson

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.FeatureManagement;
using Shelland.ImageServer.Core.Other;

namespace Shelland.ImageServer.Infrastructure.Filters;

public class ServerEnabledFilter : IAsyncActionFilter
{
    private readonly IFeatureManager featureManager;
    
    public ServerEnabledFilter(IFeatureManager featureManager)
    {
        this.featureManager = featureManager;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var isEnabled = await this.featureManager.IsEnabledAsync(Constants.FeatureFlags.Server);

        if (!isEnabled)
        {
            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.ServiceUnavailable;
            return;
        }

        await next();
    }
}