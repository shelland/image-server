// Created on 08/02/2021 15:59 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shelland.ImageServer.Models.Dto.Response
{
    public class ImageUploadResultDto
    {
        public Guid Id { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string OriginalFileUrl { get; set; }

        public List<ImageThumbnailResultDto> Thumbnails { get; set; } = new();
    }
}