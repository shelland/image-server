// Created on 23/12/2025 19:19 by Laserson

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
using Shelland.ImageServer.Infrastructure.ModelBinding;
using Shelland.ImageServer.Models.Dto.Response;
using System;
using System.Threading;
using System.Threading.Tasks;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using static System.Net.Mime.MediaTypeNames;

namespace Shelland.ImageServer.Infrastructure.Other;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = Application.Json;

        var feature = httpContext.Features.Get<IExceptionHandlerFeature>().NotNull();

        ErrorResponse response;

        if (exception is AppFlowException appFlowException)
        {
            response = new ErrorResponse(
                Message: appFlowException.Message,
                Path: feature.Path,
                Meta: appFlowException.Parameter
            );
        }
        else
        {
            response = new ErrorResponse(
                Message: feature.Error.Message,
                Path: feature.Path
            );
        }

        await httpContext.Response.WriteAsJsonAsync(response, JsonCommonOptions.Default.JsonSerializerOptions, cancellationToken: cancellationToken);
        return true;
    }
}