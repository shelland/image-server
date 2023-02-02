// Created on 08/02/2021 15:58 by Andrey Laserson

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.Infrastructure.ModelBinding;

namespace Shelland.ImageServer.Models.Dto.Request
{
    public record ImageUploadParamsDto(
        [ModelBinder(typeof(JsonBodyModelBinder))] IReadOnlyList<ImageThumbnailParamsDto> Thumbnails,
        int? Lifetime,
        OutputImageFormat? OutputFormat,
        Guid? ProfileId
    );
}