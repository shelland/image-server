// Created on 11/02/2021 14:06 by Andrey Laserson

using NetVips;
using Shelland.ImageServer.Core.Models.Domain;
using Shelland.ImageServer.Core.Models.Preferences;

namespace Shelland.ImageServer.Core.Models.Other;

/// <summary>
/// Contains information to process a requested image file
/// </summary>
public record ImageProcessingJob(
    Image Image,
    ImageProcessingSettingsModel Settings,
    ImageThumbnailParamsModel ThumbnailParams
);