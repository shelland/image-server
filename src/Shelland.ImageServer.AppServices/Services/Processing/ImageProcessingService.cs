// Created on 08/02/2021 18:26 by Andrey Laserson

using System;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using NetVips;
using Shelland.ImageServer.AppServices.Services.Abstract.Processing;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.Core.Models.Other;

namespace Shelland.ImageServer.AppServices.Services.Processing;

/// <summary>
/// <inheritdoc />
/// </summary>
public class ImageProcessingService : IImageProcessingService
{
    private readonly ILogger<ImageProcessingService> logger;

    public ImageProcessingService(ILogger<ImageProcessingService> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public Image Process(ImageProcessingJob job)
    {
        try
        {
            // Check if both sizes are valid.
            Guard.Against.True(job.ThumbnailParams.Width == null && job.ThumbnailParams.Height == null);

            // Clone a source image
            var image = job.Image.ThumbnailImage(
                width: job.ThumbnailParams.Width ?? 0,
                height: job.ThumbnailParams.Height ?? 0,
                size: job.ThumbnailParams.IsFixedSize ? Enums.Size.Force : Enums.Size.Both
            );

            

            // image.Interpolate = PixelInterpolateMethod.Bilinear;

            // Resize the image
            // image.Resize(imageSize);

            // Check if any effect was requested and apply it if so
            if (job.ThumbnailParams.Effect.HasValue)
            {
                ApplyEffect(image, job.ThumbnailParams.Effect.Value);
            }

            // Remove image metadata if such setting is set
            if (!job.Settings.KeepMetadata ?? true)
            {
                // TODO
                // image.Strip();
            }

            foreach (var item in image.GetFields())
            {
                image.Resize()
            }

            return image;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, ex.Message);
            throw new AppFlowException(AppFlowExceptionType.GenericError);
        }
    }

    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public void AddWatermark(Image srcImage, Image watermarkImage, int opacity)
    {
        // Check if opacity is between 0 and 100
        Guard.Against.OutOfRange(opacity, nameof(opacity), 0, 100);

        watermarkImage.Evaluate(Channels.Alpha, EvaluateOperator.Multiply, (double)opacity / 100);
        srcImage.Composite(watermarkImage, Gravity.Southwest, CompositeOperator.Over);
    }

    private static void ApplyEffect(Image image, ThumbnailEffectType effect)
    {
        switch (effect)
        {
            case ThumbnailEffectType.Grayscale:

                image
                break;

            case ThumbnailEffectType.Sepia:

                image.SepiaTone();
                break;
        }
    }
}