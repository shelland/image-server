// Created on 16/02/2023 13:59 by shell

using Shelland.ImageServer.Core.Models.Domain;
using System.Collections.Generic;
using System;

namespace Shelland.ImageServer.DataAccess.Models;

public record CreateImageUploadContext(
    Guid Id,
    DateTime Now,
    string OriginalFilePath,
    IReadOnlyCollection<ImageThumbnailResultModel> Thumbnails,
    string? IpAddress,
    DateTime? ExpirationDate
);