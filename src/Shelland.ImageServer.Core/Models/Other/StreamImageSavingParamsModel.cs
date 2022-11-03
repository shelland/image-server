// Created on 05/03/2021 10:38 by Andrey Laserson

using System.IO;

namespace Shelland.ImageServer.Core.Models.Other
{
    public class StreamImageSavingParamsModel : BaseImageSavingParamsModel
    {
        public Stream OutputStream { get; set; } = null!;
    }
}