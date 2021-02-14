// Created on 08/02/2021 15:59 by Andrey Laserson

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shelland.ImageServer.Models.Dto.Response
{
    public class ImageUploadResultDto
    {
        public Guid Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OriginalFileUrl { get; set; }

        public List<ImageThumbnailResultDto> Thumbnails { get; set; } = new();
    }
}