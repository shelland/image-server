// Created on 04/03/2021 12:41 by Andrey Laserson

using ImageMagick;
using Shelland.ImageServer.Core.Models.Enums;

namespace Shelland.ImageServer.Core.Models.Other
{
    public class ImageSavingParamsModel
    {
        public IMagickImage Image { get; set; }

        public string Path { get; set; }

        public int Quality { get; set; }

        public OutputImageFormat? Format { get; set; }
    }
}