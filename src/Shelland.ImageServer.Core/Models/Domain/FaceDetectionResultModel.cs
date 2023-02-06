// Created on 03/02/2023 16:11 by shell

namespace Shelland.ImageServer.Core.Models.Domain;

public record FaceDetectionResultModel(
    int X1,
    int Y1,
    int X2,
    int Y2,
    string? ImageUrl = null
);