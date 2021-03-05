// Created on 03/03/2021 17:23 by Andrey Laserson

using Shelland.ImageServer.Core.Infrastructure.Attributes;

namespace Shelland.ImageServer.Core.Models.Enums
{
    /// <summary>
    /// Output image format
    /// </summary>
    public enum OutputImageFormat
    {
        [ImageFormat("jpg")]
        [MimeType("image/jpeg")]
        Jpeg,

        [ImageFormat("png")]
        [MimeType("image/png")]
        Png,

        [ImageFormat("gif")]
        [MimeType("image/gif")]
        Gif
    }
}