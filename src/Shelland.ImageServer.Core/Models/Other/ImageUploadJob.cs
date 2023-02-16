// Created on 11/02/2021 16:18 by Andrey Laserson

using System.IO;

namespace Shelland.ImageServer.Core.Models.Other;

public record ImageUploadJob(
    Stream Stream,
    ImageUploadParamsModel Params,
    string? IpAddress
);