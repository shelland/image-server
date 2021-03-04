// Created on 03/03/2021 17:23 by Andrey Laserson

using System.ComponentModel;

namespace Shelland.ImageServer.Core.Models.Enums
{
    /// <summary>
    /// Output image format
    /// </summary>
    public enum OutputImageFormat
    {
        [Description("jpg")]
        Jpeg,

        [Description("png")]
        Png,

        [Description("gif")]
        Gif
    }
}