// Created on 11/02/2021 16:18 by Andrey Laserson

using System;
using System.IO;

namespace Shelland.ImageServer.Core.Models.Other
{
    public class ImageUploadJob
    {
        public Stream Stream { get; set; }

        public ImageUploadParamsModel Params { get; set; }

        public string IpAddress { get; set; }

        public DateTimeOffset? ExpirationDate { get; set; }
    }
}