// Created on 11/02/2021 14:06 by Andrey Laserson

using ImageMagick;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Preferences;

namespace Shelland.ImageServer.Core.Models.Other;

/// <summary>
/// Contains information to process a requested image file
/// </summary>
public record ImageProcessingJob(
    MagickImage Image,
    ImageProcessingSettingsModel Settings,
    ImageThumbnailParamsModel ThumbnailParams
);