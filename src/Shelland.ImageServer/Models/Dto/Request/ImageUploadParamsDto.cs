// Created on 08/02/2021 15:58 by Andrey Laserson

using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.Infrastructure.ModelBinding;

namespace Shelland.ImageServer.Models.Dto.Request
{
    public class ImageUploadParamsDto
    {
        [ModelBinder(BinderType = typeof(JsonBodyModelBinder))]
        public List<ImageThumbnailParamsDto> Thumbnails { get; set; } = new();

        public int? Lifetime { get; set; }

        public OutputImageFormat? OutputFormat { get; set; }

        public Guid? ProfileId { get; set; }
    }
}