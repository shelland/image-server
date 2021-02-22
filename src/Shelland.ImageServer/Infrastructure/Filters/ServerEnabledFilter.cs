// Created on 22/02/2021 20:10 by Andrey Laserson

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Shelland.ImageServer.Core.Models.Preferences;

namespace Shelland.ImageServer.Infrastructure.Filters
{
    public class ServerEnabledFilter : IAsyncActionFilter
    {
        private readonly IOptions<AppSettingsModel> options;

        public ServerEnabledFilter(IOptions<AppSettingsModel> options)
        {
            this.options = options;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var isEnabled = this.options.Value.Common.IsServerEnabled;

            if (!isEnabled)
            {
                context.HttpContext.Response.StatusCode = (int) HttpStatusCode.ServiceUnavailable;
                return;
            }

            await next();
        }
    }
}