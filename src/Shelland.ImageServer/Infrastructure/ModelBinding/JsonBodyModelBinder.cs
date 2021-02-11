// Created on 09/02/2021 16:53 by Andrey Laserson

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace Shelland.ImageServer.Infrastructure.ModelBinding
{
    /// <summary>
    /// Custom model binder to parse a JSON object from multipart request
    /// </summary>
    public class JsonBodyModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult != ValueProviderResult.None)
            {
                bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

                var stringValue = valueProviderResult.FirstValue;
                var result = JsonConvert.DeserializeObject(stringValue, bindingContext.ModelType);

                if (result != null)
                {
                    bindingContext.Result = ModelBindingResult.Success(result);
                }
            }

            return Task.CompletedTask;
        }
    }
}