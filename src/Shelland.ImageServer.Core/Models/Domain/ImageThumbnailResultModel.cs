// Created on 09/02/2021 15:07 by Andrey Laserson

namespace Shelland.ImageServer.Core.Models.Domain;

public record ImageThumbnailResultModel(
    int Width,
    int Height,
    string DiskPath,
    string Url
);