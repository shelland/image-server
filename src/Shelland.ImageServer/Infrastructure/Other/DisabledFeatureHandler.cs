// Created on 29/06/2023 18:44 by shell

using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.FeatureManagement.Mvc;

namespace Shelland.ImageServer.Infrastructure.Other;

public class DisabledFeatureHandler : IDisabledFeaturesHandler
{
    public Task HandleDisabledFeatures(IEnumerable<string> features, ActionExecutingContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
        return Task.CompletedTask;
    }
}