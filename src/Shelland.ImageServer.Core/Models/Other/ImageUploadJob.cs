// Created on 11/02/2021 16:18 by Andrey Laserson

using System.IO;

namespace Shelland.ImageServer.Core.Models.Other
{
    public class ImageUploadJob
    {
        public Stream Stream { get; set; } = null!;

        public ImageUploadParamsModel Params { get; set; } = null!;

        public string? IpAddress { get; set; }
    }
}