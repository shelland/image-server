// Created on 09/02/2021 16:53 by Andrey Laserson

using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
using Shelland.ImageServer.Core.Models.Enums;

namespace Shelland.ImageServer.Infrastructure.ModelBinding
{
    /// <summary>
    /// Custom model binder to parse a JSON object from multipart request
    /// </summary>
    public class JsonBodyModelBinder : IModelBinder
    {
        private readonly ILogger<JsonBodyModelBinder> logger;

        public JsonBodyModelBinder(ILogger<JsonBodyModelBinder> logger)
        {
            this.logger = logger;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                if (valueProviderResult != ValueProviderResult.None)
                {
                    bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

                    var stringValue = valueProviderResult.FirstValue;

                    if (!string.IsNullOrEmpty(stringValue))
                    {
                        var result = JsonSerializer.Deserialize(stringValue, bindingContext.ModelType, JsonCommonOptions.Default.JsonSerializerOptions);

                        if (result != null)
                        {
                            bindingContext.Result = ModelBindingResult.Success(result);
                        }
                    }
                    else
                    {
                        bindingContext.Result = ModelBindingResult.Failed();
                    }
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw new AppFlowException(AppFlowExceptionType.MalformedRequest);
            }
        }
    }
}