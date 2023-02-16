// Created on 10/02/2021 15:53 by Andrey Laserson

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;

namespace Shelland.ImageServer.Infrastructure.Filters;

/// <summary>
/// Generic exception filter
/// </summary>
public class ExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<ExceptionFilter> logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        this.logger = logger;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        context.ExceptionHandled = true;

        this.logger.LogError(context.Exception, "ExceptionFilter");

        if (context.Exception is AppFlowException appFlowException)
        {
            context.Result = new JsonResult(new
            {
                appFlowException.Type,
                appFlowException.Parameter
            });
        }
        else
        {
            context.Result = new JsonResult(
                new
                {
                    context.Exception.Message
                });
        }

        context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

        return Task.CompletedTask;
    }
}