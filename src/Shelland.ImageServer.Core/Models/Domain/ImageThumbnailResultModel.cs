// Created on 09/02/2021 15:07 by Andrey Laserson

namespace Shelland.ImageServer.Core.Models.Domain;

public record ImageThumbnailResultModel(
    uint Width,
    uint Height,
    string DiskPath,
    string Url
);