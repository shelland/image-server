// Created on 08/02/2021 15:59 by Andrey Laserson

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shelland.ImageServer.Models.Dto.Response;

public record ImageUploadResultDto(
    Guid Id,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? OriginalFileUrl,
    IReadOnlyCollection<ImageThumbnailResultDto> Thumbnails
);