// Created on 08/02/2021 15:58 by Andrey Laserson

using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Shelland.ImageServer.Infrastructure.ModelBinding;

namespace Shelland.ImageServer.Models.Dto.Request
{
    public class ImageUploadParamsDto
    {
        [ModelBinder(BinderType = typeof(JsonBodyModelBinder))]
        public List<ImageThumbnailParamsDto> Thumbnails { get; set; }

        public int? Lifetime { get; set; }
    }
}