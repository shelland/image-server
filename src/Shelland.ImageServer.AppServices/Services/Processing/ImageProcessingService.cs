// Created on 08/02/2021 18:26 by Andrey Laserson

using System;
using Ardalis.GuardClauses;
using ImageMagick;
using Microsoft.Extensions.Logging;
using Shelland.ImageServer.AppServices.Services.Abstract.Processing;
using Shelland.ImageServer.Core.Infrastructure.Exceptions;
using Shelland.ImageServer.Core.Infrastructure.Extensions;
using Shelland.ImageServer.Core.Models.Enums;
using Shelland.ImageServer.Core.Models.Other;

namespace Shelland.ImageServer.AppServices.Services.Processing
{
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
        public MagickImage Process(ImageProcessingJob job)
        {
            try
            {
                // Check if both sizes are valid.
                Guard.Against.PositiveCondition(job.ThumbnailParams.Width == null && job.ThumbnailParams.Height == null);

                // Clone a source image
                var image = (MagickImage) job.Image.Clone();

                // If any parameter of Resize function == 0 then another size will be used in respect with aspect ratio
                var imageSize = new MagickGeometry(job.ThumbnailParams.Width ?? 0, job.ThumbnailParams.Height ?? 0)
                {
                    IgnoreAspectRatio = job.ThumbnailParams.IsFixedSize
                };

                image.Interpolate = PixelInterpolateMethod.Bilinear;

                // Resize the image
                image.Resize(imageSize);

                // Check if any effect was requested and apply it if so
                if (job.ThumbnailParams.Effect.HasValue)
                {
                    ApplyEffect(image, job.ThumbnailParams.Effect.Value);
                }

                // Remove image metadata if such setting is set
                if (!job.Settings.KeepMetadata ?? true)
                {
                    image.Strip();
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
        public MagickImage AddWatermark(MagickImage srcImage, MagickImage watermarkImage, int opacity)
        {
            // Check if opacity is between 0 and 100
            Guard.Against.OutOfRange(opacity, nameof(opacity), 0, 100);

            watermarkImage.Evaluate(Channels.Alpha, EvaluateOperator.Multiply, (double) opacity / 100);
            srcImage.Composite(watermarkImage, Gravity.Southwest, CompositeOperator.Over);

            return srcImage;
        }

        private static void ApplyEffect(IMagickImage image, ThumbnailEffectType effect)
        {
            switch (effect)
            {
                case ThumbnailEffectType.Grayscale:

                    image.Grayscale();
                    break;

                case ThumbnailEffectType.Sepia:

                    image.SepiaTone();
                    break;
            }
        }
    }
}