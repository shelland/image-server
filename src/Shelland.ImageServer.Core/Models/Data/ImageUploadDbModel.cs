﻿// Created on 08/02/2021 16:41 by Andrey Laserson

using System;
using System.Collections.Generic;
using Shelland.ImageServer.Core.Models.Base;
using Shelland.ImageServer.Core.Models.Domain;

namespace Shelland.ImageServer.Core.Models.Data;

/// <summary>
/// Database entity that represents a thumbnail processing result
/// </summary>
public class ImageUploadDbModel : BaseModel<Guid>
{
    /// <summary>
    /// User IP address
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// Original file path
    /// </summary>
    public string OriginalFilePath { get; set; } = string.Empty;

    /// <summary>
    /// Generated thumbnails
    /// </summary>
    public IReadOnlyCollection<ImageThumbnailResultModel> Thumbnails { get; set; } = null!;

    /// <summary>
    /// Expiration date
    /// </summary>
    public DateTime? ExpiresAtUtc { get; set; }
}