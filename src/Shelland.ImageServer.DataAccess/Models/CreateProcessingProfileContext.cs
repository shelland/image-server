// Created on 16/02/2023 16:05 by shell

using System;
using System.Collections.Generic;
using Shelland.ImageServer.Core.Models.Domain;

namespace Shelland.ImageServer.DataAccess.Models;

public record CreateProcessingProfileContext(
    Guid Id,
    string Name,
    DateTime Now,
    IReadOnlyCollection<ImageThumbnailParamsModel> Parameters
);