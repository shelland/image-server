﻿// Created on 10/02/2021 0:40 by Andrey Laserson

using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.Core.Other;

namespace Shelland.ImageServer.Core.Models.Domain;

/// <summary>
/// Thumbnail processing request model
/// </summary>
public class ImageThumbnailParamsModel
{
    /// <summary>
    /// Output width
    /// </summary>
    public uint? Width { get; set; }

    /// <summary>
    /// Output height
    /// </summary>
    public uint? Height { get; set; }

    /// <summary>
    /// Effects to apply
    /// </summary>
    public ThumbnailEffectType? Effect { get; set; }

    /// <summary>
    /// Output quality (JPEG only)
    /// </summary>
    public uint? Quality { get; set; }

    /// <summary>
    /// Defines if we should ignore aspect ratio while processing
    /// </summary>
    public bool IsFixedSize => Width.HasValue && Height.HasValue;
        
    /// <summary>
    /// Watermarks params
    /// </summary>
    public WatermarkParamsModel? Watermark { get; set; }
}

public class WatermarkParamsModel
{
    /// <summary>
    /// Watermark image URL
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Default opacity
    /// </summary>
    public double Opacity { get; set; } = Constants.DefaultWatermarkOpacity;
}