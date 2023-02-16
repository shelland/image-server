// Created on 05/03/2021 10:43 by Andrey Laserson

using Shelland.ImageServer.Core.Models.Enums;

namespace Shelland.ImageServer.Core.Models.Other;

public class BaseImageSavingParamsModel
{
    public OutputImageFormat? Format { get; set; }

    public int Quality { get; set; }
}