// Created on 08/02/2021 18:27 by Andrey Laserson

using ImageMagick;
using Shelland.ImageServer.Core.Models.Other;
using Shelland.ImageServer.Core.Other;

namespace Shelland.ImageServer.AppServices.Services.Abstract.Processing;

/// <summary>
/// Image processing service performs a resizing of requested files
/// </summary>
public interface IImageProcessingService
{
    /// <summary>
    /// Performs an image resizing
    /// </summary>
    MagickImage Process(ImageProcessingJob job);

    /// <summary>
    /// 
    /// </summary>
    void AddWatermark(MagickImage srcImage, MagickImage watermarkImage, int opacity = Constants.DefaultWatermarkOpacity);
}