// Created on 17/02/2022 12:25 by shell

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Shelland.ImageServer.Infrastructure.ModelBinding;

public static class JsonCommonOptions
{
    static JsonCommonOptions()
    {
        Default = new JsonOptions
        {
            JsonSerializerOptions =
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
                Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                }
            }
        };
    }

    public static JsonOptions Default { get; }
}