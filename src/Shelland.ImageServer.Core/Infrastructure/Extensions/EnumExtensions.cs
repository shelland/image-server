// Created on 04/03/2021 13:39 by Andrey Laserson

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Shelland.ImageServer.Core.Infrastructure.Attributes;
using Shelland.ImageServer.Core.Models.Enums;

namespace Shelland.ImageServer.Core.Infrastructure.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Returns a description associated with enum item
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum val)
    {
        return val.GetType()
                   .GetMember(val.ToString())
                   .FirstOrDefault()
                   ?.GetCustomAttribute<DescriptionAttribute>(false)
                   ?.Description
               ?? val.ToString();
    }

    public static string GetImageFormat(this OutputImageFormat val)
    {
        return val.GetType()
                   .GetMember(val.ToString())
                   .FirstOrDefault()
                   ?.GetCustomAttribute<ImageFormatAttribute>(false)
                   ?.Description
               ?? val.ToString();
    }

    public static string GetMimeType(this OutputImageFormat val)
    {
        return val.GetType()
                   .GetMember(val.ToString())
                   .FirstOrDefault()
                   ?.GetCustomAttribute<MimeTypeAttribute>(false)
                   ?.Description
               ?? val.ToString();
    }
}