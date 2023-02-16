// Created on 03/02/2023 16:11 by shell

using System.Text.Json.Serialization;

namespace Shelland.ImageServer.Core.Models.Domain;

public record FaceDetectionResultModel(
    int X1,
    int Y1,
    int X2,
    int Y2,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] string? ImageUrl = null
);