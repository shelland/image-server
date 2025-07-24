// Created on 08/02/2021 20:28 by Andrey Laserson

namespace Shelland.ImageServer.Core.Models.Preferences;

public class ImageProcessingSettingsModel
{
    public uint? JpegQuality { get; set; }

    public bool? KeepMetadata { get; set; }
}