// Created on 01/06/2021 19:19 by Andrey Laserson

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Shelland.ImageServer.Infrastructure.ModelBinding;

namespace Shelland.ImageServer.Models.Dto.Request
{
    public class ProcessingProfileDto
    {
        [Required] 
        public string Name { get; set; } = string.Empty;

        [ModelBinder(BinderType = typeof(JsonBodyModelBinder))]
        public List<ImageThumbnailParamsDto> Parameters { get; set; } = new();
    }
}