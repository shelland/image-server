// Created on 10/02/2021 0:44 by Andrey Laserson

using System;
using System.Collections.Generic;

namespace Shelland.ImageServer.Core.Models.Domain;

public record ImageUploadResultModel(
    Guid Id,
    IReadOnlyCollection<ImageThumbnailResultModel> Thumbnails,
    string? OriginalFileUrl
);