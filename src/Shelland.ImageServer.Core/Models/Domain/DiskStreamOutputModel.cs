// Created on 06/02/2023 11:15 by shell

using System.IO;

namespace Shelland.ImageServer.Core.Models.Domain;

public record DiskStreamOutputModel(
    Stream OutputStream,
    string FilePath
);