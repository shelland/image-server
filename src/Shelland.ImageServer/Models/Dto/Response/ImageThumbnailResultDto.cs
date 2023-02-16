// Created on 09/02/2021 15:07 by Andrey Laserson

namespace Shelland.ImageServer.Models.Dto.Response;

public record ImageThumbnailResultDto(
    int Width,
    int Height,
    string DiskPath, 
    string Url
);